// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  BSOA GENERATED Database for 'Community'
    /// </summary>
    internal partial class PersonDatabase : Database
    {
        internal PersonTable Person { get; }
        internal CommunityTable Community { get; }

        public PersonDatabase() : base("Community")
        {
            _lastCreated = new WeakReference<PersonDatabase>(this);

            Person = AddTable(nameof(Person), new PersonTable(this));
            Community = AddTable(nameof(Community), new CommunityTable(this));
        }

        [ThreadStatic]
        private static WeakReference<PersonDatabase> _lastCreated;

        internal static PersonDatabase Current
        {
            get
            {
                PersonDatabase db;
                if (_lastCreated == null || !_lastCreated.TryGetTarget(out db)) { db = new PersonDatabase(); }
                return db;
            }
        }
    }
}
