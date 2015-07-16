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

namespace ezNReporting.Engine
{
    /// <summary>
    /// Provides a factory for creating usable instances of <see cref="IReportEngine"/> objects.
    /// </summary>
    public class ReportEngineFactory : IReportEngineFactory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportEngineFactory"/> class.
        /// </summary>
        public ReportEngineFactory()
        {
        }

        #endregion

        #region IReportEngineFactory Members

        IReportEngine IReportEngineFactory.Create()
        {
            IReportEngine engine = new ReportEngine();

            /* Register default extensions.
             */
            engine.Extensions.Add(Type.GetType("ezNReporting.Scripting.CSharpScriptingProvider"));

            return engine;
        }

        #endregion
    }
}
