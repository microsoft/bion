// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.IO;
using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Root Entity for 'SarifLog'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class SarifLog : PropertyBagHolder, ISarifNode, IRow
    {
        private SarifLogTable _table;
        private int _index;

        public SarifLog() : this(new SarifLogDatabase().SarifLog)
        { }
        
        internal SarifLog(SarifLogTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal SarifLog(SarifLogTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public SarifLog(
            Uri schemaUri,
            SarifVersion version,
            IList<Run> runs,
            IList<ExternalProperties> inlineExternalProperties,
            IDictionary<string, string> properties
        ) : this()
        {
            SchemaUri = schemaUri;
            Version = version;
            Runs = runs;
            InlineExternalProperties = inlineExternalProperties;
            Properties = properties;
        }

        public SarifLog(SarifLog other)
            : this()
        {
            SchemaUri = other.SchemaUri;
            Version = other.Version;
            Runs = other.Runs;
            InlineExternalProperties = other.InlineExternalProperties;
            Properties = other.Properties;
        }

        [DataMember(Name = "$schema", IsRequired = false, EmitDefaultValue = false)]
        [JsonConverter(typeof(UriConverter))]
        public Uri SchemaUri
        {
            get => _table.SchemaUri[_index];
            set => _table.SchemaUri[_index] = value;
        }

        [DataMember(Name = "version", IsRequired = false, EmitDefaultValue = false)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.SarifVersionConverter))]
        public SarifVersion Version
        {
            get => (SarifVersion)_table.Version[_index];
            set => _table.Version[_index] = (int)value;
        }

        [DataMember(Name = "runs", IsRequired = false, EmitDefaultValue = false)]
        public IList<Run> Runs
        {
            get => _table.Database.Run.List(_table.Runs[_index]);
            set => _table.Database.Run.List(_table.Runs[_index]).SetTo(value);
        }

        [DataMember(Name = "inlineExternalProperties", IsRequired = false, EmitDefaultValue = false)]
        public IList<ExternalProperties> InlineExternalProperties
        {
            get => _table.Database.ExternalProperties.List(_table.InlineExternalProperties[_index]);
            set => _table.Database.ExternalProperties.List(_table.InlineExternalProperties[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<SarifLog>
        public bool Equals(SarifLog other)
        {
            if (other == null) { return false; }

            if (this.SchemaUri != other.SchemaUri) { return false; }
            if (this.Version != other.Version) { return false; }
            if (this.Runs != other.Runs) { return false; }
            if (this.InlineExternalProperties != other.InlineExternalProperties) { return false; }
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
                if (SchemaUri != default(Uri))
                {
                    result = (result * 31) + SchemaUri.GetHashCode();
                }

                if (Version != default(SarifVersion))
                {
                    result = (result * 31) + Version.GetHashCode();
                }

                if (Runs != default(IList<Run>))
                {
                    result = (result * 31) + Runs.GetHashCode();
                }

                if (InlineExternalProperties != default(IList<ExternalProperties>))
                {
                    result = (result * 31) + InlineExternalProperties.GetHashCode();
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
            return Equals(obj as SarifLog);
        }

        public static bool operator ==(SarifLog left, SarifLog right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(SarifLog left, SarifLog right)
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
            _table = (SarifLogTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.SarifLog;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public SarifLog DeepClone()
        {
            return (SarifLog)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new SarifLog(this);
        }
        #endregion

        public static IEqualityComparer<SarifLog> ValueComparer => EqualityComparer<SarifLog>.Default;
        public bool ValueEquals(SarifLog other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();

        internal SarifLogDatabase Database => _table.Database;
        public ITreeSerializable DB => _table.Database;
    }
}
