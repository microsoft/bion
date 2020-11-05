// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    /// <summary>
    ///  BSOA GENERATED Database for 'FileSystem'
    /// </summary>
    internal partial class FileSystemDatabase : Database
    {
        internal FileTable File;
        internal FolderTable Folder;
        internal FileSystemTable FileSystem;

        public FileSystemDatabase() : base("FileSystem")
        {
            _lastCreated = new WeakReference<FileSystemDatabase>(this);
            GetOrBuildTables();
        }

        public override void GetOrBuildTables()
        {
            File = GetOrBuild(nameof(File), () => new FileTable(this));
            Folder = GetOrBuild(nameof(Folder), () => new FolderTable(this));
            FileSystem = GetOrBuild(nameof(FileSystem), () => new FileSystemTable(this));
        }

        [ThreadStatic]
        private static WeakReference<FileSystemDatabase> _lastCreated;

        internal static FileSystemDatabase Current
        {
            get
            {
                FileSystemDatabase db;
                if (_lastCreated == null || !_lastCreated.TryGetTarget(out db)) { db = new FileSystemDatabase(); }
                return db;
            }
        }
    }
}
