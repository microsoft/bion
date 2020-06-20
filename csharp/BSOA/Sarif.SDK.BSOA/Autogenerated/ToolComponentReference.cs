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
    ///  GENERATED: BSOA Entity for 'ToolComponentReference'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ToolComponentReference : PropertyBagHolder, ISarifNode, IRow
    {
        private ToolComponentReferenceTable _table;
        private int _index;

        public ToolComponentReference() : this(SarifLogDatabase.Current.ToolComponentReference)
        { }

        public ToolComponentReference(SarifLog root) : this(root.Database.ToolComponentReference)
        { }

        internal ToolComponentReference(ToolComponentReferenceTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal ToolComponentReference(ToolComponentReferenceTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ToolComponentReference(
            String name,
            int index,
            String guid,
            IDictionary<String, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.ToolComponentReference)
        {
            Name = name;
            Index = index;
            Guid = guid;
            Properties = properties;
        }

        public ToolComponentReference(ToolComponentReference other) 
            : this(SarifLogDatabase.Current.ToolComponentReference)
        {
            Name = other.Name;
            Index = other.Index;
            Guid = other.Guid;
            Properties = other.Properties;
        }

        partial void Init();

        public String Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        public String Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        internal override IDictionary<String, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ToolComponentReference>
        public bool Equals(ToolComponentReference other)
        {
            if (other == null) { return false; }

            if (this.Name != other.Name) { return false; }
            if (this.Index != other.Index) { return false; }
            if (this.Guid != other.Guid) { return false; }
            if (this.Properties != other.Properties) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Name != default(String))
                {
                    result = (result * 31) + Name.GetHashCode();
                }

                if (Index != default(int))
                {
                    result = (result * 31) + Index.GetHashCode();
                }

                if (Guid != default(String))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (Properties != default(IDictionary<String, SerializedPropertyInfo>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ToolComponentReference);
        }

        public static bool operator ==(ToolComponentReference left, ToolComponentReference right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ToolComponentReference left, ToolComponentReference right)
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
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ToolComponentReference;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ToolComponentReference DeepClone()
        {
            return (ToolComponentReference)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ToolComponentReference(this);
        }
        #endregion

        public static IEqualityComparer<ToolComponentReference> ValueComparer => EqualityComparer<ToolComponentReference>.Default;
        public bool ValueEquals(ToolComponentReference other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
