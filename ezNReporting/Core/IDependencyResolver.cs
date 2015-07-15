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


namespace ezNReporting.Core
{
    /// <summary>
    /// Defines a member that can be used to attempt to resolve a required dependency.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Attempts to locate a required resource by its type and name.
        /// </summary>
        /// <typeparam name="T">The expected type of the resource.</typeparam>
        /// <param name="name">The name of the resource to resolve.</param>
        /// <param name="resolvedInstance">If the resource could be resolved, contains the resource that has been resolved.</param>
        /// <returns>A boolean value indicating whether or not the requested resource could be resolved.</returns>
        bool TryResolve<T>(string name, out T resolvedInstance);
    }
}
