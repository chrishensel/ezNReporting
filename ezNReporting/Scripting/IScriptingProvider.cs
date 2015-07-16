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

using ezNReporting.Core;

namespace ezNReporting.Scripting
{
    /// <summary>
    /// Defines the interface for a type that is able to execute user scripts and return its result.
    /// </summary>
    public interface IScriptingProvider : IExtension
    {
        /// <summary>
        /// Determines whether or not this provider can execute a script that is associated with a key.
        /// </summary>
        /// <param name="key">A string representing the key (or 'identifier') for a script type. For example, 'js' might mean JavaScript.</param>
        /// <returns>A boolean value indicating whether or not this provider can execute scripts of the provided key.</returns>
        bool CanExecute(string key);

        /// <summary>
        /// Executes the provided script and returns a result, or not.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        /// <param name="options">Optional parameters to use for script execution.</param>
        /// <returns></returns>
        object Execute(string script, ScriptExecutionOptions options);
    }
}
