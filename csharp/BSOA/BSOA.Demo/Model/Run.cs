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
    ///  GENERATED: BSOA Entity for 'Run'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Run : PropertyBagHolder, ISarifNode, IRow
    {
        private RunTable _table;
        private int _index;

        internal Run(RunTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Run(RunTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Run(SarifLogBsoa database) : this(database.Run)
        { }

        public Run() : this(SarifLogBsoa.Current)
        { }

        public Run(Run other) : this()
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }
            _table.CopyItem(_index, other._table, other._index);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Tool Tool
        {
            get => _table.Database.Tool.Get(_table.Tool[_index]);
            set => _table.Tool[_index] = _table.Database.Tool.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Artifact> Artifacts
        {
            get => _table.Database.Artifact.List(_table.Artifacts[_index]);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Result> Results
        {
            get => _table.Database.Result.List(_table.Results[_index]);
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (RunTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Run;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Run DeepClone()
        {
            return (Run)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Run(this);
        }
        #endregion

        //public static IEqualityComparer<Run> ValueComparer => RunEqualityComparer.Instance;
        //public bool ValueEquals(Run other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
