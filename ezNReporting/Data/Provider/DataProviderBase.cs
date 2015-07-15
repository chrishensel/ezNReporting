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
using System.Data;

namespace ezNReporting.Data.Provider
{
    /// <summary>
    /// Provides an abstract base class for building a data provider. 
    /// </summary>
    public abstract class DataProviderBase : IDataProvider
    {
        #region Properties

        /// <summary>
        /// Contains the data that has either been retrieved, or set by the user (depending on the implementation).
        /// </summary>
        protected DataSet CurrentData { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderBase"/> class.
        /// </summary>
        protected DataProviderBase()
        {
            this.Properties = new NameValueCollection();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this instance using the provided parameter data.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// When overridden in a derived class, peforms application-specific logic to retrieve the data for the report.
        /// </summary>
        protected abstract void RetrieveData();

        #endregion

        #region IDataContainer Members

        DataSet IDataContainer.Data
        {
            get { return CurrentData; }
        }

        #endregion

        #region IDataProvider Members

        void IDataProvider.Initialize()
        {
            Initialize();
        }

        void IDataProvider.RetrieveData()
        {
            RetrieveData();
        }

        #endregion

        #region IPropertyContainer Members

        /// <summary>
        /// Gets a list of properties that are attached to this element.
        /// </summary>
        public NameValueCollection Properties { get; private set; }

        #endregion
    }
}
