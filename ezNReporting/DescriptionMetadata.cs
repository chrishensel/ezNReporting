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


namespace ezNReporting
{
    /// <summary>
    /// Default implementation of the <see cref="IDescriptionMetadata"/> interface.
    /// </summary>
    public class DescriptionMetadata : IDescriptionMetadata
    {
        #region IDescriptionMetadata Members

        /// <summary>
        /// Gets/sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the author of the item.
        /// </summary>
        public string Author { get; set; }

        #endregion
    }
}
