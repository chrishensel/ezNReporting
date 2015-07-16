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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ezNReporting.Core
{
    /// <summary>
    /// Represents a collection holds instances of <see cref="IExtension"/>.
    /// </summary>
    public class ExtensionCollection : CollectionBase
    {
        #region Fields

        private object _host;

        #endregion

        #region Constructors

        private ExtensionCollection()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionCollection"/> class.
        /// </summary>
        /// <param name="host"></param>
        public ExtensionCollection(object host)
            : this()
        {
            _host = host;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers a new extension with the host.
        /// </summary>
        /// <param name="extension"></param>
        public void Add(IExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            extension.OnRegister(_host);

            this.List.Add(extension);
        }

        /// <summary>
        /// Registers a new extension with the host, based on its type.
        /// </summary>
        /// <param name="extensionType">The type of the extension to add. The type must implement the <see cref="IExtension"/> interface.</param>
        public void Add(Type extensionType)
        {
            if (extensionType == null)
            {
                throw new ArgumentNullException("extensionType");
            }

            if (!extensionType.GetInterfaces().Contains(typeof(IExtension)))
            {
                throw new InvalidCastException("Type doesn't implement IExtension interface!");
            }

            IExtension ext = (IExtension)Activator.CreateInstance(extensionType);

            this.Add(ext);
        }

        /// <summary>
        /// Returns all extensions based on a specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> Get<T>()
        {
            return this.List.OfType<T>();
        }

        #endregion
    }
}
