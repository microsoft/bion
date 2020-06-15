// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Suppression'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Suppression : PropertyBagHolder, ISarifNode, IRow
    {
        private SuppressionTable _table;
        private int _index;

        public Suppression() : this(SarifLogDatabase.Current.Suppression)
        { }

        public Suppression(SarifLog root) : this(root.Database.Suppression)
        { }

        internal Suppression(SuppressionTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Suppression(SuppressionTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Suppression(
            string guid,
            SuppressionKind kind,
            SuppressionStatus status,
            string justification,
            Location location,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.Suppression)
        {
            Guid = guid;
            Kind = kind;
            Status = status;
            Justification = justification;
            Location = location;
            Properties = properties;
        }

        public Suppression(Suppression other) 
            : this(SarifLogDatabase.Current.Suppression)
        {
            Guid = other.Guid;
            Kind = other.Kind;
            Status = other.Status;
            Justification = other.Justification;
            Location = other.Location;
            Properties = other.Properties;
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        [DataMember(Name = "kind", IsRequired = false, EmitDefaultValue = false)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.EnumConverter))]
        public SuppressionKind Kind
        {
            get => (SuppressionKind)_table.Kind[_index];
            set => _table.Kind[_index] = (int)value;
        }

        [DataMember(Name = "status", IsRequired = false, EmitDefaultValue = false)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.EnumConverter))]
        public SuppressionStatus Status
        {
            get => (SuppressionStatus)_table.Status[_index];
            set => _table.Status[_index] = (int)value;
        }

        [DataMember(Name = "justification", IsRequired = false, EmitDefaultValue = false)]
        public string Justification
        {
            get => _table.Justification[_index];
            set => _table.Justification[_index] = value;
        }

        [DataMember(Name = "location", IsRequired = false, EmitDefaultValue = false)]
        public Location Location
        {
            get => _table.Database.Location.Get(_table.Location[_index]);
            set => _table.Location[_index] = _table.Database.Location.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Suppression>
        public bool Equals(Suppression other)
        {
            if (other == null) { return false; }

            if (this.Guid != other.Guid) { return false; }
            if (this.Kind != other.Kind) { return false; }
            if (this.Status != other.Status) { return false; }
            if (this.Justification != other.Justification) { return false; }
            if (this.Location != other.Location) { return false; }
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
                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (Kind != default(SuppressionKind))
                {
                    result = (result * 31) + Kind.GetHashCode();
                }

                if (Status != default(SuppressionStatus))
                {
                    result = (result * 31) + Status.GetHashCode();
                }

                if (Justification != default(string))
                {
                    result = (result * 31) + Justification.GetHashCode();
                }

                if (Location != default(Location))
                {
                    result = (result * 31) + Location.GetHashCode();
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
            return Equals(obj as Suppression);
        }

        public static bool operator ==(Suppression left, Suppression right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Suppression left, Suppression right)
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
            _table = (SuppressionTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Suppression;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Suppression DeepClone()
        {
            return (Suppression)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Suppression(this);
        }
        #endregion

        public static IEqualityComparer<Suppression> ValueComparer => EqualityComparer<Suppression>.Default;
        public bool ValueEquals(Suppression other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
