﻿// Copyright 2015 Sascha-Christian Hensel
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
using ezNReporting.Engine;

namespace ezNReporting.Data
{
    /// <summary>
    /// Defines members for a type that is able to perform a custom query and provide its results to the report.
    /// </summary>
    public interface IDataProvider : IDataContainer, IPropertyContainer
    {
        /// <summary>
        /// Initializes this instance using the provided parameter data.
        /// </summary>
        void Initialize();
        /// <summary>
        /// Queries the configured data source and stores its results.
        /// </summary>
        /// <param name="context">An instance of <see cref="IGenerationContext"/> that may be used during data retrieval.</param>
        void RetrieveData(IGenerationContext context);
    }
}
