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
using System.Linq;
using ezNReporting.Core;
using ezNReporting.Scripting;
using Jint;

namespace JavaScriptProvider
{
    /// <summary>
    /// Provides a scripting provider that executes JavaScript scripts.
    /// </summary>
    public class JavaScriptProvider : IScriptingProvider
    {
        #region IScriptingProvider Members

        bool IScriptingProvider.CanExecute(string key)
        {
            return string.Equals(key, "js", StringComparison.OrdinalIgnoreCase);
        }

        object IScriptingProvider.Execute(string script, ScriptExecutionOptions options)
        {
            ScriptContext context = new ScriptContext(options);

            Engine eng = new Engine(_ => _.Strict());
            eng.SetValue("context", context);

            object v = eng.Execute(script);

            if (context.ret != null)
            {
                return context.ret;
            }

            return v;
        }

        #endregion

        #region IExtension Members

        void IExtension.OnRegister(object host)
        {
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Represents the type that contains data that can be used inside the script file via the "context" global property.
        /// </summary>
        public sealed class ScriptContext
        {
            #region Properties

            /// <summary>
            /// Gets an optional array of parameters that may be given to the script.
            /// </summary>
            public object[] @params { get; private set; }
            /// <summary>
            /// Gets the instance containing the return value.
            /// </summary>
            public object ret { get; private set; }

            #endregion

            #region Constructors

            internal ScriptContext(ScriptExecutionOptions options)
            {
                this.@params = (options.Parameters != null) ? options.Parameters.ToArray() : new object[0];

                this.ret = new object();

                if (options.DesiredReturnValueType != null)
                {
                    this.ret = Activator.CreateInstance(options.DesiredReturnValueType);
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Sets the return value. The script file should call this value also to indicate that it has completed.
            /// </summary>
            /// <param name="value"></param>
            public void setReturn(object value)
            {
                this.ret = value;
            }

            #endregion
        }

        #endregion
    }
}
