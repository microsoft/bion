// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Interface exposed by objects that can hold properties of arbitrary types.
    /// </summary>
    public interface IPropertyBagHolder
    {
        IList<string> PropertyNames { get; }
        bool TryGetProperty(string propertyName, out string value);
        string GetProperty(string propertyName);
        bool TryGetProperty<T>(string propertyName, out T value);
        T GetProperty<T>(string propertyName);
        void SetProperty<T>(string propertyName, T value);
        void SetPropertiesFrom(IPropertyBagHolder other);
        void RemoveProperty(string propertyName);
    }
}