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

using System.Data;
using System.IO;
using System.Linq;
using AODL.Document.Content.Tables;
using AODL.Document.SpreadsheetDocuments;
using ezNReporting.Data;
using ezNReporting.Engine;
using ezNReporting.Export;
using ezNReporting.Template.Section;

namespace ezNReporting.Exporter.Odf
{
    /// <summary>
    /// Exports reports to the ODS (OpenDocument Spreadsheet) format.
    /// </summary>
    public class OdsDocumentExporter : OdfExporterBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OdsDocumentExporter"/> class.
        /// </summary>
        public OdsDocumentExporter()
            : base("ods")
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Exports the provided report template to an ODS document.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Stream Export(IGenerationContext context)
        {
            IReportTemplateSection sdet = context.Template.Sections.GetSection(SectionType.Detail);

            SpreadsheetDocument doc = new SpreadsheetDocument();
            doc.New();

            IMultipleRowsProducer producer = sdet.RootElement.FindFirstMultipleRowsProducer();

            if (producer != null)
            {
                Table table = TableBuilder.CreateSpreadsheetTable(doc, "Table", string.Empty);
                doc.Content.Add(table);

                WriteTable(producer, table, context);
            }

            return CreateStream(doc);
        }

        private void WriteTable(IMultipleRowsProducer rp, Table table, IGenerationContext context)
        {
            DataRow[] rows = rp.GetValue(context).ToArray();
            if (rows.Length > 0)
            {
                DataTable ptab = rows.First().Table;

                for (int iCol = 0; iCol < ptab.Columns.Count; iCol++)
                {
                    table.ColumnCollection.Add(new Column(table, string.Empty));
                }

                for (int iRow = -1; iRow < rows.Length; iRow++)
                {
                    Row row = new Row(table);
                    table.RowCollection.Add(row);

                    for (int iCol = 0; iCol < ptab.Columns.Count; iCol++)
                    {
                        Cell cell = new Cell(table);
                        object value = (iRow == -1) ? ptab.Columns[iCol].ColumnName : rows[iRow][iCol];

                        CreateAddSimpleText(table.Document, cell.Content, value);

                        row.CellCollection.Add(cell);
                    }
                }
            }
        }

        #endregion
    }
}
