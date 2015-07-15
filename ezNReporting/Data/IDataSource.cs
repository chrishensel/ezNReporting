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

using ezNReporting.Core;

namespace ezNReporting.Data
{
    /// <summary>
    /// Defines members of a data source, which serves as a data container to supply the actual report with data.
    /// </summary>
    public interface IDataSource : IPropertyContainer
    {
        /// <summary>
        /// Gets the name of the data source.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the underlying data provider that is used by this data source.
        /// </summary>
        IDataProvider Provider { get; }
    }
}
