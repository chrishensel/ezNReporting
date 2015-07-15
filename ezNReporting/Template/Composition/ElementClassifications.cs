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

namespace ezNReporting.Template.Composition
{
    /// <summary>
    /// Specifies some generic classifications that may describe a composition element.
    /// </summary>
    [Flags()]
    public enum ElementClassifications
    {
        /// <summary>
        /// The element is not classifiable or has no classification.
        /// </summary>
        Other = 0,
        /// <summary>
        /// The element represents a container that holds other elements.
        /// </summary>
        Container = 1,
        /// <summary>
        /// The element represents text, such as a (static or dynamic) label.
        /// </summary>
        Text = 2,
        /// <summary>
        /// The element represents a table that contains multiple rows of data, but is no container.
        /// </summary>
        Table = 4,
        /// <summary>
        /// The element is a placeholder, which has no text and may have a visual appearance.
        /// </summary>
        Placeholder = 2048,
        /// <summary>
        /// The element is a separator, which has no text and may have a visual appearance.
        /// </summary>
        Separator = 4096,
    }
}
