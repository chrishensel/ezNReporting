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
using ezNReporting.Core;
using ezNReporting.Data;

namespace ezNReporting.Template.Composition.Elements
{
    /// <summary>
    /// Represents a table element that generates a single line for each row.
    /// </summary>
    [TypeKey(ThisTypeKey)]
    public class TableElement : CompositionElementBase, IMultipleRowsProducer
    {
        #region Constants

        private const string ThisTypeKey = "table";

        #endregion

        #region Fields

        private DataSet _data;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the name of the data source from which the table acquires the data to display.
        /// </summary>
        public string DataSource
        {
            get { return Properties["data-source"]; }
            set { Properties["data-source"] = value; }
        }

        /// <summary>
        /// Gets/sets the name of the <see cref="DataTable"/> to use for returning its <see cref="DataRow"/> contents.
        /// If this is null, the element tries to get the data from the first table.
        /// </summary>
        public string DataTableName
        {
            get { return Properties["data-table"]; }
            set { Properties["data-table"] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElement"/> class.
        /// </summary>
        public TableElement()
            : base(false)
        {
            this.Classification = ElementClassifications.Table;
        }

        #endregion

        #region IMultipleRowsProducer Members

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="context"></param>
        protected override void Prepare(IDataPreparationContext context)
        {
            base.Prepare(context);

            _data = context.RequestDataContainer(DataSource).Data;
        }

        IEnumerable<DataRow> IMultipleRowsProducer.Rows
        {
            get { return GetRowsData(); }
        }

        private IEnumerable<DataRow> GetRowsData()
        {
            DataTable table = null;

            if (_data != null && _data.Tables.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(DataTableName))
                {
                    table = _data.Tables[DataTableName];
                }
                else
                {
                    table = _data.Tables[0];
                }
            }

            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    yield return row;
                }
            }
        }

        #endregion
    }
}
