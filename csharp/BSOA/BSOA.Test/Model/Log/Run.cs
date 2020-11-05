// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Collections;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Test.Model.Log
{
    /// <summary>
    ///  BSOA GENERATED Root Entity for 'Run'
    /// </summary>
    public partial class Run : IRow<Run>, IEquatable<Run>
    {
        private RunTable _table;
        private int _index;

        internal RunDatabase Database => _table.Database;
        public IDatabase DB => _table.Database;

        public Run() : this(new RunDatabase().Run)
        { }

        public Run(Run other) : this(new RunDatabase().Run)
        {
            CopyFrom(other);
        }

        internal Run(RunTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal Run(RunTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

        public IList<Result> Results
        {
            get { _table.EnsureCurrent(this); return TypedList<Result>.Get(_table.Database.Result, _table.Results, _index); }
            set { _table.EnsureCurrent(this); TypedList<Result>.Set(_table.Database.Result, _table.Results, _index, value); }
        }

        public IList<Rule> Rules
        {
            get { _table.EnsureCurrent(this); return TypedList<Rule>.Get(_table.Database.Rule, _table.Rules, _index); }
            set { _table.EnsureCurrent(this); TypedList<Rule>.Set(_table.Database.Rule, _table.Rules, _index, value); }
        }

        #region IEquatable<Run>
        public bool Equals(Run other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.Results, other.Results)) { return false; }
            if (!object.Equals(this.Rules, other.Rules)) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Results != default(IList<Result>))
                {
                    result = (result * 31) + Results.GetHashCode();
                }

                if (Rules != default(IList<Rule>))
                {
                    result = (result * 31) + Rules.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Run);
        }

        public static bool operator ==(Run left, Run right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Run left, Run right)
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
            _table = (RunTable)table;
            _index = index;
        }

        public void CopyFrom(Run other)
        {
            Results = other.Results?.Select((item) => Result.Copy(_table.Database, item)).ToList();
            Rules = other.Rules?.Select((item) => Rule.Copy(_table.Database, item)).ToList();
        }
        #endregion

        #region Easy Serialization
        public void WriteBsoa(System.IO.Stream stream)
        {
            using (BinaryTreeWriter writer = new BinaryTreeWriter(stream))
            {
                DB.Write(writer);
            }
        }

        public void WriteBsoa(string filePath)
        {
            WriteBsoa(System.IO.File.Create(filePath));
        }

        public static Run ReadBsoa(System.IO.Stream stream)
        {
            using (BinaryTreeReader reader = new BinaryTreeReader(stream))
            {
                Run result = new Run();
                result.DB.Read(reader);
                return result;
            }
        }

        public static Run ReadBsoa(string filePath)
        {
            return ReadBsoa(System.IO.File.OpenRead(filePath));
        }

        public static TreeDiagnostics Diagnostics(string filePath)
        {
            return Diagnostics(System.IO.File.OpenRead(filePath));
        }

        public static TreeDiagnostics Diagnostics(System.IO.Stream stream)
        {
            using (BinaryTreeReader btr = new BinaryTreeReader(stream))
            using (TreeDiagnosticsReader reader = new TreeDiagnosticsReader(btr))
            {
                Run result = new Run();
                result.DB.Read(reader);
                return reader.Tree;
            }
        }
        #endregion
    }
}
