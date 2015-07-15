// Copyright 2015 Sascha-Christian Hensel
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Data;
using ezNReporting.Core;

namespace ezNReporting.Data.Provider
{
    /// <summary>
    /// Provides a data provider that uses a <see cref="IDbConnection"/> to query its data from.
    /// </summary>
    [TypeKey(ThisTypeKey)]
    public class DbDataProvider : DataProviderBase
    {
        #region Constants

        private const string ThisTypeKey = "db";

        #endregion

        #region Fields

        private bool _wasExplicitlyCreated;
        private string[] _queries;

        #endregion

        #region Properties

        public string ConnectionString
        {
            get { return this.Properties["connectionString"]; }
            set { this.Properties["connectionString"] = value; }
        }

        public string ConnectionType
        {
            get { return this.Properties["connectionType"]; }
            set { this.Properties["connectionType"] = value; }
        }

        public string Queries
        {
            get { return this.Properties["queries"]; }
            set { this.Properties["queries"] = value; }
        }

        /// <summary>
        /// Gets/sets the connection to use for querying data.
        /// See documentation for further information.
        /// </summary>
        /// <remarks>Ideally, the connection is already open when assigned to this property.
        /// You can open it at a later point, however you risk an exception when the data is being queried
        /// and the connection hadn't been opened.</remarks>
        public IDbConnection Connection { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataProvider"/> class.
        /// </summary>
        public DbDataProvider()
            : base()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overridden to find out which queries and options are needed.
        /// </summary>
        /// <param name="parameters"></param>
        protected override void Initialize()
        {
            base.Initialize();

            string queriesRaw = this.Queries;
            if (!string.IsNullOrWhiteSpace(queriesRaw))
            {
                _queries = queriesRaw.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (this.Connection == null)
            {
                string connectionString = this.ConnectionString;
                string connectionTypeName = this.ConnectionType;

                if (!string.IsNullOrWhiteSpace(connectionString) && !string.IsNullOrWhiteSpace(connectionTypeName))
                {
                    this.Connection = (IDbConnection)Activator.CreateInstance(Type.GetType(connectionTypeName));
                    this.Connection.ConnectionString = connectionString;

                    _wasExplicitlyCreated = true;
                }
            }
        }

        /// <summary>
        /// Overridden to retrieve the data from the specified queries.
        /// </summary>
        protected override void RetrieveData()
        {
            if (_queries == null)
            {
                return;
            }

            try
            {
                DataSet ds = new DataSet();

                if (_wasExplicitlyCreated && this.Connection.State == ConnectionState.Closed)
                {
                    this.Connection.Open();
                }

                foreach (string query in _queries)
                {
                    using (IDbCommand command = Connection.CreateCommand())
                    {
                        command.CommandText = query;

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            ds.Tables.Add(CreateDataTable(reader));
                        }
                    }
                }

                this.CurrentData = ds;
            }
            finally
            {
                if (_wasExplicitlyCreated && this.Connection != null)
                {
                    this.Connection.Close();
                    this.Connection.Dispose();
                }
            }
        }

        private DataTable CreateDataTable(IDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();

            DataTable data = new DataTable();

            foreach (DataRow row in schema.Rows)
            {
                DataColumn col = new DataColumn(Convert.ToString(row["ColumnName"]));

                data.Columns.Add(col);

                col.SetOrdinal(Convert.ToInt32(row["ColumnOrdinal"]));
            }

            while (reader.Read())
            {
                DataRow row = data.NewRow();

                foreach (DataColumn col in data.Columns)
                {
                    row.SetField(col.ColumnName, reader.GetValue(col.Ordinal));
                }

                data.Rows.Add(row);
            }

            return data;
        }

        #endregion
    }
}
