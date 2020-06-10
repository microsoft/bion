// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'ToolComponentReference'
    /// </summary>
    [DataContract]
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
        }

        internal ToolComponentReference(ToolComponentReferenceTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ToolComponentReference(
            string name,
            int index,
            string guid,
            IDictionary<string, string> properties
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

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        [DataMember(Name = "index", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        internal override IDictionary<string, string> Properties
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
                if (Name != default(string))
                {
                    result = (result * 31) + Name.GetHashCode();
                }

                if (Index != default(int))
                {
                    result = (result * 31) + Index.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (Properties != default(IDictionary<string, string>))
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

        void IRow.Reset(ITable table, int index)
        {
            _table = (ToolComponentReferenceTable)table;
            _index = index;
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
