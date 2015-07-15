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
using ezNReporting.Data;

namespace ezNReporting.Template.Composition.Elements
{
    /// <summary>
    /// Represents a label that outputs custom text.
    /// </summary>
    [TypeKey(ThisTypeKey)]
    public class StaticLabelElement : CompositionElementBase, IScalarValueProducer
    {
        #region Constants

        private const string ThisTypeKey = "static";

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the value (label text).
        /// </summary>
        public object Value
        {
            get { return Properties["value"]; }
            set { Properties["value"] = value.ToString(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticLabelElement"/> class.
        /// </summary>
        public StaticLabelElement()
            : base(false)
        {
            this.Classification = ElementClassifications.Text;
        }

        #endregion
    }
}
