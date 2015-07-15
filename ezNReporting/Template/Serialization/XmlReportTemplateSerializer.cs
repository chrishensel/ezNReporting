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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ezNReporting.Core;
using ezNReporting.Data;
using ezNReporting.Template.Composition;
using ezNReporting.Template.Section;

namespace ezNReporting.Template.Serialization
{
    class XmlReportTemplateSerializer : IReportTemplateSerializer
    {
        #region Fields

        private IDependencyConsumer _dependencyConsumer;

        #endregion

        #region Constructors

        internal XmlReportTemplateSerializer(IDependencyConsumer dependencyConsumer)
        {
            if (dependencyConsumer == null)
            {
                throw new ArgumentNullException("dependencyConsumer");
            }

            _dependencyConsumer = dependencyConsumer;
        }

        #endregion

        #region Methods

        private static T GetAttribute<T>(XElement element, string name, T defaultValue)
        {
            XAttribute att = element.Attribute(name);
            if (att != null)
            {
                var conv = TypeDescriptor.GetConverter(typeof(T));
                return (T)conv.ConvertFromString(att.Value);
            }

            return defaultValue;
        }

        private static T TryCreateInstance<T>(IDependencyResolver dependencyResolver, string alias)
        {
            T instance = default(T);
            dependencyResolver.TryResolve<T>(alias, out instance);

            return instance;
        }

        #endregion

        #region IReportTemplateSerializer Members

        IReportTemplate IReportTemplateSerializer.Deserialize(Stream content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            IDependencyResolver dependencyResolver = _dependencyConsumer.DependencyResolver;

            XDocument doc = XDocument.Load(content);
            XElement root = doc.Root;

            ReportTemplate rep = new ReportTemplate();

            XElement elmInfo = root.Element("description");
            DescriptionMetadata description = new DescriptionMetadata();
            description.Name = elmInfo.Element("name").Value;
            description.Author = elmInfo.Element("author").Value;
            rep.Description = description;

            XElement elmDs = root.Element("data");
            ParseDataSourcesInto(dependencyResolver, elmDs, rep.DataSources);

            XElement elmLayout = root.Element("layout");
            ParseLayoutSectionsInto(dependencyResolver, elmLayout, rep);

            return rep;
        }

        private static void ParseDataSourcesInto(IDependencyResolver dependencyResolver, XElement node, DataSourceCollection list)
        {
            foreach (XElement elmSource in node.Elements("source"))
            {
                DataSource source = new DataSource();
                source.Name = GetAttribute(elmSource, "name", string.Empty);
                source.Provider = TryCreateInstance<IDataProvider>(dependencyResolver, GetAttribute(elmSource, "provider", string.Empty));

                foreach (XAttribute att in elmSource.Attributes())
                {
                    switch (att.Name.LocalName)
                    {
                        case "name":
                        case "provider":
                            source.Set(att.Name.LocalName, att.Value);
                            continue;
                        default:
                            source.Provider.Set(att.Name.LocalName, att.Value);
                            break;
                    }
                }

                foreach (XElement elm in elmSource.Elements())
                {
                    source.Provider.Set(elm.Name.LocalName, elm.Value);
                }

                list.Set(source);
            }
        }

        private static void ParseLayoutSectionsInto(IDependencyResolver dependencyResolver, XElement node, ReportTemplate rep)
        {
            foreach (XElement elmSection in node.Elements("section"))
            {
                SectionType type = SectionType.Invalid;
                if (!Enum.TryParse(GetAttribute(elmSection, "name", ""), true, out type))
                {
                    continue;
                }

                IReportTemplateSection section = rep.Sections.GetSection(type);

                string rootElementAlias = GetAttribute(elmSection, "rootContainer", string.Empty);
                if (string.IsNullOrWhiteSpace(rootElementAlias))
                {
                    continue;
                }

                ICompositionElement rootElement = TryCreateInstance<ICompositionElement>(dependencyResolver, rootElementAlias);
                section.RootElement = rootElement;

                CreateElementsRecursive(dependencyResolver, elmSection, rootElement);
            }
        }

        private static void CreateElementsRecursive(IDependencyResolver dependencyResolver, XElement node, ICompositionElement parent)
        {
            if (node.Elements().Any() && !parent.ChildrenSupported)
            {
                return;
            }

            foreach (XElement elmChild in node.Elements("element"))
            {
                string typeAlias = GetAttribute(elmChild, "type", string.Empty);
                if (string.IsNullOrWhiteSpace(typeAlias))
                {
                    continue;
                }

                ICompositionElement child = TryCreateInstance<ICompositionElement>(dependencyResolver, typeAlias);

                foreach (XAttribute attr in elmChild.Attributes())
                {
                    switch (attr.Name.LocalName)
                    {
                        case "type":
                            continue;
                        default:
                            {
                                child.Properties.Add(attr.Name.LocalName, attr.Value);
                            } break;
                    }
                }

                CreateElementsRecursive(dependencyResolver, elmChild, child);

                parent.AddChild(child);
            }
        }

        Stream IReportTemplateSerializer.Serialize(IReportTemplate template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            MemoryStream stream = new MemoryStream();

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

            /* Write template root.
             */
            XElement root = new XElement("template");

            /* Write description.
             */
            XElement desc = new XElement("description");
            desc.Add(new XElement("name", template.Description.Name));
            desc.Add(new XElement("author", template.Description.Author));
            root.Add(desc);

            /* Write data sources.
             */
            XElement data = new XElement("data");

            foreach (IDataSource item in template.DataSources)
            {
                XElement ds = new XElement("source");
                ds.Add(new XAttribute("name", item.Name));
                ds.Add(new XAttribute("provider", TypeKeyAttribute.GetTypeKeyOf(item.Provider)));

                foreach (string key in item.Provider.Properties.Keys)
                {
                    string value = item.Provider.Properties[key];

                    ds.Add(new XElement(key, new XCData(value)));
                }

                data.Add(ds);
            }

            root.Add(data);

            /* Write layout sections.
             */
            XElement layout = new XElement("layout");

            foreach (IReportTemplateSection item in template.Sections)
            {
                XElement section = new XElement("section");
                section.Add(new XAttribute("name", item.Type.ToString().ToLower()));
                section.Add(new XAttribute("rootContainer", TypeKeyAttribute.GetTypeKeyOf(item.RootElement)));

                WriteElement(item.RootElement, section);

                layout.Add(section);
            }

            root.Add(layout);

            doc.Add(root);

            doc.Save(stream);

            stream.Position = 0L;

            return stream;
        }

        private void WriteElement(ICompositionElement element, XElement parent)
        {
            XElement el = new XElement("element");
            el.Add(new XAttribute("type", TypeKeyAttribute.GetTypeKeyOf(element)));

            foreach (string propKey in element.Properties.Keys)
            {
                el.Add(new XAttribute(propKey, element.Properties[propKey]));
            }

            if (element.ChildrenSupported)
            {
                foreach (ICompositionElement child in element.Children)
                {
                    WriteElement(child, el);
                }
            }

            parent.Add(el);
        }

        #endregion
    }
}
