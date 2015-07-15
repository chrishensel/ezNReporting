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

using ezNReporting.Data;
using ezNReporting.Template.Section;

namespace ezNReporting.Template
{
    class ReportTemplate : IReportTemplate, IDataPreparationContext
    {
        #region IReportTemplate Members

        public IDescriptionMetadata Description { get; set; }
        public DataSourceCollection DataSources { get; private set; }
        public SectionCollection Sections { get; private set; }

        #endregion

        #region Constructors

        public ReportTemplate()
        {
            DataSources = new DataSourceCollection();
            Sections = new SectionCollection();
        }

        #endregion

        #region IProvideValueContext Members

        IDataContainer IDataPreparationContext.RequestDataContainer(string name)
        {
            return DataSources.GetByName(name).Provider;
        }

        #endregion
    }
}
