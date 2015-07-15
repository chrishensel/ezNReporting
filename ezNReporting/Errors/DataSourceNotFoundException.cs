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

namespace ezNReporting.Errors
{
    /// <summary>
    /// Represents an error that occurs when attempting to access a data source that doesn't exist.
    /// </summary>
    [Serializable()]
    public class DataSourceNotFoundException : DataSourceException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceNotFoundException"/> class.
        /// </summary>
        public DataSourceNotFoundException()
            : base(Properties.Resources.DataSourceNotFoundExceptionMessage)
        {

        }

        #endregion
    }
}
