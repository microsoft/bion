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
    ///  GENERATED: BSOA Entity for 'LogicalLocation'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class LogicalLocation : PropertyBagHolder, ISarifNode, IRow
    {
        private LogicalLocationTable _table;
        private int _index;

        public LogicalLocation() : this(SarifLogDatabase.Current.LogicalLocation)
        { }

        public LogicalLocation(SarifLog root) : this(root.Database.LogicalLocation)
        { }

        internal LogicalLocation(LogicalLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal LogicalLocation(LogicalLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public LogicalLocation(
            string name,
            int index,
            string fullyQualifiedName,
            string decoratedName,
            int parentIndex,
            string kind,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.LogicalLocation)
        {
            Name = name;
            Index = index;
            FullyQualifiedName = fullyQualifiedName;
            DecoratedName = decoratedName;
            ParentIndex = parentIndex;
            Kind = kind;
            Properties = properties;
        }

        public LogicalLocation(LogicalLocation other) 
            : this(SarifLogDatabase.Current.LogicalLocation)
        {
            Name = other.Name;
            Index = other.Index;
            FullyQualifiedName = other.FullyQualifiedName;
            DecoratedName = other.DecoratedName;
            ParentIndex = other.ParentIndex;
            Kind = other.Kind;
            Properties = other.Properties;
        }

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        [DataMember(Name = "index", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        [DataMember(Name = "fullyQualifiedName", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FullyQualifiedName
        {
            get => _table.FullyQualifiedName[_index];
            set => _table.FullyQualifiedName[_index] = value;
        }

        [DataMember(Name = "decoratedName", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DecoratedName
        {
            get => _table.DecoratedName[_index];
            set => _table.DecoratedName[_index] = value;
        }

        [DataMember(Name = "parentIndex", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        [DataMember(Name = "kind", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Kind
        {
            get => _table.Kind[_index];
            set => _table.Kind[_index] = value;
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<LogicalLocation>
        public bool Equals(LogicalLocation other)
        {
            if (other == null) { return false; }

            if (this.Name != other.Name) { return false; }
            if (this.Index != other.Index) { return false; }
            if (this.FullyQualifiedName != other.FullyQualifiedName) { return false; }
            if (this.DecoratedName != other.DecoratedName) { return false; }
            if (this.ParentIndex != other.ParentIndex) { return false; }
            if (this.Kind != other.Kind) { return false; }
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

                if (FullyQualifiedName != default(string))
                {
                    result = (result * 31) + FullyQualifiedName.GetHashCode();
                }

                if (DecoratedName != default(string))
                {
                    result = (result * 31) + DecoratedName.GetHashCode();
                }

                if (ParentIndex != default(int))
                {
                    result = (result * 31) + ParentIndex.GetHashCode();
                }

                if (Kind != default(string))
                {
                    result = (result * 31) + Kind.GetHashCode();
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
            return Equals(obj as LogicalLocation);
        }

        public static bool operator ==(LogicalLocation left, LogicalLocation right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(LogicalLocation left, LogicalLocation right)
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
            _table = (LogicalLocationTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.LogicalLocation;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public LogicalLocation DeepClone()
        {
            return (LogicalLocation)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new LogicalLocation(this);
        }
        #endregion

        public static IEqualityComparer<LogicalLocation> ValueComparer => EqualityComparer<LogicalLocation>.Default;
        public bool ValueEquals(LogicalLocation other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
