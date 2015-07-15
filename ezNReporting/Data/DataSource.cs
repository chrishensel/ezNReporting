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

using System.Collections.Specialized;

namespace ezNReporting.Data
{
    /// <summary>
    /// Represents a simple data source.
    /// </summary>
    public class DataSource : IDataSource
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource"/> class.
        /// </summary>
        public DataSource()
        {
            Properties = new NameValueCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="provider"></param>
        public DataSource(string name, IDataProvider provider)
            : this()
        {
            this.Name = name;
            this.Provider = provider;
        }

        #endregion

        #region IDataSource Members

        /// <summary>
        /// Gets the name of the data source.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets the underlying data provider that is used by this data source.
        /// </summary>
        public IDataProvider Provider { get; set; }

        #endregion

        #region IPropertyContainer Members

        /// <summary>
        /// Gets a list of properties that are attached to this element.
        /// </summary>
        public NameValueCollection Properties { get; set; }

        #endregion
    }
}
