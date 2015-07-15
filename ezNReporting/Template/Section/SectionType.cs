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


namespace ezNReporting.Template.Section
{
    /// <summary>
    /// Specifies the section types that are supported in a template.
    /// See documentation for further information.
    /// </summary>
    /// <remarks>Please be aware that not all sections are required to be exported by any exporter.
    /// Some exporters may ignore certain sections and also format them differently.
    /// Please see the documentation of the specific exporter for details.</remarks>
    public enum SectionType
    {
        /// <summary>
        /// Enumeration default value; don't use.
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// Identifies the detail section of a template.
        /// See documentation for further information.
        /// </summary>
        /// <remarks>The detail section is typically where most data gets exported to.
        /// It identifies the section between the page header and footer (in traditional, page-oriented exporters).</remarks>
        Detail = 1,
    }
}
