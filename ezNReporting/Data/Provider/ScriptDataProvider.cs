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
using System.Data;
using System.Linq;
using ezNReporting.Core;
using ezNReporting.Engine;
using ezNReporting.Scripting;

namespace ezNReporting.Data.Provider
{
    /// <summary>
    /// Implements a data provider that executes a custom script to return its data.
    /// </summary>
    [TypeKey(ThisTypeKey)]
    public class ScriptDataProvider : DataProviderBase
    {
        #region Constants

        private const string ThisTypeKey = "script";

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the type key of the script to execute.
        /// See documentation for further information.
        /// </summary>
        /// <remarks>Examples include 'cs' for C# code or 'js' for JavaScript code.</remarks>
        public string ScriptTypeKey
        {
            get { return Properties["script-type"]; }
            set { Properties["script-type"] = value; }
        }

        /// <summary>
        /// Gets/sets the script text to execute.
        /// </summary>
        public string ScriptText
        {
            get { return Properties["text"]; }
            set { Properties["text"] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptDataProvider"/> class.
        /// </summary>
        public ScriptDataProvider()
            : base()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches for a <see cref="IScriptingProvider"/> that can execute the script type as stated in <see cref="ScriptTypeKey"/> and invokes the script.
        /// </summary>
        /// <param name="context"></param>
        protected override void RetrieveData(IGenerationContext context)
        {
            IScriptingProvider[] matchingProviders = context.Engine.Extensions.Get<IScriptingProvider>().Where(_ => _.CanExecute(this.ScriptTypeKey)).ToArray();

            /* We need at least one matching provider. If there is more than one matching provider, take the first one and maybe omit warning.
             */
            if (matchingProviders.Length > 0)
            {
                IScriptingProvider prov = matchingProviders.First();

                ScriptExecutionOptions options = new ScriptExecutionOptions();
                options.AssociatedDataProvider = this;
                options.DesiredReturnValueType = typeof(DataSet);

                try
                {
                    object ret = prov.Execute(this.ScriptText, options);

                    DataSet ds = ret as DataSet;

                    if (ds != null)
                    {
                        this.CurrentData = ds;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion
    }
}
