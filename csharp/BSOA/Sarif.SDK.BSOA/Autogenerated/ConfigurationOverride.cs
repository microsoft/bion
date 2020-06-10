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
    ///  GENERATED: BSOA Entity for 'ConfigurationOverride'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ConfigurationOverride : PropertyBagHolder, ISarifNode, IRow
    {
        private ConfigurationOverrideTable _table;
        private int _index;

        public ConfigurationOverride() : this(SarifLogDatabase.Current.ConfigurationOverride)
        { }

        public ConfigurationOverride(SarifLog root) : this(root.Database.ConfigurationOverride)
        { }

        internal ConfigurationOverride(ConfigurationOverrideTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ConfigurationOverride(ConfigurationOverrideTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ConfigurationOverride(
            ReportingConfiguration configuration,
            ReportingDescriptorReference descriptor,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.ConfigurationOverride)
        {
            Configuration = configuration;
            Descriptor = descriptor;
            Properties = properties;
        }

        public ConfigurationOverride(ConfigurationOverride other) 
            : this(SarifLogDatabase.Current.ConfigurationOverride)
        {
            Configuration = other.Configuration;
            Descriptor = other.Descriptor;
            Properties = other.Properties;
        }

        [DataMember(Name = "configuration", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ReportingConfiguration Configuration
        {
            get => _table.Database.ReportingConfiguration.Get(_table.Configuration[_index]);
            set => _table.Configuration[_index] = _table.Database.ReportingConfiguration.LocalIndex(value);
        }

        [DataMember(Name = "descriptor", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ReportingDescriptorReference Descriptor
        {
            get => _table.Database.ReportingDescriptorReference.Get(_table.Descriptor[_index]);
            set => _table.Descriptor[_index] = _table.Database.ReportingDescriptorReference.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ConfigurationOverride>
        public bool Equals(ConfigurationOverride other)
        {
            if (other == null) { return false; }

            if (this.Configuration != other.Configuration) { return false; }
            if (this.Descriptor != other.Descriptor) { return false; }
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
                if (Configuration != default(ReportingConfiguration))
                {
                    result = (result * 31) + Configuration.GetHashCode();
                }

                if (Descriptor != default(ReportingDescriptorReference))
                {
                    result = (result * 31) + Descriptor.GetHashCode();
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
            return Equals(obj as ConfigurationOverride);
        }

        public static bool operator ==(ConfigurationOverride left, ConfigurationOverride right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ConfigurationOverride left, ConfigurationOverride right)
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
            _table = (ConfigurationOverrideTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ConfigurationOverride;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ConfigurationOverride DeepClone()
        {
            return (ConfigurationOverride)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ConfigurationOverride(this);
        }
        #endregion

        public static IEqualityComparer<ConfigurationOverride> ValueComparer => EqualityComparer<ConfigurationOverride>.Default;
        public bool ValueEquals(ConfigurationOverride other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
