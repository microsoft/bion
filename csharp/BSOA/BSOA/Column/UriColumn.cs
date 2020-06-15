// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace BSOA.Column
{
    /// <summary>
    ///  UriColumn implements IColumn for Uri on top of a StringColumn
    /// </summary>
    public class UriColumn : WrappingColumn<Uri, string>
    {
        public UriColumn() : base(new StringColumn())
        { }

        public override Uri this[int index] 
        {
            get => Convert(Inner[index]);
            set => Inner[index] = Convert(value);
        }

        private static Uri Convert(string value)
        {
            return (value == null ? null : new Uri(value, UriKind.RelativeOrAbsolute));
        }

        private static string Convert(Uri value)
        {
            return value?.OriginalString;
        }
    }
}
