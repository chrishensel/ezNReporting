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
using System.IO;
using System.Linq;
using AODL.Document.Content;
using AODL.Document.Content.Tables;
using AODL.Document.Content.Text;
using AODL.Document.TextDocuments;
using ezNReporting.Data;
using ezNReporting.Engine;
using ezNReporting.Export;
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
    public class OdtDocumentExporter : ReportExporterBase
    {
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

            WriteElement(sdet.RootElement, doc);

            FileInfo tmp = new FileInfo(Path.GetTempFileName().Replace(".tmp", ".odt"));
            using (tmp.Create()) { }

            doc.SaveTo(tmp.FullName);

            MemoryStream stream = new MemoryStream();
            using (FileStream fs = tmp.OpenRead())
            {
                fs.CopyTo(stream);
            }

            tmp.Delete();

            return stream;
        }

        private void WriteElement(ICompositionElement element, TextDocument doc)
        {
            if (element.ChildrenSupported)
            {
                foreach (ICompositionElement child in element.Children)
                {
                    WriteElement(child, doc);
                }
            }

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
                    DataRow[] rows = rp.Rows.ToArray();
                    if (rows.Length > 0)
                    {
                        DataTable ptab = rows.First().Table;

                        Table table = TableBuilder.CreateTextDocumentTable(doc, ptab.TableName, string.Empty, rows.Length + 1, ptab.Columns.Count, 0d, true, true);
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
            }
        }

        private static void CreateAddSimpleText(TextDocument doc, IContentCollection content, object value)
        {
            Paragraph p = ParagraphBuilder.CreateStandardTextParagraph(doc);
            p.TextContent.Add(new SimpleText(doc, Convert.ToString(value ?? string.Empty)));

            content.Add(p);
        }

        #endregion
    }
}
