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
using ezNReporting.Data;
using ezNReporting.Template.Composition;

namespace ezNReporting.Export
{
    /// <summary>
    /// Provides utility functions that help with exporting reports.
    /// </summary>
    public static class ExportExtensions
    {
        #region Methods

        /// <summary>
        /// Recursively walks the provided element and all children until it finds the first instance of an element
        /// implementing the <see cref="IMultipleRowsProducer"/> interface, which it then returns.
        /// See documentation for further information.
        /// </summary>
        /// <remarks>This is helpful for exporters that export to a format where no free-form output is possible.
        /// Examples include the CSV exporter or an exporter that exports to a spreadsheet.</remarks>
        /// <param name="element">The element to search recursively.</param>
        /// <returns>An object representing the found element. -or- null, if no such element was found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> was null.</exception>
        public static IMultipleRowsProducer FindFirstMultipleRowsProducer(this ICompositionElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            IMultipleRowsProducer prod = null;

            prod = element as IMultipleRowsProducer;

            if (prod != null)
            {
                return prod;
            }

            if (element.ChildrenSupported)
            {
                foreach (ICompositionElement child in element.Children)
                {
                    prod = FindFirstMultipleRowsProducer(child);

                    if (prod != null)
                    {
                        return prod;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
