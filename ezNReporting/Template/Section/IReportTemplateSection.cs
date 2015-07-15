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

using ezNReporting.Template.Composition;

namespace ezNReporting.Template.Section
{
    /// <summary>
    /// Defines members of a section, which is a logical grouping within a report.
    /// </summary>
    public interface IReportTemplateSection
    {
        /// <summary>
        /// Gets the section type.
        /// </summary>
        SectionType Type { get; }
        /// <summary>
        /// Gets/sets an element representing the root container of this section.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The root element is being set to a null value.</exception>
        ICompositionElement RootElement { get; set; }
    }
}
