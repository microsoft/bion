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
    ///  GENERATED: BSOA Entity for 'Result'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Result : PropertyBagHolder, ISarifNode, IRow
    {
        private ResultTable _table;
        private int _index;

        internal Result(ResultTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Result(ResultTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Result(SarifLogBsoa database) : this(database.Result)
        { }

        public Result() : this(SarifLogBsoa.Current)
        { }

        public Result(Result other) : this()
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }
            _table.CopyItem(_index, other._table, other._index);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.EnumConverter))]
        public Microsoft.CodeAnalysis.Sarif.BaselineState BaselineState
        {
            get => (Microsoft.CodeAnalysis.Sarif.BaselineState)_table.BaselineState[_index];
            set => _table.BaselineState[_index] = (int)value;
        }

        [DataMember(Name = "ruleId", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string RuleId
        {
            get => _table.RuleId[_index];
            set => _table.RuleId[_index] = value;
        }

        [DataMember(Name = "ruleIndex", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int RuleIndex
        {
            get => _table.RuleIndex[_index];
            set => _table.RuleIndex[_index] = value;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Location> Locations
        {
            get => _table.Database.Location.List(_table.Locations[_index]);
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ResultTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Result;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Result DeepClone()
        {
            return (Result)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Result(this);
        }
        #endregion

        //public static IEqualityComparer<Result> ValueComparer => ResultEqualityComparer.Instance;
        //public bool ValueEquals(Result other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
