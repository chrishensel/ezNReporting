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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ezNReporting.Data;
using ezNReporting.Engine;
using ezNReporting.Template.Composition;
using ezNReporting.Template.Section;

namespace ezNReporting.Export
{
    /// <summary>
    /// Provides a report exporter that generates XHTML-files from report templates.
    /// See documentation for further information.
    /// </summary>
    /// <remarks>This report exporter only exports the "detail" section - all other sections are ignored.</remarks>
    public class XHtmlReportExporter : ReportExporterBase
    {
        #region Properties

        /// <summary>
        /// Gets/sets a stream containing the cascading stylesheet (CSS) that will be inserted into the resulting document, if any.
        /// </summary>
        public Stream Css { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Stream Export(IGenerationContext context)
        {
            IReportTemplateSection sd = context.Template.Sections.GetSection(SectionType.Detail);

            XDocument doc = new XDocument();
            doc.Add(new XElement("html"));

            XElement elmHead = new XElement("head");
            doc.Root.Add(elmHead);
            elmHead.Add(new XElement("title", context.Template.Description.Name));

            XElement elmBody = new XElement("body");
            doc.Root.Add(elmBody);

            string css = GetCssContent();
            if (!string.IsNullOrWhiteSpace(css))
            {
                XElement elmStyle = new XElement("style");
                elmBody.Add(elmStyle);
                elmStyle.Add(new XText(css));
            }

            WriteElement(sd.RootElement, elmBody);

            MemoryStream ms = new MemoryStream();
            doc.Save(ms);

            return ms;
        }

        private void WriteElement(ICompositionElement element, XElement parent)
        {
            XElement el = CreateTag(TagType.Div);

            if (element.ChildrenSupported)
            {
                foreach (ICompositionElement child in element.Children)
                {
                    WriteElement(child, el);
                }
            }

            IScalarValueProducer singleValue = element as IScalarValueProducer;
            if (singleValue != null)
            {
                XElement elValue = CreateTag(TagType.Span);
                elValue.Add(new XAttribute("class", "value"));
                elValue.Add(singleValue.Value);

                el.Add(elValue);
            }
            else
            {
                IMultipleRowsProducer rows = element as IMultipleRowsProducer;
                if (rows != null)
                {
                    XElement elTable = CreateTag(TagType.Table);
                    XElement elTableHeader = null;

                    foreach (DataRow row in rows.Rows)
                    {
                        if (elTableHeader == null)
                        {
                            elTableHeader = CreateTag(TagType.TableHeader);
                            XElement elTableHeaderRow = CreateTag(TagType.TableRow);

                            foreach (DataColumn col in row.Table.Columns)
                            {
                                elTableHeaderRow.Add(new XElement(GetTagName(TagType.TableColumn), new XText(col.ColumnName)));
                            }

                            elTableHeader.Add(elTableHeaderRow);

                            elTable.Add(elTableHeader);
                        }

                        XElement elRow = CreateTag(TagType.TableRow);

                        foreach (object colValue in row.ItemArray)
                        {
                            XElement elColValue = CreateTag(TagType.Span);
                            elColValue.Add(new XAttribute("class", "value"));
                            elColValue.Add(colValue);

                            elRow.Add(new XElement(GetTagName(TagType.TableColumn), elColValue));
                        }

                        elTable.Add(elRow);
                    }

                    el.Add(elTable);
                }
                else
                {
                    if (element.Classification.HasFlag(ElementClassifications.Separator))
                    {
                        el.Name = GetTagName(TagType.HorizontalRule);
                    }

                    double height = element.GetProperty("height", double.NaN);
                    if (!double.IsNaN(height))
                    {
                        ApplyClass(el, "height-" + height.ToString());
                    }
                }
            }

            parent.Add(el);
        }

        /// <summary>
        /// Applies a value to the 'class=""' attribute to the given element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="className">The class name to append.</param>
        protected void ApplyClass(XElement element, string className)
        {
            XAttribute cls = element.Attribute("class");
            if (cls == null)
            {
                cls = new XAttribute("class", "");
                element.Add(cls);
            }

            IList<string> classes = cls.Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (!classes.Contains(className))
            {
                classes.Add(className);

                cls.Value = string.Join(" ", classes);
            }
        }

        private string GetCssContent()
        {
            if (Css != null)
            {
                using (StreamReader reader = new StreamReader(this.Css, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }

            return null;
        }

        private XElement CreateTag(TagType tag)
        {
            XElement element = new XElement(GetTagName(tag));

            return element;
        }

        private string GetTagName(TagType tag)
        {
            switch (tag)
            {
                case TagType.Div: return "div";
                case TagType.Span: return "span";
                case TagType.Table: return "table";
                case TagType.TableHeader: return "thead";
                case TagType.TableRow: return "tr";
                case TagType.TableColumn: return "td";
                case TagType.HorizontalRule: return "hr";
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

        #region Nested types

        enum TagType
        {
            None = 0,
            Div,
            Span,
            Table,
            TableHeader,
            TableRow,
            TableColumn,
            HorizontalRule,
        }

        #endregion
    }
}
