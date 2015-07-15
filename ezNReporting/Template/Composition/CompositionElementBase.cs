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
using System.Collections.Generic;
using System.Collections.Specialized;
using ezNReporting.Data;

namespace ezNReporting.Template.Composition
{
    /// <summary>
    /// Represents the base class for any concrete composition element.
    /// </summary>
    public abstract class CompositionElementBase : ICompositionElement, IDataPreparation
    {
        #region Fields

        private List<ICompositionElement> _children;

        #endregion

        #region Properties

        /// <summary>
        /// Determines whether or not this element supports children.
        /// </summary>
        protected bool SupportsChildren { get; private set; }

        #endregion

        #region Constructors

        private CompositionElementBase()
        {
            _children = new List<ICompositionElement>();
            Properties = new NameValueCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositionElementBase"/> class.
        /// </summary>
        /// <param name="supportsChildren">Whether or not children are supported.</param>
        protected CompositionElementBase(bool supportsChildren)
            : this()
        {
            this.SupportsChildren = supportsChildren;
        }

        #endregion

        #region ICompositionElement Members

        /// <summary>
        /// Gets the classification of this element.
        /// </summary>
        public ElementClassifications Classification { get; protected set; }

        ICompositionElement ICompositionElement.Parent { get; set; }

        IReadOnlyList<ICompositionElement> ICompositionElement.Children
        {
            get
            {
                if (SupportsChildren)
                {
                    return _children;
                }

                throw new NotSupportedException("This container does not support children!");
            }
        }

        bool ICompositionElement.ChildrenSupported
        {
            get { return SupportsChildren; }
        }

        void ICompositionElement.AddChild(ICompositionElement element)
        {
            if (!SupportsChildren)
            {
                throw new NotSupportedException("This container does not support children!");
            }

            if (element.Parent != null)
            {
                throw new InvalidOperationException("element already has a parent!");
            }

            _children.Add(element);
            element.Parent = this;
        }

        T ICompositionElement.GetProperty<T>(string name, T defaultValue)
        {
            string value = null;

            ICompositionElement node = this;
            while (true)
            {
                if (node == null)
                {
                    break;
                }

                value = node.Properties[name];

                if (value != null)
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }

                node = ((ICompositionElement)node).Parent;
            }

            return defaultValue;
        }

        #endregion

        #region IPropertyContainer Members

        /// <summary>
        /// Gets a list of properties that are attached to this element.
        /// </summary>
        public NameValueCollection Properties { get; private set; }

        #endregion

        #region IProvideValue Members

        void IDataPreparation.Prepare(IDataPreparationContext context)
        {
            Prepare(context);
        }

        /// <summary>
        /// When overridden in a derived class, prepares data for export.
        /// </summary>
        /// <param name="context">The <see cref="IDataPreparationContext"/> to use.</param>
        protected virtual void Prepare(IDataPreparationContext context)
        {

        }

        #endregion
    }
}
