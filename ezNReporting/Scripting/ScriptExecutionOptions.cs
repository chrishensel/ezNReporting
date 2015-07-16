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
using System.Collections.Generic;
using ezNReporting.Data;

namespace ezNReporting.Scripting
{
    /// <summary>
    /// Provides values that may influence the execution of a user script.
    /// </summary>
    public class ScriptExecutionOptions
    {
        #region Properties

        /// <summary>
        /// Gets/sets the data provider associated with the script execution, if any.
        /// </summary>
        public IDataProvider AssociatedDataProvider { get; set; }
        /// <summary>
        /// Gets/sets an optional collection of parameters handed over to the script.
        /// </summary>
        public ICollection<object> Parameters { get; set; }
        /// <summary>
        /// Gets/sets the type that describes the desired return value.
        /// </summary>
        public Type DesiredReturnValueType { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptExecutionOptions"/> class.
        /// </summary>
        public ScriptExecutionOptions()
        {
            Parameters = new List<object>();
        }

        #endregion
    }
}
