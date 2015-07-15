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
using System.Globalization;

namespace ezNReporting.Errors
{
    /// <summary>
    /// Represents the exception that is thrown when a type is supposed to have the <see cref="Core.TypeKeyAttribute"/> applied, but didn't.
    /// </summary>
    [Serializable()]
    public class TypeKeyNotFoundException : ReportingExceptionBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeKeyNotFoundException"/> class.
        /// </summary>
        /// <param name="type">The type that did not have a <see cref="Core.TypeKeyAttribute"/> applied.</param>
        public TypeKeyNotFoundException(Type type)
            : base(string.Format(CultureInfo.InvariantCulture, Properties.Resources.TypeKeyNotFoundExceptionMessage, type.FullName))
        {

        }

        #endregion
    }
}
