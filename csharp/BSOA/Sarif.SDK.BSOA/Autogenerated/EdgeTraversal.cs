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
    ///  GENERATED: BSOA Entity for 'EdgeTraversal'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class EdgeTraversal : PropertyBagHolder, ISarifNode, IRow
    {
        private EdgeTraversalTable _table;
        private int _index;

        public EdgeTraversal() : this(SarifLogDatabase.Current.EdgeTraversal)
        { }

        public EdgeTraversal(SarifLog root) : this(root.Database.EdgeTraversal)
        { }

        internal EdgeTraversal(EdgeTraversalTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal EdgeTraversal(EdgeTraversalTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public EdgeTraversal(
            string edgeId,
            Message message,
            IDictionary<string, MultiformatMessageString> finalState,
            int stepOverEdgeCount,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.EdgeTraversal)
        {
            EdgeId = edgeId;
            Message = message;
            FinalState = finalState;
            StepOverEdgeCount = stepOverEdgeCount;
            Properties = properties;
        }

        public EdgeTraversal(EdgeTraversal other) 
            : this(SarifLogDatabase.Current.EdgeTraversal)
        {
            EdgeId = other.EdgeId;
            Message = other.Message;
            FinalState = other.FinalState;
            StepOverEdgeCount = other.StepOverEdgeCount;
            Properties = other.Properties;
        }

        [DataMember(Name = "edgeId", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string EdgeId
        {
            get => _table.EdgeId[_index];
            set => _table.EdgeId[_index] = value;
        }

        [DataMember(Name = "message", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "finalState", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IDictionary<string, MultiformatMessageString> FinalState
        {
            get => _table.FinalState[_index];
            set => _table.FinalState[_index] = value;
        }

        [DataMember(Name = "stepOverEdgeCount", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int StepOverEdgeCount
        {
            get => _table.StepOverEdgeCount[_index];
            set => _table.StepOverEdgeCount[_index] = value;
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<EdgeTraversal>
        public bool Equals(EdgeTraversal other)
        {
            if (other == null) { return false; }

            if (this.EdgeId != other.EdgeId) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.FinalState != other.FinalState) { return false; }
            if (this.StepOverEdgeCount != other.StepOverEdgeCount) { return false; }
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
                if (EdgeId != default(string))
                {
                    result = (result * 31) + EdgeId.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (FinalState != default(IDictionary<string, MultiformatMessageString>))
                {
                    result = (result * 31) + FinalState.GetHashCode();
                }

                if (StepOverEdgeCount != default(int))
                {
                    result = (result * 31) + StepOverEdgeCount.GetHashCode();
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
            return Equals(obj as EdgeTraversal);
        }

        public static bool operator ==(EdgeTraversal left, EdgeTraversal right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(EdgeTraversal left, EdgeTraversal right)
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
            _table = (EdgeTraversalTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.EdgeTraversal;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public EdgeTraversal DeepClone()
        {
            return (EdgeTraversal)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new EdgeTraversal(this);
        }
        #endregion

        public static IEqualityComparer<EdgeTraversal> ValueComparer => EqualityComparer<EdgeTraversal>.Default;
        public bool ValueEquals(EdgeTraversal other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
