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
    ///  GENERATED: BSOA Entity for 'ThreadFlowLocation'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ThreadFlowLocation : PropertyBagHolder, ISarifNode, IRow
    {
        private ThreadFlowLocationTable _table;
        private int _index;

        public ThreadFlowLocation() : this(SarifLogDatabase.Current.ThreadFlowLocation)
        { }

        public ThreadFlowLocation(SarifLog root) : this(root.Database.ThreadFlowLocation)
        { }

        internal ThreadFlowLocation(ThreadFlowLocationTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal ThreadFlowLocation(ThreadFlowLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ThreadFlowLocation(
            int index,
            Location location,
            Stack stack,
            IList<string> kinds,
            IList<ReportingDescriptorReference> taxa,
            string module,
            IDictionary<string, MultiformatMessageString> state,
            int nestingLevel,
            int executionOrder,
            DateTime executionTimeUtc,
            ThreadFlowLocationImportance importance,
            WebRequest webRequest,
            WebResponse webResponse,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.ThreadFlowLocation)
        {
            Index = index;
            Location = location;
            Stack = stack;
            Kinds = kinds;
            Taxa = taxa;
            Module = module;
            State = state;
            NestingLevel = nestingLevel;
            ExecutionOrder = executionOrder;
            ExecutionTimeUtc = executionTimeUtc;
            Importance = importance;
            WebRequest = webRequest;
            WebResponse = webResponse;
            Properties = properties;
        }

        public ThreadFlowLocation(ThreadFlowLocation other) 
            : this(SarifLogDatabase.Current.ThreadFlowLocation)
        {
            Index = other.Index;
            Location = other.Location;
            Stack = other.Stack;
            Kinds = other.Kinds;
            Taxa = other.Taxa;
            Module = other.Module;
            State = other.State;
            NestingLevel = other.NestingLevel;
            ExecutionOrder = other.ExecutionOrder;
            ExecutionTimeUtc = other.ExecutionTimeUtc;
            Importance = other.Importance;
            WebRequest = other.WebRequest;
            WebResponse = other.WebResponse;
            Properties = other.Properties;
        }

        partial void Init();

        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        public Location Location
        {
            get => _table.Database.Location.Get(_table.Location[_index]);
            set => _table.Location[_index] = _table.Database.Location.LocalIndex(value);
        }

        public Stack Stack
        {
            get => _table.Database.Stack.Get(_table.Stack[_index]);
            set => _table.Stack[_index] = _table.Database.Stack.LocalIndex(value);
        }

        public IList<string> Kinds
        {
            get => _table.Kinds[_index];
            set => _table.Kinds[_index] = value;
        }

        public IList<ReportingDescriptorReference> Taxa
        {
            get => _table.Database.ReportingDescriptorReference.List(_table.Taxa[_index]);
            set => _table.Database.ReportingDescriptorReference.List(_table.Taxa[_index]).SetTo(value);
        }

        public string Module
        {
            get => _table.Module[_index];
            set => _table.Module[_index] = value;
        }

        public IDictionary<string, MultiformatMessageString> State
        {
            get => _table.State[_index];
            set => _table.State[_index] = value;
        }

        public int NestingLevel
        {
            get => _table.NestingLevel[_index];
            set => _table.NestingLevel[_index] = value;
        }

        public int ExecutionOrder
        {
            get => _table.ExecutionOrder[_index];
            set => _table.ExecutionOrder[_index] = value;
        }

        public DateTime ExecutionTimeUtc
        {
            get => _table.ExecutionTimeUtc[_index];
            set => _table.ExecutionTimeUtc[_index] = value;
        }

        public ThreadFlowLocationImportance Importance
        {
            get => (ThreadFlowLocationImportance)_table.Importance[_index];
            set => _table.Importance[_index] = (int)value;
        }

        public WebRequest WebRequest
        {
            get => _table.Database.WebRequest.Get(_table.WebRequest[_index]);
            set => _table.WebRequest[_index] = _table.Database.WebRequest.LocalIndex(value);
        }

        public WebResponse WebResponse
        {
            get => _table.Database.WebResponse.Get(_table.WebResponse[_index]);
            set => _table.WebResponse[_index] = _table.Database.WebResponse.LocalIndex(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ThreadFlowLocation>
        public bool Equals(ThreadFlowLocation other)
        {
            if (other == null) { return false; }

            if (this.Index != other.Index) { return false; }
            if (this.Location != other.Location) { return false; }
            if (this.Stack != other.Stack) { return false; }
            if (this.Kinds != other.Kinds) { return false; }
            if (this.Taxa != other.Taxa) { return false; }
            if (this.Module != other.Module) { return false; }
            if (this.State != other.State) { return false; }
            if (this.NestingLevel != other.NestingLevel) { return false; }
            if (this.ExecutionOrder != other.ExecutionOrder) { return false; }
            if (this.ExecutionTimeUtc != other.ExecutionTimeUtc) { return false; }
            if (this.Importance != other.Importance) { return false; }
            if (this.WebRequest != other.WebRequest) { return false; }
            if (this.WebResponse != other.WebResponse) { return false; }
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
                if (Index != default(int))
                {
                    result = (result * 31) + Index.GetHashCode();
                }

                if (Location != default(Location))
                {
                    result = (result * 31) + Location.GetHashCode();
                }

                if (Stack != default(Stack))
                {
                    result = (result * 31) + Stack.GetHashCode();
                }

                if (Kinds != default(IList<string>))
                {
                    result = (result * 31) + Kinds.GetHashCode();
                }

                if (Taxa != default(IList<ReportingDescriptorReference>))
                {
                    result = (result * 31) + Taxa.GetHashCode();
                }

                if (Module != default(string))
                {
                    result = (result * 31) + Module.GetHashCode();
                }

                if (State != default(IDictionary<string, MultiformatMessageString>))
                {
                    result = (result * 31) + State.GetHashCode();
                }

                if (NestingLevel != default(int))
                {
                    result = (result * 31) + NestingLevel.GetHashCode();
                }

                if (ExecutionOrder != default(int))
                {
                    result = (result * 31) + ExecutionOrder.GetHashCode();
                }

                if (ExecutionTimeUtc != default(DateTime))
                {
                    result = (result * 31) + ExecutionTimeUtc.GetHashCode();
                }

                if (Importance != default(ThreadFlowLocationImportance))
                {
                    result = (result * 31) + Importance.GetHashCode();
                }

                if (WebRequest != default(WebRequest))
                {
                    result = (result * 31) + WebRequest.GetHashCode();
                }

                if (WebResponse != default(WebResponse))
                {
                    result = (result * 31) + WebResponse.GetHashCode();
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
            return Equals(obj as ThreadFlowLocation);
        }

        public static bool operator ==(ThreadFlowLocation left, ThreadFlowLocation right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ThreadFlowLocation left, ThreadFlowLocation right)
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

        void IRow.Next()
        {
            _index++;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ThreadFlowLocation;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ThreadFlowLocation DeepClone()
        {
            return (ThreadFlowLocation)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ThreadFlowLocation(this);
        }
        #endregion

        public static IEqualityComparer<ThreadFlowLocation> ValueComparer => EqualityComparer<ThreadFlowLocation>.Default;
        public bool ValueEquals(ThreadFlowLocation other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
