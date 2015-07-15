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
using ezNReporting.Core;

namespace ezNReporting
{
    /// <summary>
    /// Provides general extensions that are used throughout the reporting namespace.
    /// </summary>
    public static class GeneralExtensions
    {
        #region Methods

        /// <summary>
        /// Sets or appends a value to the given property.
        /// If the property does already exist, the value will be appended (see NameValueCollection).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Add<T>(this T container, string key, object value) where T : IPropertyContainer
        {
            container.Properties.Add(key, Convert.ToString(value ?? ""));

            return container;
        }

        /// <summary>
        /// Sets the property with the given key to a specific value.
        /// A previously set property value will be overridden.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Set<T>(this T container, string key, object value) where T : IPropertyContainer
        {
            container.Properties.Set(key, Convert.ToString(value ?? ""));

            return container;
        }

        #endregion
    }
}
