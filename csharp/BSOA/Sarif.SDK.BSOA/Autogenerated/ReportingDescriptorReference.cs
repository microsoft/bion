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
    ///  GENERATED: BSOA Entity for 'ReportingDescriptorReference'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ReportingDescriptorReference : PropertyBagHolder, ISarifNode, IRow
    {
        private ReportingDescriptorReferenceTable _table;
        private int _index;

        public ReportingDescriptorReference() : this(SarifLogDatabase.Current.ReportingDescriptorReference)
        { }

        public ReportingDescriptorReference(SarifLog root) : this(root.Database.ReportingDescriptorReference)
        { }

        internal ReportingDescriptorReference(ReportingDescriptorReferenceTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ReportingDescriptorReference(ReportingDescriptorReferenceTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ReportingDescriptorReference(
            string id,
            int index,
            string guid,
            ToolComponentReference toolComponent,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.ReportingDescriptorReference)
        {
            Id = id;
            Index = index;
            Guid = guid;
            ToolComponent = toolComponent;
            Properties = properties;
        }

        public ReportingDescriptorReference(ReportingDescriptorReference other) 
            : this(SarifLogDatabase.Current.ReportingDescriptorReference)
        {
            Id = other.Id;
            Index = other.Index;
            Guid = other.Guid;
            ToolComponent = other.ToolComponent;
            Properties = other.Properties;
        }

        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        [DataMember(Name = "index", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        [DataMember(Name = "toolComponent", IsRequired = false, EmitDefaultValue = false)]
        public ToolComponentReference ToolComponent
        {
            get => _table.Database.ToolComponentReference.Get(_table.ToolComponent[_index]);
            set => _table.ToolComponent[_index] = _table.Database.ToolComponentReference.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ReportingDescriptorReference>
        public bool Equals(ReportingDescriptorReference other)
        {
            if (other == null) { return false; }

            if (this.Id != other.Id) { return false; }
            if (this.Index != other.Index) { return false; }
            if (this.Guid != other.Guid) { return false; }
            if (this.ToolComponent != other.ToolComponent) { return false; }
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
                if (Id != default(string))
                {
                    result = (result * 31) + Id.GetHashCode();
                }

                if (Index != default(int))
                {
                    result = (result * 31) + Index.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (ToolComponent != default(ToolComponentReference))
                {
                    result = (result * 31) + ToolComponent.GetHashCode();
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
            return Equals(obj as ReportingDescriptorReference);
        }

        public static bool operator ==(ReportingDescriptorReference left, ReportingDescriptorReference right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ReportingDescriptorReference left, ReportingDescriptorReference right)
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
            _table = (ReportingDescriptorReferenceTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ReportingDescriptorReference;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ReportingDescriptorReference DeepClone()
        {
            return (ReportingDescriptorReference)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ReportingDescriptorReference(this);
        }
        #endregion

        public static IEqualityComparer<ReportingDescriptorReference> ValueComparer => EqualityComparer<ReportingDescriptorReference>.Default;
        public bool ValueEquals(ReportingDescriptorReference other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
