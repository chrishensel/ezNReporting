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
using System.IO;
using ezNReporting.Core;
using ezNReporting.Data;
using ezNReporting.Errors;
using ezNReporting.Export;
using ezNReporting.Template;
using ezNReporting.Template.Composition;
using ezNReporting.Template.Section;

namespace ezNReporting.Engine
{
    class ReportEngine : IReportEngine
    {
        #region Fields

        private ExtensionCollection _extensions;

        #endregion

        #region Constructors

        internal ReportEngine()
        {
            _extensions = new ExtensionCollection(this);
        }

        #endregion

        #region IReportEngine Members

        ExtensionCollection IReportEngine.Extensions
        {
            get { return _extensions; }
        }

        Stream IReportEngine.Generate(IReportTemplate template, Type exporterType)
        {
            if (exporterType == null)
            {
                throw new ArgumentNullException("exporterType");
            }

            IReportExporter exporter = (IReportExporter)Activator.CreateInstance(exporterType);

            using (IReportExporter exp = exporter)
            {
                return Generate(template, exporter);
            }
        }

        Stream IReportEngine.Generate(IReportTemplate template, IReportExporter exporter)
        {
            return Generate(template, exporter);
        }

        private Stream Generate(IReportTemplate template, IReportExporter exporter)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            if (exporter == null)
            {
                throw new ArgumentNullException("exporter");
            }

            try
            {
                return GenerateWithFault(template, exporter);
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException(ex);
            }
        }

        private Stream GenerateWithFault(IReportTemplate template, IReportExporter exporter)
        {
            IGenerationContext context = new GenerationContext() { Engine = this, Template = template };

            foreach (IDataSource item in template.DataSources)
            {
                try
                {
                    item.Provider.Initialize();
                    item.Provider.RetrieveData(context);
                }
                catch (Exception ex)
                {
                    throw new DataSourceInitializeException(item.Name, ex);
                }
            }

            foreach (IReportTemplateSection section in template.Sections)
            {
                PrepareElement(context, section.RootElement);
            }

            Stream stream = exporter.Export(context);

            stream.Position = 0L;

            return stream;
        }

        private void PrepareElement(IDataPreparationContext context, ICompositionElement element)
        {
            if (element.ChildrenSupported)
            {
                foreach (ICompositionElement child in element.Children)
                {
                    PrepareElement(context, child);
                }
            }

            IDataPreparation pv = element as IDataPreparation;
            if (pv != null)
            {
                pv.Prepare(context);
            }
        }

        #endregion
    }
}
