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
using ezNReporting.Data.Provider;
using ezNReporting.Engine;

namespace ezNReporting.Web.Utilities.ezNReporting
{
    class RandomDataProvider : DataProviderBase
    {
        #region Methods

        protected override void RetrieveData(IGenerationContext context)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Value");

            Random rnd = new Random();

            for (int i = 0; i < 100; i++)
            {
                table.LoadDataRow(new object[] { rnd.Next() }, true);
            }

            this.CurrentData = new DataSet();
            this.CurrentData.Tables.Add(table);
        }

        #endregion
    }
}