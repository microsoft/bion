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
    ///  GENERATED: BSOA Entity for 'StackFrame'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class StackFrame : PropertyBagHolder, ISarifNode, IRow
    {
        private StackFrameTable _table;
        private int _index;

        public StackFrame() : this(SarifLogDatabase.Current.StackFrame)
        { }

        public StackFrame(SarifLog root) : this(root.Database.StackFrame)
        { }

        internal StackFrame(StackFrameTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal StackFrame(StackFrameTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public StackFrame(
            Location location,
            string module,
            int threadId,
            IList<string> parameters,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.StackFrame)
        {
            Location = location;
            Module = module;
            ThreadId = threadId;
            Parameters = parameters;
            Properties = properties;
        }

        public StackFrame(StackFrame other) 
            : this(SarifLogDatabase.Current.StackFrame)
        {
            Location = other.Location;
            Module = other.Module;
            ThreadId = other.ThreadId;
            Parameters = other.Parameters;
            Properties = other.Properties;
        }

        partial void Init();

        public Location Location
        {
            get => _table.Database.Location.Get(_table.Location[_index]);
            set => _table.Location[_index] = _table.Database.Location.LocalIndex(value);
        }

        public string Module
        {
            get => _table.Module[_index];
            set => _table.Module[_index] = value;
        }

        public int ThreadId
        {
            get => _table.ThreadId[_index];
            set => _table.ThreadId[_index] = value;
        }

        public IList<string> Parameters
        {
            get => _table.Parameters[_index];
            set => _table.Parameters[_index] = value;
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<StackFrame>
        public bool Equals(StackFrame other)
        {
            if (other == null) { return false; }

            if (this.Location != other.Location) { return false; }
            if (this.Module != other.Module) { return false; }
            if (this.ThreadId != other.ThreadId) { return false; }
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
                if (Location != default(Location))
                {
                    result = (result * 31) + Location.GetHashCode();
                }

                if (Module != default(string))
                {
                    result = (result * 31) + Module.GetHashCode();
                }

                if (ThreadId != default(int))
                {
                    result = (result * 31) + ThreadId.GetHashCode();
                }

                if (Parameters != default(IList<string>))
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
            return Equals(obj as StackFrame);
        }

        public static bool operator ==(StackFrame left, StackFrame right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(StackFrame left, StackFrame right)
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
            _table = (StackFrameTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.StackFrame;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public StackFrame DeepClone()
        {
            return (StackFrame)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new StackFrame(this);
        }
        #endregion

        public static IEqualityComparer<StackFrame> ValueComparer => EqualityComparer<StackFrame>.Default;
        public bool ValueEquals(StackFrame other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
