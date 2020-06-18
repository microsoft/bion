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
    ///  GENERATED: BSOA Entity for 'ReportingConfiguration'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ReportingConfiguration : PropertyBagHolder, ISarifNode, IRow
    {
        private ReportingConfigurationTable _table;
        private int _index;

        public ReportingConfiguration() : this(SarifLogDatabase.Current.ReportingConfiguration)
        { }

        public ReportingConfiguration(SarifLog root) : this(root.Database.ReportingConfiguration)
        { }

        internal ReportingConfiguration(ReportingConfigurationTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal ReportingConfiguration(ReportingConfigurationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ReportingConfiguration(
            bool enabled,
            FailureLevel level,
            double rank,
            PropertyBag parameters,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.ReportingConfiguration)
        {
            Enabled = enabled;
            Level = level;
            Rank = rank;
            Parameters = parameters;
            Properties = properties;
        }

        public ReportingConfiguration(ReportingConfiguration other) 
            : this(SarifLogDatabase.Current.ReportingConfiguration)
        {
            Enabled = other.Enabled;
            Level = other.Level;
            Rank = other.Rank;
            Parameters = other.Parameters;
            Properties = other.Properties;
        }

        partial void Init();

        public bool Enabled
        {
            get => _table.Enabled[_index];
            set => _table.Enabled[_index] = value;
        }

        public FailureLevel Level
        {
            get => (FailureLevel)_table.Level[_index];
            set => _table.Level[_index] = (int)value;
        }

        public double Rank
        {
            get => _table.Rank[_index];
            set => _table.Rank[_index] = value;
        }

        public PropertyBag Parameters
        {
            get => _table.Database.PropertyBag.Get(_table.Parameters[_index]);
            set => _table.Parameters[_index] = _table.Database.PropertyBag.LocalIndex(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ReportingConfiguration>
        public bool Equals(ReportingConfiguration other)
        {
            if (other == null) { return false; }

            if (this.Enabled != other.Enabled) { return false; }
            if (this.Level != other.Level) { return false; }
            if (this.Rank != other.Rank) { return false; }
            if (this.Parameters != other.Parameters) { return false; }
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
                if (Enabled != default(bool))
                {
                    result = (result * 31) + Enabled.GetHashCode();
                }

                if (Level != default(FailureLevel))
                {
                    result = (result * 31) + Level.GetHashCode();
                }

                if (Rank != default(double))
                {
                    result = (result * 31) + Rank.GetHashCode();
                }

                if (Parameters != default(PropertyBag))
                {
                    result = (result * 31) + Parameters.GetHashCode();
                }

                if (Properties != default(IDictionary<string, SerializedPropertyInfo>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ReportingConfiguration);
        }

        public static bool operator ==(ReportingConfiguration left, ReportingConfiguration right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ReportingConfiguration left, ReportingConfiguration right)
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
            _table = (ReportingConfigurationTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ReportingConfiguration;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ReportingConfiguration DeepClone()
        {
            return (ReportingConfiguration)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ReportingConfiguration(this);
        }
        #endregion

        public static IEqualityComparer<ReportingConfiguration> ValueComparer => EqualityComparer<ReportingConfiguration>.Default;
        public bool ValueEquals(ReportingConfiguration other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
