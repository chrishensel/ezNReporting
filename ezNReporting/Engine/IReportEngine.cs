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
using ezNReporting.Export;
using ezNReporting.Template;

namespace ezNReporting.Engine
{
    /// <summary>
    /// Defines members for a report engine, which is the integral component that generates a single report.
    /// </summary>
    public interface IReportEngine
    {
        /// <summary>
        /// Gets a container that holds instances of <see cref="IExtension"/> types.
        /// </summary>
        ExtensionCollection Extensions { get; }

        /// <summary>
        /// Generates the provided report, using the specified exporter type.
        /// </summary>
        /// <param name="template">The template to use.</param>
        /// <param name="exporterType">The type of the exporter to use.</param>
        /// <returns>A stream containing the generated report.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="template"/> or <paramref name="exporterType"/> were null.</exception>
        Stream Generate(IReportTemplate template, Type exporterType);
        /// <summary>
        /// Generates the provided report, using the specified exporter.
        /// </summary>
        /// <param name="template">The template to use.</param>
        /// <param name="exporter">The to use.</param>
        /// <returns>A stream containing the generated report.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="template"/> or <paramref name="exporter"/> were null.</exception>
        Stream Generate(IReportTemplate template, IReportExporter exporter);
    }
}
