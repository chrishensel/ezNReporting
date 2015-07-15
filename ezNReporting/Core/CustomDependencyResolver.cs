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

using System.Collections.Generic;
using System.Linq;

namespace ezNReporting.Core
{
    /// <summary>
    /// Provides a dependency resolver that can be used to customize the dependency resolving procedure.
    /// </summary>
    public class CustomDependencyResolver : IDependencyResolver
    {
        #region Fields

        private IDependencyResolver _innerResolver;
        private List<ResolveItem> _items;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDependencyResolver"/> class.
        /// </summary>
        public CustomDependencyResolver()
        {
            _items = new List<ResolveItem>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDependencyResolver"/> class.
        /// </summary>
        /// <param name="innerResolver">An instance of <see cref="IDependencyResolver"/> to use when this instance could not resolve a resource.</param>
        public CustomDependencyResolver(IDependencyResolver innerResolver)
            : this()
        {
            _innerResolver = innerResolver;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Specifies a new resource for the provided type and name.
        /// </summary>
        /// <typeparam name="T">The type of the resource.</typeparam>
        /// <param name="name">The name of the resource.</param>
        /// <param name="value">The value of the resource.</param>
        /// <returns>A reference to this instance.</returns>
        public CustomDependencyResolver Set<T>(string name, T value)
        {
            _items.Add(new ResolveItem()
            {
                Name = name,
                TypeAQN = typeof(T).AssemblyQualifiedName,
                Value = value
            });

            return this;
        }

        #endregion

        #region IDependencyResolver Members

        bool IDependencyResolver.TryResolve<T>(string name, out T resolvedInstance)
        {
            resolvedInstance = default(T);

            string typeAqn = typeof(T).AssemblyQualifiedName;
            ResolveItem ri = _items.FirstOrDefault(_ => _.TypeAQN == typeAqn && _.Name == name);
            if (ri != null)
            {
                resolvedInstance = (T)ri.Value;
                return true;
            }

            if (_innerResolver != null)
            {
                return _innerResolver.TryResolve<T>(name, out resolvedInstance);
            }

            return false;
        }

        #endregion

        #region Nested types

        class ResolveItem
        {
            internal string Name;
            internal string TypeAQN;
            internal object Value;
        }

        #endregion
    }
}
