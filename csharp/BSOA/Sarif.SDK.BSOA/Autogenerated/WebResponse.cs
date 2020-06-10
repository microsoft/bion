// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'WebResponse'
    /// </summary>
    [DataContract]
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
            IDictionary<string, string> properties
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

        [DataMember(Name = "index", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        [DataMember(Name = "protocol", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Protocol
        {
            get => _table.Protocol[_index];
            set => _table.Protocol[_index] = value;
        }

        [DataMember(Name = "version", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Version
        {
            get => _table.Version[_index];
            set => _table.Version[_index] = value;
        }

        [DataMember(Name = "statusCode", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int StatusCode
        {
            get => _table.StatusCode[_index];
            set => _table.StatusCode[_index] = value;
        }

        [DataMember(Name = "reasonPhrase", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ReasonPhrase
        {
            get => _table.ReasonPhrase[_index];
            set => _table.ReasonPhrase[_index] = value;
        }

        [DataMember(Name = "headers", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IDictionary<string, string> Headers
        {
            get => _table.Headers[_index];
            set => _table.Headers[_index] = value;
        }

        [DataMember(Name = "body", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ArtifactContent Body
        {
            get => _table.Database.ArtifactContent.Get(_table.Body[_index]);
            set => _table.Body[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        [DataMember(Name = "noResponseReceived", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool NoResponseReceived
        {
            get => _table.NoResponseReceived[_index];
            set => _table.NoResponseReceived[_index] = value;
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        internal override IDictionary<string, string> Properties
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

                if (Properties != default(IDictionary<string, string>))
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
