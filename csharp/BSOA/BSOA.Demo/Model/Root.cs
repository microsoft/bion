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
    ///  GENERATED: BSOA Entity for 'Root'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    internal partial class Root : PropertyBagHolder, ISarifNode, IRow
    {
        private RootTable _table;
        private int _index;

        internal Root(RootTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Root(RootTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Root(SarifLogBsoa database) : this(database.Root)
        { }

        public Root() : this(SarifLogBsoa.Current)
        { }

        public Root(
            IList<Run> runs
        ) : this(SarifLogBsoa.Current)
        {
            Runs = runs;
        }

        public Root(Root other)
        {
            Runs = other.Runs;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Run> Runs
        {
            get => _table.Database.Run.List(_table.Runs[_index]);
            set => _table.Database.Run.List(_table.Runs[_index]).SetTo(value);
        }

        #region IEquatable<Root>
        public bool Equals(Root other)
        {
            if (other == null) { return false; }

            if (this.Runs != other.Runs) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Runs != default(IList<Run>))
                {
                    result = (result * 31) + Runs.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Root);
        }

        public static bool operator ==(Root left, Root right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Root left, Root right)
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
            _table = (RootTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.SarifLog;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Root DeepClone()
        {
            return (Root)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Root(this);
        }
        #endregion

        public static IEqualityComparer<Root> ValueComparer => EqualityComparer<Root>.Default;
        public bool ValueEquals(Root other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
