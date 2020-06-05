// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BSOA.IO;
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
    ///  GENERATED: BSOA Root Entity for 'SarifLog'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class SarifLog : PropertyBagHolder, ISarifNode, IRow
    {
        private SarifLogTable _table;
        private int _index;

        public SarifLog() : this(new SarifLogDatabase().SarifLog)
        { }
        
        internal SarifLog(SarifLogTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal SarifLog(SarifLogTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public SarifLog(
            IList<Run> runs
        ) : this()
        {
            Runs = runs;
        }

        public SarifLog(SarifLog other)
            : this()
        {
            Runs = other.Runs;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Run> Runs
        {
            get => _table.Database.Run.List(_table.Runs[_index]);
            set => _table.Database.Run.List(_table.Runs[_index]).SetTo(value);
        }

        #region IEquatable<SarifLog>
        public bool Equals(SarifLog other)
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
            return Equals(obj as SarifLog);
        }

        public static bool operator ==(SarifLog left, SarifLog right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(SarifLog left, SarifLog right)
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
            _table = (SarifLogTable)table;
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
        public SarifLog DeepClone()
        {
            return (SarifLog)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new SarifLog(this);
        }
        #endregion

        public static IEqualityComparer<SarifLog> ValueComparer => EqualityComparer<SarifLog>.Default;
        public bool ValueEquals(SarifLog other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();

        internal SarifLogDatabase Database => _table.Database;
        public ITreeSerializable DB => _table.Database;
    }
}
