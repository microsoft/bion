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
    ///  GENERATED: BSOA Entity for 'ThreadFlow'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ThreadFlow : PropertyBagHolder, ISarifNode, IRow
    {
        private ThreadFlowTable _table;
        private int _index;

        public ThreadFlow() : this(SarifLogDatabase.Current.ThreadFlow)
        { }

        public ThreadFlow(SarifLog root) : this(root.Database.ThreadFlow)
        { }

        internal ThreadFlow(ThreadFlowTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ThreadFlow(ThreadFlowTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ThreadFlow(
            string id,
            Message message,
            IDictionary<string, MultiformatMessageString> initialState,
            IDictionary<string, MultiformatMessageString> immutableState,
            IList<ThreadFlowLocation> locations,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.ThreadFlow)
        {
            Id = id;
            Message = message;
            InitialState = initialState;
            ImmutableState = immutableState;
            Locations = locations;
            Properties = properties;
        }

        public ThreadFlow(ThreadFlow other) 
            : this(SarifLogDatabase.Current.ThreadFlow)
        {
            Id = other.Id;
            Message = other.Message;
            InitialState = other.InitialState;
            ImmutableState = other.ImmutableState;
            Locations = other.Locations;
            Properties = other.Properties;
        }

        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        [DataMember(Name = "message", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "initialState", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IDictionary<string, MultiformatMessageString> InitialState
        {
            get => _table.InitialState[_index];
            set => _table.InitialState[_index] = value;
        }

        [DataMember(Name = "immutableState", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IDictionary<string, MultiformatMessageString> ImmutableState
        {
            get => _table.ImmutableState[_index];
            set => _table.ImmutableState[_index] = value;
        }

        [DataMember(Name = "locations", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<ThreadFlowLocation> Locations
        {
            get => _table.Database.ThreadFlowLocation.List(_table.Locations[_index]);
            set => _table.Database.ThreadFlowLocation.List(_table.Locations[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ThreadFlow>
        public bool Equals(ThreadFlow other)
        {
            if (other == null) { return false; }

            if (this.Id != other.Id) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.InitialState != other.InitialState) { return false; }
            if (this.ImmutableState != other.ImmutableState) { return false; }
            if (this.Locations != other.Locations) { return false; }
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

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (InitialState != default(IDictionary<string, MultiformatMessageString>))
                {
                    result = (result * 31) + InitialState.GetHashCode();
                }

                if (ImmutableState != default(IDictionary<string, MultiformatMessageString>))
                {
                    result = (result * 31) + ImmutableState.GetHashCode();
                }

                if (Locations != default(IList<ThreadFlowLocation>))
                {
                    result = (result * 31) + Locations.GetHashCode();
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
            return Equals(obj as ThreadFlow);
        }

        public static bool operator ==(ThreadFlow left, ThreadFlow right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ThreadFlow left, ThreadFlow right)
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
            _table = (ThreadFlowTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ThreadFlow;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ThreadFlow DeepClone()
        {
            return (ThreadFlow)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ThreadFlow(this);
        }
        #endregion

        public static IEqualityComparer<ThreadFlow> ValueComparer => EqualityComparer<ThreadFlow>.Default;
        public bool ValueEquals(ThreadFlow other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
