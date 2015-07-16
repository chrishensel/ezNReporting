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
using ezNReporting.Engine;

namespace ezNReporting.Export
{
    /// <summary>
    /// Provides an abstract base class for building a report exporter. 
    /// </summary>
    public abstract class ReportExporterBase : IReportExporter
    {
        #region IReportExporter Members
        
        Stream IReportExporter.Export(IGenerationContext context)
        {
            return Export(context);
        }

        /// <summary>
        /// Exports the specified report.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Stream Export(IGenerationContext context);

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {

        }

        #endregion
    }
}
