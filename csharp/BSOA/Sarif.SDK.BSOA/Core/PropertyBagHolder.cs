// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Base class for objects that can hold properties of arbitrary types.
    /// </summary>
    public abstract class PropertyBagHolder : IPropertyBagHolder
    {
        protected PropertyBagHolder()
        {
            Tags = new TagsCollection(this);
        }

        [JsonIgnore]
        public IList<string> PropertyNames
        {
            get
            {
                return Properties != null ? Properties.Keys.ToList() : new List<string>();
            }
        }

        /// <summary>
        /// Key/value pairs that provide additional information about the run.
        /// </summary>
        internal abstract IDictionary<string, string> Properties { get; set; }

        public bool TryGetProperty(string propertyName, out string value)
        {
            if (Properties != null && Properties.Keys.Contains(propertyName))
            {
                value = GetProperty(propertyName);
                return true;
            }

            value = null;
            return false;
        }

        public string GetProperty(string propertyName)
        {
            return GetProperty<string>(propertyName);
        }

        public bool TryGetProperty<T>(string propertyName, out T value)
        {
            if (Properties?.ContainsKey(propertyName) == true)
            {
                value = GetProperty<T>(propertyName);
                return true;
            }

            value = default;
            return false;
        }

        public T GetProperty<T>(string propertyName)
        {
            string value = null;

            if (!Properties.TryGetValue(propertyName, out value))
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        SdkResources.PropertyDoesNotExist,
                        propertyName));
            }

            if (value == null)
            {
                if (typeof(T).IsValueType)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            SdkResources.PropertyOfValueTypeCannotBeNull,
                            propertyName,
                            typeof(T).FullName));
                }

                // This will return null for reference types. Could not set null here b/c T could be a value type as well.
                return default;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }

        public bool TryGetSerializedPropertyValue(string propertyName, out string serializedValue)
        {
            if (Properties?.ContainsKey(propertyName) == true)
            {
                serializedValue = GetSerializedPropertyValue(propertyName);
                return true;
            }

            serializedValue = null;
            return false;
        }

        public string GetSerializedPropertyValue(string propertyName)
        {
            if (Properties?.ContainsKey(propertyName) != true)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        SdkResources.PropertyDoesNotExist,
                        propertyName));
            }

            return Properties[propertyName];
        }

        private static readonly JsonSerializerSettings s_settingsWithComprehensiveV2ContractResolver = new JsonSerializerSettings
        {
            ContractResolver = new SarifContractResolver(),
            Formatting = Formatting.None
        };

        public void SetProperty<T>(string propertyName, T value)
        {
            bool isString = typeof(T) == typeof(string);

            if (value == null)
            {
                // This is consistent with what the PropertyBagConverter does when it encounters
                // a null-valued property. Whether we create a property bag dictionary entry
                // by deserializing a null from the log file or by calling SetProperty("aProp", null),
                // the internal representation is the same: a null value in the Properties
                // dictionary.
                Properties[propertyName] = null;
            }
            else
            {
                string serializedValue;

                if (isString)
                {
                    serializedValue = JsonConvert.ToString(value);
                }
                else
                {
                    // Use the appropriate serializer settings
                    JsonSerializerSettings settings = null;

                    if (propertyName.StartsWith("sarifv1/"))
                    {
                        throw new NotImplementedException();
                        //settings = SarifTransformerUtilities.JsonSettingsV1Compact;
                    }
                    else if (propertyName.StartsWith("sarifv2/"))
                    {
                        settings = s_settingsWithComprehensiveV2ContractResolver;
                    }

                    serializedValue = JsonConvert.SerializeObject(value, settings);
                }

                Properties[propertyName] = serializedValue;
            }
        }

        public void SetPropertiesFrom(IPropertyBagHolder other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            // We need the concrete class because the IPropertyBagHolder interface
            // doesn't expose the raw Properties array.
            PropertyBagHolder otherHolder = other as PropertyBagHolder;
            Debug.Assert(otherHolder != null);

            Properties.Clear();

            foreach (string propertyName in other.PropertyNames)
            {
                Properties[propertyName] = otherHolder.Properties[propertyName];
            }
        }

        public void RemoveProperty(string propertyName)
        {
            Properties?.Remove(propertyName);
        }

        [JsonIgnore]
        public TagsCollection Tags { get; }

        public virtual bool ShouldSerializeProperties()
        {
            return (this.Properties?.Count ?? 0) > 0;
        }
    }
}
