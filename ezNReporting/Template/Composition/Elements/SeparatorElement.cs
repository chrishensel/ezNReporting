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
using ezNReporting.Core;

namespace ezNReporting.Template.Composition.Elements
{
    /// <summary>
    /// Represents a visual separator.
    /// </summary>
    [TypeKey(ThisTypeKey)]
    public class SeparatorElement : CompositionElementBase
    {
        #region Constants

        private const string ThisTypeKey = "separator";

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the height of the separator.
        /// </summary>
        public int Height
        {
            get { return Convert.ToInt32(Properties["height"]); }
            set { Properties["height"] = value.ToString(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatorElement"/> class.
        /// </summary>
        public SeparatorElement()
            : base(false)
        {
            this.Classification = ElementClassifications.Separator;
        }

        #endregion
    }
}
