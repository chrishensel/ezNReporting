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
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using ezNReporting.Core;
using Microsoft.CSharp;

namespace ezNReporting.Scripting
{
    class CSharpScriptingProvider : IScriptingProvider
    {
        #region Methods

        private static object ExecuteScript(string script, ScriptExecutionOptions options)
        {
            object result = null;

            CompilerParameters cpar = new CompilerParameters();
            cpar.GenerateInMemory = true;
            cpar.ReferencedAssemblies.Add("mscorlib.dll");
            cpar.ReferencedAssemblies.Add("System.dll");
            cpar.ReferencedAssemblies.Add("System.Core.dll");
            cpar.ReferencedAssemblies.Add("System.Data.dll");
            cpar.ReferencedAssemblies.Add("System.Xml.dll");
            cpar.ReferencedAssemblies.Add(typeof(CSharpScriptingProvider).Assembly.Location);

            string code = Properties.Resources.ScriptProviderCSharpTemplate;
            code = code.Replace("{Source}", script);

            CSharpCodeProvider csp = new CSharpCodeProvider();
            CompilerResults res = csp.CompileAssemblyFromSource(cpar, code);

            if (!res.Errors.HasErrors)
            {
                MethodInfo func = res.CompiledAssembly.ExportedTypes.First().GetMethods().First();
                result = func.Invoke(null, new object[] { options });
            }

            return result;
        }

        #endregion

        #region IScriptingProvider Members

        bool IScriptingProvider.CanExecute(string key)
        {
            return string.Equals(key, "cs", StringComparison.OrdinalIgnoreCase);
        }

        object IScriptingProvider.Execute(string script, ScriptExecutionOptions options)
        {
            return ExecuteScript(script, options);
        }

        #endregion

        #region IExtension Members

        void IExtension.OnRegister(object host)
        {
        }

        #endregion
    }
}
