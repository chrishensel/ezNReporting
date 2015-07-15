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

using System.IO;
using ezNReporting.Core;

namespace ezNReporting.Template
{
    /// <summary>
    /// Defines methods that can create ready-to-use instances of <see cref="IReportTemplate"/>s.
    /// </summary>
    public interface IReportTemplateFactory : IDependencyConsumer
    {
        /// <summary>
        /// Creates a new, blank report template using only the provided description metadata.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IReportTemplate Create(IDescriptionMetadata description);
        /// <summary>
        /// Creates a report template from the provided stream containing a supported report template file.
        /// </summary>
        /// <param name="templateStream"></param>
        /// <returns></returns>
        IReportTemplate Create(Stream templateStream);
        /// <summary>
        /// Serializes the provided report template to a format that can be deserialized by the <see cref="Create(Stream)"/> method.
        /// </summary>
        /// <param name="template">The template to serialize.</param>
        /// <returns>A stream containing the serialized template.</returns>
        Stream Save(IReportTemplate template);
    }
}
