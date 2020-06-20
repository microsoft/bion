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
    ///  GENERATED: BSOA Entity for 'WebRequest'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class WebRequest : PropertyBagHolder, ISarifNode, IRow
    {
        private WebRequestTable _table;
        private int _index;

        public WebRequest() : this(SarifLogDatabase.Current.WebRequest)
        { }

        public WebRequest(SarifLog root) : this(root.Database.WebRequest)
        { }

        internal WebRequest(WebRequestTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal WebRequest(WebRequestTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public WebRequest(
            int index,
            String protocol,
            String version,
            String target,
            String method,
            IDictionary<String, String> headers,
            IDictionary<String, String> parameters,
            ArtifactContent body,
            IDictionary<String, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.WebRequest)
        {
            Index = index;
            Protocol = protocol;
            Version = version;
            Target = target;
            Method = method;
            Headers = headers;
            Parameters = parameters;
            Body = body;
            Properties = properties;
        }

        public WebRequest(WebRequest other) 
            : this(SarifLogDatabase.Current.WebRequest)
        {
            Index = other.Index;
            Protocol = other.Protocol;
            Version = other.Version;
            Target = other.Target;
            Method = other.Method;
            Headers = other.Headers;
            Parameters = other.Parameters;
            Body = other.Body;
            Properties = other.Properties;
        }

        partial void Init();

        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        public String Protocol
        {
            get => _table.Protocol[_index];
            set => _table.Protocol[_index] = value;
        }

        public String Version
        {
            get => _table.Version[_index];
            set => _table.Version[_index] = value;
        }

        public String Target
        {
            get => _table.Target[_index];
            set => _table.Target[_index] = value;
        }

        public String Method
        {
            get => _table.Method[_index];
            set => _table.Method[_index] = value;
        }

        public IDictionary<String, String> Headers
        {
            get => _table.Headers[_index];
            set => _table.Headers[_index] = value;
        }

        public IDictionary<String, String> Parameters
        {
            get => _table.Parameters[_index];
            set => _table.Parameters[_index] = value;
        }

        public ArtifactContent Body
        {
            get => _table.Database.ArtifactContent.Get(_table.Body[_index]);
            set => _table.Body[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        internal override IDictionary<String, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<WebRequest>
        public bool Equals(WebRequest other)
        {
            if (other == null) { return false; }

            if (this.Index != other.Index) { return false; }
            if (this.Protocol != other.Protocol) { return false; }
            if (this.Version != other.Version) { return false; }
            if (this.Target != other.Target) { return false; }
            if (this.Method != other.Method) { return false; }
            if (this.Headers != other.Headers) { return false; }
            if (this.Parameters != other.Parameters) { return false; }
            if (this.Body != other.Body) { return false; }
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
                if (Index != default(int))
                {
                    result = (result * 31) + Index.GetHashCode();
                }

                if (Protocol != default(String))
                {
                    result = (result * 31) + Protocol.GetHashCode();
                }

                if (Version != default(String))
                {
                    result = (result * 31) + Version.GetHashCode();
                }

                if (Target != default(String))
                {
                    result = (result * 31) + Target.GetHashCode();
                }

                if (Method != default(String))
                {
                    result = (result * 31) + Method.GetHashCode();
                }

                if (Headers != default(IDictionary<String, String>))
                {
                    result = (result * 31) + Headers.GetHashCode();
                }

                if (Parameters != default(IDictionary<String, String>))
                {
                    result = (result * 31) + Parameters.GetHashCode();
                }

                if (Body != default(ArtifactContent))
                {
                    result = (result * 31) + Body.GetHashCode();
                }

                if (Properties != default(IDictionary<String, SerializedPropertyInfo>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WebRequest);
        }

        public static bool operator ==(WebRequest left, WebRequest right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(WebRequest left, WebRequest right)
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
        public SarifNodeKind SarifNodeKind => SarifNodeKind.WebRequest;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public WebRequest DeepClone()
        {
            return (WebRequest)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new WebRequest(this);
        }
        #endregion

        public static IEqualityComparer<WebRequest> ValueComparer => EqualityComparer<WebRequest>.Default;
        public bool ValueEquals(WebRequest other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
