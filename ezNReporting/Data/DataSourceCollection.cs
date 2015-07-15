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
using ezNReporting.Errors;

namespace ezNReporting.Data
{
    /// <summary>
    /// Provides a specialized collection that holds <see cref="IDataSource"/> objects.
    /// </summary>
    public sealed class DataSourceCollection : IEnumerable<IDataSource>
    {
        #region Fields

        private readonly IDictionary<string, IDataSource> _list;

        #endregion

        #region Constructors

        internal DataSourceCollection()
            : base()
        {
            _list = new Dictionary<string, IDataSource>();
        }

        #endregion

        #region Methods

        private static string GetKey(string name)
        {
            if (name == null)
            {
                return null;
            }

            return name.ToLowerInvariant();
        }

        /// <summary>
        /// Adds a new data source to the collection, or overwrites an existing data source that has the same name.
        /// </summary>
        /// <param name="source">An instance of <see cref="IDataSource"/> to add or overwrite.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> was null.</exception>
        public void Set(IDataSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            _list[GetKey(source.Name)] = source;
        }

        /// <summary>
        /// Searches for the data source with the specified name.
        /// </summary>
        /// <param name="name">The name of the data source to search for (case-insensitive). May be null.</param>
        /// <returns>The instance of <see cref="IDataSource"/>.</returns>
        /// <exception cref="DataSourceNotFoundException">The requested data source wasn't found.</exception>
        public IDataSource GetByName(string name)
        {
            IDataSource ds = null;

            if (_list.TryGetValue(GetKey(name), out ds))
            {
                return ds;
            }

            throw new DataSourceNotFoundException();
        }

        #endregion

        #region IEnumerable<IDataSource> Members

        IEnumerator<IDataSource> IEnumerable<IDataSource>.GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }

        #endregion
    }
}
