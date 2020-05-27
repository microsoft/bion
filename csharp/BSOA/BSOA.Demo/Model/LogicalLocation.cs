// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace BSOA.Demo.Model
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

        internal LogicalLocation(LogicalLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public LogicalLocation(LogicalLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public LogicalLocation(SarifLogBsoa database) : this(database.LogicalLocation)
        { }

        public LogicalLocation() : this(SarifLogBsoa.Current)
        { }

        public LogicalLocation(LogicalLocation other) : this()
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }
            _table.CopyItem(_index, other._table, other._index);
        }

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
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

        [DataMember(Name = "fullyQualifiedName", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FullyQualifiedName
        {
            get => _table.FullyQualifiedName[_index];
            set => _table.FullyQualifiedName[_index] = value;
        }

        [DataMember(Name = "decoratedName", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string DecoratedName
        {
            get => _table.DecoratedName[_index];
            set => _table.DecoratedName[_index] = value;
        }

        [DataMember(Name = "parentIndex", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        [DataMember(Name = "kind", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Kind
        {
            get => _table.Kind[_index];
            set => _table.Kind[_index] = value;
        }

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

        //public static IEqualityComparer<LogicalLocation> ValueComparer => LogicalLocationEqualityComparer.Instance;
        //public bool ValueEquals(LogicalLocation other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
