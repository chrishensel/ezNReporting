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
using System.Text;
using ezNReporting.Data;
using ezNReporting.Engine;
using ezNReporting.Template.Section;

namespace ezNReporting.Export
{
    /// <summary>
    /// Provides a report exporter that exports the first occurrence of a <see cref="IMultipleRowsProducer"/> to a CSV file.
    /// See documentation for further information.
    /// </summary>
    /// <remarks>This report exporter is useful to generate straightforward CSV files that can the be used as a data interchange file or for a quick overview over some data.</remarks>
    public class CsvReportExporter : ReportExporterBase
    {
        #region Methods

        /// <summary>
        /// Exports the specified report template in a CSV-format.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Stream Export(IGenerationContext context)
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            ExportSection(writer, context.Template.Sections.GetSection(SectionType.Detail));

            writer.Flush();

            return stream;
        }

        private void ExportSection(TextWriter writer, IReportTemplateSection section)
        {
            /* This searches for the first occurrence of a multiple rows producer.
             * CSV format is very limited and it makes no sense to export more tables or just a single value.
             */

            IMultipleRowsProducer producer = section.RootElement.FindFirstMultipleRowsProducer();

            if (producer != null)
            {
                DataRow[] rows = producer.Rows.ToArray();

                if (rows.Length > 0)
                {
                    /* Write header row.
                     */
                    writer.WriteLine(string.Join(";", rows.First().Table.Columns.Cast<DataColumn>().Select(_ => _.ColumnName)));

                    /* Write data.
                     */
                    foreach (DataRow row in rows)
                    {
                        writer.WriteLine(string.Join(";", row.ItemArray));
                    }
                }
            }
        }

        #endregion
    }
}
