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
using ezNReporting.Errors;

namespace ezNReporting.Core
{
    /// <summary>
    /// Specifies a unique name for a type with the purpose of locating a type based on this type key.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TypeKeyAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets the unique type key.
        /// </summary>
        public string TypeKey { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeKeyAttribute"/> class.
        /// </summary>
        /// <param name="typeKey">The unique type key.</param>
        public TypeKeyAttribute(string typeKey)
        {
            TypeKey = typeKey;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches for the <see cref="TypeKeyAttribute"/> applied to the specified type and returns it.
        /// </summary>
        /// <param name="instance">The instance to return the type key off of.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/> was null.</exception>
        /// <exception cref="TypeKeyNotFoundException">The <see cref="TypeKeyAttribute"/> was not applied to the specified type.</exception>
        public static string GetTypeKeyOf(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            TypeKeyAttribute[] attribs = (TypeKeyAttribute[])instance.GetType().GetCustomAttributes(typeof(TypeKeyAttribute), false);
            if (attribs.Length > 0)
            {
                return attribs[0].TypeKey;
            }

            throw new TypeKeyNotFoundException(instance.GetType());
        }

        #endregion
    }
}
