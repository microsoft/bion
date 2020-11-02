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
    ///  BSOA GENERATED Entity for 'Rule'
    /// </summary>
    public partial class Rule : IRow<Rule>, IEquatable<Rule>
    {
        private RuleTable _table;
        private int _index;

        public Rule() : this(RunDatabase.Current.Rule)
        { }

        public Rule(Run root) : this(root.Database.Rule)
        { }

        public Rule(Run root, Rule other) : this(root.Database.Rule)
        {
            CopyFrom(other);
        }

        internal Rule(RunDatabase database, Rule other) : this(database.Rule)
        {
            CopyFrom(other);
        }

        internal Rule(RuleTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal Rule(RuleTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

        public string Id
        {
            get => _table.Id[_index, this];
            set => _table.Id[_index, this] = value;
        }

        public String Guid
        {
            get => _table.Guid[_index, this];
            set => _table.Guid[_index, this] = value;
        }

        public Uri HelpUri
        {
            get => _table.HelpUri[_index, this];
            set => _table.HelpUri[_index, this] = value;
        }

        public IList<Rule> RelatedRules
        {
            get => TypedList<Rule>.Get(_table.Database.Rule, _table.RelatedRules, _index, this);
            set => TypedList<Rule>.Set(_table.Database.Rule, _table.RelatedRules, _index, value, this);
        }

        #region IEquatable<Rule>
        public bool Equals(Rule other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.Id, other.Id)) { return false; }
            if (!object.Equals(this.Guid, other.Guid)) { return false; }
            if (!object.Equals(this.HelpUri, other.HelpUri)) { return false; }
            if (!object.Equals(this.RelatedRules, other.RelatedRules)) { return false; }

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

                if (Guid != default(String))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (HelpUri != default(Uri))
                {
                    result = (result * 31) + HelpUri.GetHashCode();
                }

                if (RelatedRules != default(IList<Rule>))
                {
                    result = (result * 31) + RelatedRules.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rule);
        }

        public static bool operator ==(Rule left, Rule right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Rule left, Rule right)
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
            _table = (RuleTable)table;
            _index = index;
        }

        public void CopyFrom(Rule other)
        {
            Id = other.Id;
            Guid = other.Guid;
            HelpUri = other.HelpUri;
            RelatedRules = other.RelatedRules?.Select((item) => Rule.Copy(_table.Database, item)).ToList();
        }

        internal static Rule Copy(RunDatabase database, Rule other)
        {
            return (other == null ? null : new Rule(database, other));
        }
        #endregion
    }
}
