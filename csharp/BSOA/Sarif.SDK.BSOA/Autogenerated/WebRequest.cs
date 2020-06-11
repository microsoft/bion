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
    ///  GENERATED: BSOA Entity for 'WebRequest'
    /// </summary>
    [DataContract]
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
        }

        internal WebRequest(WebRequestTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public WebRequest(
            int index,
            string protocol,
            string version,
            string target,
            string method,
            IDictionary<string, string> headers,
            IDictionary<string, string> parameters,
            ArtifactContent body,
            IDictionary<string, string> properties
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

        [DataMember(Name = "index", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        [DataMember(Name = "protocol", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Protocol
        {
            get => _table.Protocol[_index];
            set => _table.Protocol[_index] = value;
        }

        [DataMember(Name = "version", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Version
        {
            get => _table.Version[_index];
            set => _table.Version[_index] = value;
        }

        [DataMember(Name = "target", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Target
        {
            get => _table.Target[_index];
            set => _table.Target[_index] = value;
        }

        [DataMember(Name = "method", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Method
        {
            get => _table.Method[_index];
            set => _table.Method[_index] = value;
        }

        [DataMember(Name = "headers", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, string> Headers
        {
            get => _table.Headers[_index];
            set => _table.Headers[_index] = value;
        }

        [DataMember(Name = "parameters", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, string> Parameters
        {
            get => _table.Parameters[_index];
            set => _table.Parameters[_index] = value;
        }

        [DataMember(Name = "body", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ArtifactContent Body
        {
            get => _table.Database.ArtifactContent.Get(_table.Body[_index]);
            set => _table.Body[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal override IDictionary<string, string> Properties
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

                if (Protocol != default(string))
                {
                    result = (result * 31) + Protocol.GetHashCode();
                }

                if (Version != default(string))
                {
                    result = (result * 31) + Version.GetHashCode();
                }

                if (Target != default(string))
                {
                    result = (result * 31) + Target.GetHashCode();
                }

                if (Method != default(string))
                {
                    result = (result * 31) + Method.GetHashCode();
                }

                if (Headers != default(IDictionary<string, string>))
                {
                    result = (result * 31) + Headers.GetHashCode();
                }

                if (Parameters != default(IDictionary<string, string>))
                {
                    result = (result * 31) + Parameters.GetHashCode();
                }

                if (Body != default(ArtifactContent))
                {
                    result = (result * 31) + Body.GetHashCode();
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

        void IRow.Reset(ITable table, int index)
        {
            _table = (WebRequestTable)table;
            _index = index;
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
