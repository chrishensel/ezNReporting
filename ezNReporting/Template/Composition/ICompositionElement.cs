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

using System.Collections.Generic;
using ezNReporting.Core;

namespace ezNReporting.Template.Composition
{
    /// <summary>
    /// Defines members for a type used to construct a single report section.
    /// </summary>
    public interface ICompositionElement : IPropertyContainer
    {
        /// <summary>
        /// Gets/sets the element that is the parent of this element.
        /// </summary>
        ICompositionElement Parent { get; set; }
        /// <summary>
        /// Determines whether or not this element supports children.
        /// </summary>
        bool ChildrenSupported { get; }
        /// <summary>
        /// Gets a list of children this element has.
        /// See documentation for further information.
        /// </summary>
        /// <remarks>This list is read-only. You must call <see cref="AddChild(ICompositionElement)"/> to add a new children.</remarks>
        /// <exception cref="System.NotSupportedException">The element doesn't support children.</exception>
        IReadOnlyList<ICompositionElement> Children { get; }
        /// <summary>
        /// Gets the classification of this element.
        /// This information may be used by exporters for styling etc.
        /// </summary>
        ElementClassifications Classification { get; }

        /// <summary>
        /// Adds the specified element to its list of children.
        /// </summary>
        /// <param name="element">The element to add.</param>
        /// <exception cref="System.NotSupportedException">The element doesn't support children.</exception>
        /// <exception cref="System.InvalidOperationException"><paramref name="element"/> already has a parent.</exception>
        void AddChild(ICompositionElement element);
        /// <summary>
        /// Returns the value of the property with the specified name, or a default value, if the property didn't exist.
        /// See documentation for further information.
        /// </summary>
        /// <remarks>The property search is done starting from the instance on which the method was called,
        /// then continues searching the parents until the value has been located. This means that values on the root level
        /// are overridden by values on children.</remarks>
        /// <typeparam name="T">The desired type of the value.</typeparam>
        /// <param name="name">The name of the property to search.</param>
        /// <param name="defaultValue">A default value, if the property didn't exist.</param>
        /// <returns>The actual value from the property, or the value from <paramref name="defaultValue"/> if not found.</returns>
        T GetProperty<T>(string name, T defaultValue);
    }
}
