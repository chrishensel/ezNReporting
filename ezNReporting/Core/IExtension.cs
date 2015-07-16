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


namespace ezNReporting.Core
{
    /// <summary>
    /// Defines members for a generic extension.
    /// </summary>
    public interface IExtension
    {
        /// <summary>
        /// Called by the extension host when this extension is registered.
        /// </summary>
        /// <param name="host">The host at which this extension is registered.</param>
        void OnRegister(object host);
    }
}
