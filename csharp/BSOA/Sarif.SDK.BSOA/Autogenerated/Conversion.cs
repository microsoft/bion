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
    ///  GENERATED: BSOA Entity for 'Conversion'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Conversion : PropertyBagHolder, ISarifNode, IRow
    {
        private ConversionTable _table;
        private int _index;

        public Conversion() : this(SarifLogDatabase.Current.Conversion)
        { }

        public Conversion(SarifLog root) : this(root.Database.Conversion)
        { }

        internal Conversion(ConversionTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal Conversion(ConversionTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Conversion(
            Tool tool,
            Invocation invocation,
            IList<ArtifactLocation> analysisToolLogFiles,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Conversion)
        {
            Tool = tool;
            Invocation = invocation;
            AnalysisToolLogFiles = analysisToolLogFiles;
            Properties = properties;
        }

        public Conversion(Conversion other) 
            : this(SarifLogDatabase.Current.Conversion)
        {
            Tool = other.Tool;
            Invocation = other.Invocation;
            AnalysisToolLogFiles = other.AnalysisToolLogFiles;
            Properties = other.Properties;
        }

        partial void Init();

        public Tool Tool
        {
            get => _table.Database.Tool.Get(_table.Tool[_index]);
            set => _table.Tool[_index] = _table.Database.Tool.LocalIndex(value);
        }

        public Invocation Invocation
        {
            get => _table.Database.Invocation.Get(_table.Invocation[_index]);
            set => _table.Invocation[_index] = _table.Database.Invocation.LocalIndex(value);
        }

        public IList<ArtifactLocation> AnalysisToolLogFiles
        {
            get => _table.Database.ArtifactLocation.List(_table.AnalysisToolLogFiles[_index]);
            set => _table.Database.ArtifactLocation.List(_table.AnalysisToolLogFiles[_index]).SetTo(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Conversion>
        public bool Equals(Conversion other)
        {
            if (other == null) { return false; }

            if (this.Tool != other.Tool) { return false; }
            if (this.Invocation != other.Invocation) { return false; }
            if (this.AnalysisToolLogFiles != other.AnalysisToolLogFiles) { return false; }
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
                if (Tool != default(Tool))
                {
                    result = (result * 31) + Tool.GetHashCode();
                }

                if (Invocation != default(Invocation))
                {
                    result = (result * 31) + Invocation.GetHashCode();
                }

                if (AnalysisToolLogFiles != default(IList<ArtifactLocation>))
                {
                    result = (result * 31) + AnalysisToolLogFiles.GetHashCode();
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
            return Equals(obj as Conversion);
        }

        public static bool operator ==(Conversion left, Conversion right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Conversion left, Conversion right)
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
            _table = (ConversionTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Conversion;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Conversion DeepClone()
        {
            return (Conversion)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Conversion(this);
        }
        #endregion

        public static IEqualityComparer<Conversion> ValueComparer => EqualityComparer<Conversion>.Default;
        public bool ValueEquals(Conversion other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
