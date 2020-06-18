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
    ///  GENERATED: BSOA Entity for 'WebResponse'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class WebResponse : PropertyBagHolder, ISarifNode, IRow
    {
        private WebResponseTable _table;
        private int _index;

        public WebResponse() : this(SarifLogDatabase.Current.WebResponse)
        { }

        public WebResponse(SarifLog root) : this(root.Database.WebResponse)
        { }

        internal WebResponse(WebResponseTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal WebResponse(WebResponseTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public WebResponse(
            int index,
            string protocol,
            string version,
            int statusCode,
            string reasonPhrase,
            IDictionary<string, string> headers,
            ArtifactContent body,
            bool noResponseReceived,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.WebResponse)
        {
            Index = index;
            Protocol = protocol;
            Version = version;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Headers = headers;
            Body = body;
            NoResponseReceived = noResponseReceived;
            Properties = properties;
        }

        public WebResponse(WebResponse other) 
            : this(SarifLogDatabase.Current.WebResponse)
        {
            Index = other.Index;
            Protocol = other.Protocol;
            Version = other.Version;
            StatusCode = other.StatusCode;
            ReasonPhrase = other.ReasonPhrase;
            Headers = other.Headers;
            Body = other.Body;
            NoResponseReceived = other.NoResponseReceived;
            Properties = other.Properties;
        }

        partial void Init();

        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        public string Protocol
        {
            get => _table.Protocol[_index];
            set => _table.Protocol[_index] = value;
        }

        public string Version
        {
            get => _table.Version[_index];
            set => _table.Version[_index] = value;
        }

        public int StatusCode
        {
            get => _table.StatusCode[_index];
            set => _table.StatusCode[_index] = value;
        }

        public string ReasonPhrase
        {
            get => _table.ReasonPhrase[_index];
            set => _table.ReasonPhrase[_index] = value;
        }

        public IDictionary<string, string> Headers
        {
            get => _table.Headers[_index];
            set => _table.Headers[_index] = value;
        }

        public ArtifactContent Body
        {
            get => _table.Database.ArtifactContent.Get(_table.Body[_index]);
            set => _table.Body[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        public bool NoResponseReceived
        {
            get => _table.NoResponseReceived[_index];
            set => _table.NoResponseReceived[_index] = value;
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<WebResponse>
        public bool Equals(WebResponse other)
        {
            if (other == null) { return false; }

            if (this.Index != other.Index) { return false; }
            if (this.Protocol != other.Protocol) { return false; }
            if (this.Version != other.Version) { return false; }
            if (this.StatusCode != other.StatusCode) { return false; }
            if (this.ReasonPhrase != other.ReasonPhrase) { return false; }
            if (this.Headers != other.Headers) { return false; }
            if (this.Body != other.Body) { return false; }
            if (this.NoResponseReceived != other.NoResponseReceived) { return false; }
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

                if (Protocol != default(string))
                {
                    result = (result * 31) + Protocol.GetHashCode();
                }

                if (Version != default(string))
                {
                    result = (result * 31) + Version.GetHashCode();
                }

                if (StatusCode != default(int))
                {
                    result = (result * 31) + StatusCode.GetHashCode();
                }

                if (ReasonPhrase != default(string))
                {
                    result = (result * 31) + ReasonPhrase.GetHashCode();
                }

                if (Headers != default(IDictionary<string, string>))
                {
                    result = (result * 31) + Headers.GetHashCode();
                }

                if (Body != default(ArtifactContent))
                {
                    result = (result * 31) + Body.GetHashCode();
                }

                if (NoResponseReceived != default(bool))
                {
                    result = (result * 31) + NoResponseReceived.GetHashCode();
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
            return Equals(obj as WebResponse);
        }

        public static bool operator ==(WebResponse left, WebResponse right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(WebResponse left, WebResponse right)
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
            _table = (WebResponseTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.WebResponse;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public WebResponse DeepClone()
        {
            return (WebResponse)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new WebResponse(this);
        }
        #endregion

        public static IEqualityComparer<WebResponse> ValueComparer => EqualityComparer<WebResponse>.Default;
        public bool ValueEquals(WebResponse other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
