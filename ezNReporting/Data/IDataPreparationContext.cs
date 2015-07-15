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


namespace ezNReporting.Data
{
    /// <summary>
    /// Defines members that are used during data preparation phase.
    /// </summary>
    public interface IDataPreparationContext
    {
        /// <summary>
        /// Returns the data container from the data source with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ezNReporting.Errors.DataSourceException">The specified data source couldn't be found.</exception>
        IDataContainer RequestDataContainer(string name);
    }
}
