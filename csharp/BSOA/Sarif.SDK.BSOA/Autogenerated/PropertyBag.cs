// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'PropertyBag'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class PropertyBag : ISarifNode, IRow
    {
        private PropertyBagTable _table;
        private int _index;

        public PropertyBag() : this(SarifLogDatabase.Current.PropertyBag)
        { }

        public PropertyBag(SarifLog root) : this(root.Database.PropertyBag)
        { }

        internal PropertyBag(PropertyBagTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal PropertyBag(PropertyBagTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public PropertyBag(
            IList<string> tags
        ) 
            : this(SarifLogDatabase.Current.PropertyBag)
        {
            Tags = tags;
        }

        public PropertyBag(PropertyBag other) 
            : this(SarifLogDatabase.Current.PropertyBag)
        {
            Tags = other.Tags;
        }

        partial void Init();

        public IList<string> Tags
        {
            get => _table.Tags[_index];
            set => _table.Tags[_index] = value;
        }

        #region IEquatable<PropertyBag>
        public bool Equals(PropertyBag other)
        {
            if (other == null) { return false; }

            if (this.Tags != other.Tags) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Tags != default(IList<string>))
                {
                    result = (result * 31) + Tags.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PropertyBag);
        }

        public static bool operator ==(PropertyBag left, PropertyBag right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(PropertyBag left, PropertyBag right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
        #endregion

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Next()
        {
            _index++;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.PropertyBag;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public PropertyBag DeepClone()
        {
            return (PropertyBag)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new PropertyBag(this);
        }
        #endregion

        public static IEqualityComparer<PropertyBag> ValueComparer => EqualityComparer<PropertyBag>.Default;
        public bool ValueEquals(PropertyBag other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
