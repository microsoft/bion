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

        public Result() : this(SarifLogDatabase.Current.Result)
        { }

        public Result(SarifLog root) : this(root.Database.Result)
        { }

        internal Result(ResultTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Result(ResultTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Result(
            Microsoft.CodeAnalysis.Sarif.BaselineState baselineState,
            string ruleId,
            int ruleIndex,
            Message message,
            IList<Location> locations,
            string guid
        ) 
            : this(SarifLogDatabase.Current.Result)
        {
            BaselineState = baselineState;
            RuleId = ruleId;
            RuleIndex = ruleIndex;
            Message = message;
            Locations = locations;
            Guid = guid;
        }

        public Result(Result other) 
            : this(SarifLogDatabase.Current.Result)
        {
            BaselineState = other.BaselineState;
            RuleId = other.RuleId;
            RuleIndex = other.RuleIndex;
            Message = other.Message;
            Locations = other.Locations;
            Guid = other.Guid;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.EnumConverter))]
        public Microsoft.CodeAnalysis.Sarif.BaselineState BaselineState
        {
            get => (Microsoft.CodeAnalysis.Sarif.BaselineState)_table.BaselineState[_index];
            set => _table.BaselineState[_index] = (int)value;
        }

        [DataMember(Name = "ruleId", IsRequired = false, EmitDefaultValue = false)]
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
            set => _table.Database.Location.List(_table.Locations[_index]).SetTo(value);
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        #region IEquatable<Result>
        public bool Equals(Result other)
        {
            if (other == null) { return false; }

            if (this.BaselineState != other.BaselineState) { return false; }
            if (this.RuleId != other.RuleId) { return false; }
            if (this.RuleIndex != other.RuleIndex) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.Locations != other.Locations) { return false; }
            if (this.Guid != other.Guid) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (BaselineState != default(Microsoft.CodeAnalysis.Sarif.BaselineState))
                {
                    result = (result * 31) + BaselineState.GetHashCode();
                }

                if (RuleId != default(string))
                {
                    result = (result * 31) + RuleId.GetHashCode();
                }

                if (RuleIndex != default(int))
                {
                    result = (result * 31) + RuleIndex.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (Locations != default(IList<Location>))
                {
                    result = (result * 31) + Locations.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Result);
        }

        public static bool operator ==(Result left, Result right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Result left, Result right)
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

        public static IEqualityComparer<Result> ValueComparer => EqualityComparer<Result>.Default;
        public bool ValueEquals(Result other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
