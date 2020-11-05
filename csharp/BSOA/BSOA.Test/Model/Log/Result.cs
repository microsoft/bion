// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Test.Model.Log
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'Result'
    /// </summary>
    public partial class Result : IRow<Result>, IEquatable<Result>
    {
        private ResultTable _table;
        private int _index;

        public Result() : this(RunDatabase.Current.Result)
        { }

        public Result(Run root) : this(root.Database.Result)
        { }

        public Result(Run root, Result other) : this(root.Database.Result)
        {
            CopyFrom(other);
        }

        internal Result(RunDatabase database, Result other) : this(database.Result)
        {
            CopyFrom(other);
        }

        internal Result(ResultTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal Result(ResultTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

        public string RuleId
        {
            get => _table.RuleId[_index, this];
            set => _table.RuleId[_index, this] = value;
        }

        public Rule Rule
        {
            get => _table.Database.Rule.Get(_table.Rule[_index, this]);
            set => _table.Rule[_index, this] = _table.Database.Rule.LocalIndex(value);
        }

        public string Guid
        {
            get => _table.Guid[_index, this];
            set => _table.Guid[_index, this] = value;
        }

        public bool IsActive
        {
            get => _table.IsActive[_index, this];
            set => _table.IsActive[_index, this] = value;
        }

        public string Message
        {
            get => _table.Message[_index, this];
            set => _table.Message[_index, this] = value;
        }

        public int StartLine
        {
            get => _table.StartLine[_index, this];
            set => _table.StartLine[_index, this] = value;
        }

        public DateTime WhenDetectedUtc
        {
            get => _table.WhenDetectedUtc[_index, this];
            set => _table.WhenDetectedUtc[_index, this] = value;
        }

        public BaselineState BaselineState
        {
            get => (BaselineState)_table.BaselineState[_index, this];
            set => _table.BaselineState[_index, this] = (int)value;
        }

        public IDictionary<String, String> Properties
        {
            get => _table.Properties[_index, this];
            set => _table.Properties[_index, this] = value;
        }

        public IList<int> Tags
        {
            get => _table.Tags[_index, this];
            set => _table.Tags[_index, this] = value;
        }

        #region IEquatable<Result>
        public bool Equals(Result other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.RuleId, other.RuleId)) { return false; }
            if (!object.Equals(this.Rule, other.Rule)) { return false; }
            if (!object.Equals(this.Guid, other.Guid)) { return false; }
            if (!object.Equals(this.IsActive, other.IsActive)) { return false; }
            if (!object.Equals(this.Message, other.Message)) { return false; }
            if (!object.Equals(this.StartLine, other.StartLine)) { return false; }
            if (!object.Equals(this.WhenDetectedUtc, other.WhenDetectedUtc)) { return false; }
            if (!object.Equals(this.BaselineState, other.BaselineState)) { return false; }
            if (!object.Equals(this.Properties, other.Properties)) { return false; }
            if (!object.Equals(this.Tags, other.Tags)) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (RuleId != default(string))
                {
                    result = (result * 31) + RuleId.GetHashCode();
                }

                if (Rule != default(Rule))
                {
                    result = (result * 31) + Rule.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (IsActive != default(bool))
                {
                    result = (result * 31) + IsActive.GetHashCode();
                }

                if (Message != default(string))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (StartLine != default(int))
                {
                    result = (result * 31) + StartLine.GetHashCode();
                }

                if (WhenDetectedUtc != default(DateTime))
                {
                    result = (result * 31) + WhenDetectedUtc.GetHashCode();
                }

                if (BaselineState != default(BaselineState))
                {
                    result = (result * 31) + BaselineState.GetHashCode();
                }

                if (Properties != default(IDictionary<String, String>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }

                if (Tags != default(IList<int>))
                {
                    result = (result * 31) + Tags.GetHashCode();
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

        void IRow.Remap(ITable table, int index)
        {
            _table = (ResultTable)table;
            _index = index;
        }

        public void CopyFrom(Result other)
        {
            RuleId = other.RuleId;
            Rule = Rule.Copy(_table.Database, other.Rule);
            Guid = other.Guid;
            IsActive = other.IsActive;
            Message = other.Message;
            StartLine = other.StartLine;
            WhenDetectedUtc = other.WhenDetectedUtc;
            BaselineState = other.BaselineState;
            Properties = other.Properties;
            Tags = other.Tags;
        }

        internal static Result Copy(RunDatabase database, Result other)
        {
            return (other == null ? null : new Result(database, other));
        }
        #endregion
    }
}