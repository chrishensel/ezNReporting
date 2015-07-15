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
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Web;

namespace ezNReporting.Web.Data
{
    class DataRepository : IDataRepository
    {
        #region Constants

        private const string DatabasePath = "App_Data/database.sqlite3";

        #endregion

        #region Fields

        private static SQLiteFactory _factory;
        private static string _connectionString;

        private static SQLiteConnection _connection;

        #endregion

        #region Constructors

        internal static void Initialize(HttpServerUtility server)
        {
            _factory = new SQLiteFactory();

            var builder = (SQLiteConnectionStringBuilder)_factory.CreateConnectionStringBuilder();
            builder.DataSource = server.MapPath(DatabasePath);
            builder.FailIfMissing = false;

            _connectionString = builder.ConnectionString;

            _connection = (SQLiteConnection)_factory.CreateConnection();
            _connection.ConnectionString = _connectionString;
            _connection.Open();

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS report (id INTEGER PRIMARY KEY AUTOINCREMENT, guid TEXT NOT NULL, name TEXT NOT NULL, created_at INTEGER NOT NULL, created_by TEXT NOT NULL, contents BLOB NOT NULL)";
                cmd.ExecuteNonQuery();
            }
        }

        internal DataRepository()
        {
        }

        #endregion

        #region Methods

        private static ReportData DataRowToReport(DataRow row, bool includeDefinition)
        {
            ReportData report = new ReportData();

            try
            {
                report.Id = (int)row.Field<long>("id");
                report.Guid = new Guid(row.Field<string>("guid"));
                report.Name = row.Field<string>("name");
                report.CreatedAt = new DateTimeOffset(row.Field<long>("created_at"), TimeSpan.Zero);
                report.CreatedBy = row.Field<string>("created_by");

                if (includeDefinition)
                {
                    report.Definition = Encoding.UTF8.GetString(row.Field<byte[]>("contents"));
                }
            }
            catch (Exception)
            {
                report.State = EntityState.Erroneous;
            }

            return report;
        }

        #endregion

        #region IDataRepository Members

        void IDataRepository.Add(ReportData report)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO report(guid, name, created_at, created_by, contents) VALUES(@1, @2, @3, @4, @5);";

                cmd.Parameters.AddWithValue("@1", report.Guid.ToString());
                cmd.Parameters.AddWithValue("@2", report.Name);
                cmd.Parameters.AddWithValue("@3", report.CreatedAt.UtcTicks);
                cmd.Parameters.AddWithValue("@4", report.CreatedBy);
                cmd.Parameters.AddWithValue("@5", report.Definition);

                cmd.ExecuteNonQuery();
            }
        }

        ReportData IDataRepository.Get(string guid)
        {
            DataTable result = new DataTable();

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM report WHERE guid = @1;";
                cmd.Parameters.AddWithValue("@1", guid);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    result.Load(reader);
                }
            }

            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];

                return DataRowToReport(row, true);
            }

            return null;
        }

        IEnumerable<ReportData> IDataRepository.GetReports(int lastId, int count, bool ascending)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count", "count must be greater or equal than one (1)!");
            }

            DataTable result = new DataTable();

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM report !LASTID ORDER BY created_at !ORDER LIMIT @limit;";

                cmd.CommandText = cmd.CommandText.Replace("!LASTID", (lastId > 0) ? "WHERE id > @lastid" : string.Empty);
                cmd.CommandText = cmd.CommandText.Replace("!ORDER", ascending ? "ASC" : "DESC");

                cmd.Parameters.AddWithValue("@lastid", lastId);
                cmd.Parameters.AddWithValue("@limit", count);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    result.Load(reader);
                }
            }

            foreach (DataRow row in result.Rows)
            {
                yield return DataRowToReport(row, false);
            }
        }

        void IDataRepository.Delete(string guid)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM report WHERE guid = @1;";
                cmd.Parameters.AddWithValue("@1", guid);

                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {

        }

        #endregion
    }
}