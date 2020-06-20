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
    ///  GENERATED: BSOA Entity for 'ExternalPropertyFileReferences'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ExternalPropertyFileReferences : PropertyBagHolder, ISarifNode, IRow
    {
        private ExternalPropertyFileReferencesTable _table;
        private int _index;

        public ExternalPropertyFileReferences() : this(SarifLogDatabase.Current.ExternalPropertyFileReferences)
        { }

        public ExternalPropertyFileReferences(SarifLog root) : this(root.Database.ExternalPropertyFileReferences)
        { }

        internal ExternalPropertyFileReferences(ExternalPropertyFileReferencesTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal ExternalPropertyFileReferences(ExternalPropertyFileReferencesTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ExternalPropertyFileReferences(
            ExternalPropertyFileReference conversion,
            IList<ExternalPropertyFileReference> graphs,
            ExternalPropertyFileReference externalizedProperties,
            IList<ExternalPropertyFileReference> artifacts,
            IList<ExternalPropertyFileReference> invocations,
            IList<ExternalPropertyFileReference> logicalLocations,
            IList<ExternalPropertyFileReference> threadFlowLocations,
            IList<ExternalPropertyFileReference> results,
            IList<ExternalPropertyFileReference> taxonomies,
            IList<ExternalPropertyFileReference> addresses,
            ExternalPropertyFileReference driver,
            IList<ExternalPropertyFileReference> extensions,
            IList<ExternalPropertyFileReference> policies,
            IList<ExternalPropertyFileReference> translations,
            IList<ExternalPropertyFileReference> webRequests,
            IList<ExternalPropertyFileReference> webResponses,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.ExternalPropertyFileReferences)
        {
            Conversion = conversion;
            Graphs = graphs;
            ExternalizedProperties = externalizedProperties;
            Artifacts = artifacts;
            Invocations = invocations;
            LogicalLocations = logicalLocations;
            ThreadFlowLocations = threadFlowLocations;
            Results = results;
            Taxonomies = taxonomies;
            Addresses = addresses;
            Driver = driver;
            Extensions = extensions;
            Policies = policies;
            Translations = translations;
            WebRequests = webRequests;
            WebResponses = webResponses;
            Properties = properties;
        }

        public ExternalPropertyFileReferences(ExternalPropertyFileReferences other) 
            : this(SarifLogDatabase.Current.ExternalPropertyFileReferences)
        {
            Conversion = other.Conversion;
            Graphs = other.Graphs;
            ExternalizedProperties = other.ExternalizedProperties;
            Artifacts = other.Artifacts;
            Invocations = other.Invocations;
            LogicalLocations = other.LogicalLocations;
            ThreadFlowLocations = other.ThreadFlowLocations;
            Results = other.Results;
            Taxonomies = other.Taxonomies;
            Addresses = other.Addresses;
            Driver = other.Driver;
            Extensions = other.Extensions;
            Policies = other.Policies;
            Translations = other.Translations;
            WebRequests = other.WebRequests;
            WebResponses = other.WebResponses;
            Properties = other.Properties;
        }

        partial void Init();

        public ExternalPropertyFileReference Conversion
        {
            get => _table.Database.ExternalPropertyFileReference.Get(_table.Conversion[_index]);
            set => _table.Conversion[_index] = _table.Database.ExternalPropertyFileReference.LocalIndex(value);
        }

        public IList<ExternalPropertyFileReference> Graphs
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Graphs[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Graphs[_index]).SetTo(value);
        }

        public ExternalPropertyFileReference ExternalizedProperties
        {
            get => _table.Database.ExternalPropertyFileReference.Get(_table.ExternalizedProperties[_index]);
            set => _table.ExternalizedProperties[_index] = _table.Database.ExternalPropertyFileReference.LocalIndex(value);
        }

        public IList<ExternalPropertyFileReference> Artifacts
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Artifacts[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Artifacts[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> Invocations
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Invocations[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Invocations[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> LogicalLocations
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.LogicalLocations[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.LogicalLocations[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> ThreadFlowLocations
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.ThreadFlowLocations[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.ThreadFlowLocations[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> Results
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Results[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Results[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> Taxonomies
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Taxonomies[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Taxonomies[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> Addresses
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Addresses[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Addresses[_index]).SetTo(value);
        }

        public ExternalPropertyFileReference Driver
        {
            get => _table.Database.ExternalPropertyFileReference.Get(_table.Driver[_index]);
            set => _table.Driver[_index] = _table.Database.ExternalPropertyFileReference.LocalIndex(value);
        }

        public IList<ExternalPropertyFileReference> Extensions
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Extensions[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Extensions[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> Policies
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Policies[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Policies[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> Translations
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.Translations[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.Translations[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> WebRequests
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.WebRequests[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.WebRequests[_index]).SetTo(value);
        }

        public IList<ExternalPropertyFileReference> WebResponses
        {
            get => _table.Database.ExternalPropertyFileReference.List(_table.WebResponses[_index]);
            set => _table.Database.ExternalPropertyFileReference.List(_table.WebResponses[_index]).SetTo(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ExternalPropertyFileReferences>
        public bool Equals(ExternalPropertyFileReferences other)
        {
            if (other == null) { return false; }

            if (this.Conversion != other.Conversion) { return false; }
            if (this.Graphs != other.Graphs) { return false; }
            if (this.ExternalizedProperties != other.ExternalizedProperties) { return false; }
            if (this.Artifacts != other.Artifacts) { return false; }
            if (this.Invocations != other.Invocations) { return false; }
            if (this.LogicalLocations != other.LogicalLocations) { return false; }
            if (this.ThreadFlowLocations != other.ThreadFlowLocations) { return false; }
            if (this.Results != other.Results) { return false; }
            if (this.Taxonomies != other.Taxonomies) { return false; }
            if (this.Addresses != other.Addresses) { return false; }
            if (this.Driver != other.Driver) { return false; }
            if (this.Extensions != other.Extensions) { return false; }
            if (this.Policies != other.Policies) { return false; }
            if (this.Translations != other.Translations) { return false; }
            if (this.WebRequests != other.WebRequests) { return false; }
            if (this.WebResponses != other.WebResponses) { return false; }
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
                if (Conversion != default(ExternalPropertyFileReference))
                {
                    result = (result * 31) + Conversion.GetHashCode();
                }

                if (Graphs != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Graphs.GetHashCode();
                }

                if (ExternalizedProperties != default(ExternalPropertyFileReference))
                {
                    result = (result * 31) + ExternalizedProperties.GetHashCode();
                }

                if (Artifacts != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Artifacts.GetHashCode();
                }

                if (Invocations != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Invocations.GetHashCode();
                }

                if (LogicalLocations != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + LogicalLocations.GetHashCode();
                }

                if (ThreadFlowLocations != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + ThreadFlowLocations.GetHashCode();
                }

                if (Results != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Results.GetHashCode();
                }

                if (Taxonomies != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Taxonomies.GetHashCode();
                }

                if (Addresses != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Addresses.GetHashCode();
                }

                if (Driver != default(ExternalPropertyFileReference))
                {
                    result = (result * 31) + Driver.GetHashCode();
                }

                if (Extensions != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Extensions.GetHashCode();
                }

                if (Policies != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Policies.GetHashCode();
                }

                if (Translations != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + Translations.GetHashCode();
                }

                if (WebRequests != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + WebRequests.GetHashCode();
                }

                if (WebResponses != default(IList<ExternalPropertyFileReference>))
                {
                    result = (result * 31) + WebResponses.GetHashCode();
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
            return Equals(obj as ExternalPropertyFileReferences);
        }

        public static bool operator ==(ExternalPropertyFileReferences left, ExternalPropertyFileReferences right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ExternalPropertyFileReferences left, ExternalPropertyFileReferences right)
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
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ExternalPropertyFileReferences;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ExternalPropertyFileReferences DeepClone()
        {
            return (ExternalPropertyFileReferences)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ExternalPropertyFileReferences(this);
        }
        #endregion

        public static IEqualityComparer<ExternalPropertyFileReferences> ValueComparer => EqualityComparer<ExternalPropertyFileReferences>.Default;
        public bool ValueEquals(ExternalPropertyFileReferences other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
