// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Model;

namespace BSOA.Test.Model.V1
{
    /// <summary>
    ///  BSOA GENERATED Database for 'Community'
    /// </summary>
    internal partial class PersonDatabase : Database
    {
        [ThreadStatic]
        private static WeakReference<PersonDatabase> _lastCreated;

        internal static PersonDatabase Current => (_lastCreated.TryGetTarget(out PersonDatabase value) ? value : new PersonDatabase());
        
        internal PersonTable Person { get; }
        internal CommunityTable Community { get; }

        public PersonDatabase()
        {
            _lastCreated = new WeakReference<PersonDatabase>(this);

            Person = AddTable(nameof(Person), new PersonTable(this));
            Community = AddTable(nameof(Community), new CommunityTable(this));
        }
    }
}
