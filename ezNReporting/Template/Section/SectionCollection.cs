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

using System.Collections;
using System.Linq;

namespace ezNReporting.Template.Section
{
    /// <summary>
    /// Provides a specialized, read-only collection that holds <see cref="IReportTemplateSection"/> objects.
    /// </summary>
    public sealed class SectionCollection : ReadOnlyCollectionBase
    {
        #region Constructors

        internal SectionCollection()
            : base()
        {
            this.InnerList.Add(new ReportTemplateSection() { Type = SectionType.Detail });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the section of the given type.
        /// </summary>
        /// <param name="type">The section type to return.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">The requested section wasn't found.</exception>
        public IReportTemplateSection GetSection(SectionType type)
        {
            return this.InnerList.Cast<IReportTemplateSection>().Single(_ => _.Type == type);
        }

        #endregion
    }
}
