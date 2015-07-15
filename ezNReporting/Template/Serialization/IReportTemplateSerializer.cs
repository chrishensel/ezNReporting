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

namespace ezNReporting.Template.Serialization
{
    /// <summary>
    /// Defines methods for a type that is able to serialize and deserialize a report to and from a sequence of bytes.
    /// </summary>
    public interface IReportTemplateSerializer
    {
        /// <summary>
        /// Creates a <see cref="IReportTemplate"/> from the specified sequence of bytes.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        IReportTemplate Deserialize(Stream content);
        /// <summary>
        /// Serializes the given report template into a sequence of bytes that can then be persisted or transferred.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        Stream Serialize(IReportTemplate template);
    }
}
