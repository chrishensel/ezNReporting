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
using ezNReporting.Template.Serialization;

namespace ezNReporting.Template
{
    /// <summary>
    /// Provides a factory for creating usable instances of <see cref="IReportTemplate"/> objects.
    /// </summary>
    public class ReportTemplateFactory : IReportTemplateFactory
    {
        #region Fields

        private IDependencyResolver _dependencyResolver;
        private IReportTemplateSerializer _serializer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplateFactory"/> class.
        /// </summary>
        public ReportTemplateFactory()
        {
            _dependencyResolver = new AliasDependencyResolver();
            _serializer = new XmlReportTemplateSerializer(this);
        }

        #endregion

        #region IReportTemplateFactory Members

        IReportTemplate IReportTemplateFactory.Create(IDescriptionMetadata description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            ReportTemplate tmpl = new ReportTemplate();
            tmpl.Description = description;

            return tmpl;
        }

        IReportTemplate IReportTemplateFactory.Create(Stream templateStream)
        {
            if (templateStream == null)
            {
                throw new ArgumentNullException("templateStream");
            }

            return _serializer.Deserialize(templateStream);
        }

        Stream IReportTemplateFactory.Save(IReportTemplate template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            return _serializer.Serialize(template);
        }

        #endregion

        #region IDependencyConsumer Members

        IDependencyResolver IDependencyConsumer.DependencyResolver
        {
            get { return _dependencyResolver; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _dependencyResolver = value;
            }
        }

        #endregion
    }
}
