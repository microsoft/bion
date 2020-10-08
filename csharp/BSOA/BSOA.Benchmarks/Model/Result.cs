// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Benchmarks.Model
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'Result'
    /// </summary>
    public partial class Result : IRow<Result>, IEquatable<Result>
    {
        private readonly ResultTable _table;
        private readonly int _index;

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
            get => _table.RuleId[_index];
            set => _table.RuleId[_index] = value;
        }

        public bool IsActive
        {
            get => _table.IsActive[_index];
            set => _table.IsActive[_index] = value;
        }

        public string Message
        {
            get => _table.Message[_index];
            set => _table.Message[_index] = value;
        }

        public int StartLine
        {
            get => _table.StartLine[_index];
            set => _table.StartLine[_index] = value;
        }

        public DateTime WhenDetectedUtc
        {
            get => _table.WhenDetectedUtc[_index];
            set => _table.WhenDetectedUtc[_index] = value;
        }

        public BaselineState BaselineState
        {
            get => (BaselineState)_table.BaselineState[_index];
            set => _table.BaselineState[_index] = (int)value;
        }

        public IDictionary<String, String> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        public IList<String> Tags
        {
            get => _table.Tags[_index];
            set => _table.Tags[_index] = value;
        }

        #region IEquatable<Result>
        public bool Equals(Result other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.RuleId, other.RuleId)) { return false; }
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

                if (Tags != default(IList<String>))
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
        ITable IRow<Result>.Table => _table;
        int IRow<Result>.Index => _index;

        public void CopyFrom(Result other)
        {
            RuleId = other.RuleId;
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
