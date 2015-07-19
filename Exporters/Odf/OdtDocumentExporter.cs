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
using AODL.Document;
using AODL.Document.Content.Tables;
using AODL.Document.TextDocuments;
using ezNReporting.Data;
using ezNReporting.Engine;
using ezNReporting.Template.Composition;
using ezNReporting.Template.Section;

/* See also the following link for an example of how to use the AODL library.
 * https://wiki.openoffice.org/wiki/AODL_example_17
 * 
 * Hint: currently, the exporter requires admin rights (at least when using the Web app).
 * We need to figure out why the AODL library does need to write to an "aodlwrite" directory, which is annoying!
 *
 */

namespace ezNReporting.Exporter.Odf
{
    /// <summary>
    /// Exports reports to the ODT (OpenDocument Text) format.
    /// </summary>
    public class OdtDocumentExporter : OdfExporterBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OdtDocumentExporter"/> class.
        /// </summary>
        public OdtDocumentExporter()
            : base("odt")
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Exports the provided report template to an ODT document.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Stream Export(IGenerationContext context)
        {
            IReportTemplateSection sdet = context.Template.Sections.GetSection(SectionType.Detail);

            TextDocument doc = new TextDocument();
            doc.New();

            doc.DocumentConfigurations2 = null;

            doc.DocumentMetadata.Creator = context.Template.Description.Author;
            doc.DocumentMetadata.Title = context.Template.Description.Name;

            WriteElement(sdet.RootElement, doc);

            return CreateStream(doc);
        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="doc"></param>
        /// <param name="level"></param>
        protected override void OnWriteElement(ICompositionElement element, IDocument doc, int level)
        {
            IScalarValueProducer singleValue = element as IScalarValueProducer;
            if (singleValue != null)
            {
                CreateAddSimpleText(doc, doc.Content, singleValue.Value);
            }
            else
            {
                IMultipleRowsProducer rp = element as IMultipleRowsProducer;
                if (rp != null)
                {
                    WriteTable(rp, doc);
                }
            }
        }

        private void WriteTable(IMultipleRowsProducer rp, IDocument doc)
        {
            DataRow[] rows = rp.Rows.ToArray();
            if (rows.Length > 0)
            {
                DataTable ptab = rows.First().Table;

                Table table = TableBuilder.CreateTextDocumentTable((TextDocument)doc, ptab.TableName, string.Empty, rows.Length + 1, ptab.Columns.Count, 0d, true, true);
                CellCollection tableCells = table.RowHeader.RowCollection[0].CellCollection;

                for (int i = 0; i < tableCells.Count; i++)
                {
                    CreateAddSimpleText(doc, tableCells[i].Content, ptab.Columns[i].ColumnName);
                }

                for (int i = 0; i < table.RowCollection.Count; i++)
                {
                    tableCells = table.RowCollection[i].CellCollection;

                    for (int j = 0; j < tableCells.Count; j++)
                    {
                        CreateAddSimpleText(doc, tableCells[j].Content, rows[i][j]);
                    }
                }

                doc.Content.Add(table);
            }
        }

        #endregion
    }
}
