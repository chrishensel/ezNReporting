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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using ezNReporting.Core;
using ezNReporting.Engine;

namespace ezNReporting.Data.Provider
{
    /// <summary>
    /// Provides a <see cref="IDataProvider"/> that uses custom tables as its data source.
    /// </summary>
    [TypeKey(ThisTypeKey)]
    public class StaticDataProvider : DataProviderBase
    {
        #region Properties

        /// <summary>
        /// Gets an enumeration over all tables that have been added.
        /// </summary>
        public IEnumerable<DataTable> Tables
        {
            get
            {
                if (this.CurrentData != null)
                {
                    return this.CurrentData.Tables.Cast<DataTable>();
                }

                return null;
            }
        }

        #endregion

        #region Constants

        private const string ThisTypeKey = "static";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticDataProvider"/> class.
        /// </summary>
        public StaticDataProvider()
            : base()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new table with data.
        /// </summary>
        /// <param name="table">The table to add.</param>
        public void AddTable(DataTable table)
        {
            if (CurrentData == null)
            {
                CurrentData = new DataSet();
            }

            CurrentData.Tables.Add(table);
        }

        /// <summary>
        /// Adds a new row to the table at the given index.
        /// </summary>
        /// <param name="tableIndex">The zero-based index of the table to add a new row to.</param>
        /// <param name="values">The values to add. Their types must correspond to the columns used in the specified table.</param>
        /// <returns>A reference to this instance.</returns>
        public StaticDataProvider AddRow(int tableIndex, params object[] values)
        {
            DataTable table = this.Tables.ElementAt(tableIndex);

            table.LoadDataRow(values, LoadOption.Upsert);

            return this;
        }

        /// <summary>
        /// The implementation does nothing, as data is provided by the user.
        /// </summary>
        /// <param name="context"></param>
        protected override void RetrieveData(IGenerationContext context)
        {

        }

        #endregion
    }
}
