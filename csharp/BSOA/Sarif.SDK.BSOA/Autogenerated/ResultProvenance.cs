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
    ///  GENERATED: BSOA Entity for 'ResultProvenance'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ResultProvenance : PropertyBagHolder, ISarifNode, IRow
    {
        private ResultProvenanceTable _table;
        private int _index;

        public ResultProvenance() : this(SarifLogDatabase.Current.ResultProvenance)
        { }

        public ResultProvenance(SarifLog root) : this(root.Database.ResultProvenance)
        { }

        internal ResultProvenance(ResultProvenanceTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ResultProvenance(ResultProvenanceTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ResultProvenance(
            DateTime firstDetectionTimeUtc,
            DateTime lastDetectionTimeUtc,
            string firstDetectionRunGuid,
            string lastDetectionRunGuid,
            int invocationIndex,
            IList<PhysicalLocation> conversionSources,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.ResultProvenance)
        {
            FirstDetectionTimeUtc = firstDetectionTimeUtc;
            LastDetectionTimeUtc = lastDetectionTimeUtc;
            FirstDetectionRunGuid = firstDetectionRunGuid;
            LastDetectionRunGuid = lastDetectionRunGuid;
            InvocationIndex = invocationIndex;
            ConversionSources = conversionSources;
            Properties = properties;
        }

        public ResultProvenance(ResultProvenance other) 
            : this(SarifLogDatabase.Current.ResultProvenance)
        {
            FirstDetectionTimeUtc = other.FirstDetectionTimeUtc;
            LastDetectionTimeUtc = other.LastDetectionTimeUtc;
            FirstDetectionRunGuid = other.FirstDetectionRunGuid;
            LastDetectionRunGuid = other.LastDetectionRunGuid;
            InvocationIndex = other.InvocationIndex;
            ConversionSources = other.ConversionSources;
            Properties = other.Properties;
        }

        [DataMember(Name = "firstDetectionTimeUtc", IsRequired = false, EmitDefaultValue = false)]
        public DateTime FirstDetectionTimeUtc
        {
            get => _table.FirstDetectionTimeUtc[_index];
            set => _table.FirstDetectionTimeUtc[_index] = value;
        }

        [DataMember(Name = "lastDetectionTimeUtc", IsRequired = false, EmitDefaultValue = false)]
        public DateTime LastDetectionTimeUtc
        {
            get => _table.LastDetectionTimeUtc[_index];
            set => _table.LastDetectionTimeUtc[_index] = value;
        }

        [DataMember(Name = "firstDetectionRunGuid", IsRequired = false, EmitDefaultValue = false)]
        public string FirstDetectionRunGuid
        {
            get => _table.FirstDetectionRunGuid[_index];
            set => _table.FirstDetectionRunGuid[_index] = value;
        }

        [DataMember(Name = "lastDetectionRunGuid", IsRequired = false, EmitDefaultValue = false)]
        public string LastDetectionRunGuid
        {
            get => _table.LastDetectionRunGuid[_index];
            set => _table.LastDetectionRunGuid[_index] = value;
        }

        [DataMember(Name = "invocationIndex", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        public int InvocationIndex
        {
            get => _table.InvocationIndex[_index];
            set => _table.InvocationIndex[_index] = value;
        }

        [DataMember(Name = "conversionSources", IsRequired = false, EmitDefaultValue = false)]
        public IList<PhysicalLocation> ConversionSources
        {
            get => _table.Database.PhysicalLocation.List(_table.ConversionSources[_index]);
            set => _table.Database.PhysicalLocation.List(_table.ConversionSources[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ResultProvenance>
        public bool Equals(ResultProvenance other)
        {
            if (other == null) { return false; }

            if (this.FirstDetectionTimeUtc != other.FirstDetectionTimeUtc) { return false; }
            if (this.LastDetectionTimeUtc != other.LastDetectionTimeUtc) { return false; }
            if (this.FirstDetectionRunGuid != other.FirstDetectionRunGuid) { return false; }
            if (this.LastDetectionRunGuid != other.LastDetectionRunGuid) { return false; }
            if (this.InvocationIndex != other.InvocationIndex) { return false; }
            if (this.ConversionSources != other.ConversionSources) { return false; }
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
                if (FirstDetectionTimeUtc != default(DateTime))
                {
                    result = (result * 31) + FirstDetectionTimeUtc.GetHashCode();
                }

                if (LastDetectionTimeUtc != default(DateTime))
                {
                    result = (result * 31) + LastDetectionTimeUtc.GetHashCode();
                }

                if (FirstDetectionRunGuid != default(string))
                {
                    result = (result * 31) + FirstDetectionRunGuid.GetHashCode();
                }

                if (LastDetectionRunGuid != default(string))
                {
                    result = (result * 31) + LastDetectionRunGuid.GetHashCode();
                }

                if (InvocationIndex != default(int))
                {
                    result = (result * 31) + InvocationIndex.GetHashCode();
                }

                if (ConversionSources != default(IList<PhysicalLocation>))
                {
                    result = (result * 31) + ConversionSources.GetHashCode();
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
            return Equals(obj as ResultProvenance);
        }

        public static bool operator ==(ResultProvenance left, ResultProvenance right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ResultProvenance left, ResultProvenance right)
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
            _table = (ResultProvenanceTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ResultProvenance;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ResultProvenance DeepClone()
        {
            return (ResultProvenance)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ResultProvenance(this);
        }
        #endregion

        public static IEqualityComparer<ResultProvenance> ValueComparer => EqualityComparer<ResultProvenance>.Default;
        public bool ValueEquals(ResultProvenance other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}