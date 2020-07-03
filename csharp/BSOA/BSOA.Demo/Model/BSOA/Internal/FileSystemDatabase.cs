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
        internal FileTable File { get; }
        internal FolderTable Folder { get; }
        internal FileSystemTable FileSystem { get; }

        public FileSystemDatabase()
        {
            _lastCreated = new WeakReference<FileSystemDatabase>(this);

            File = AddTable(nameof(File), new FileTable(this));
            Folder = AddTable(nameof(Folder), new FolderTable(this));
            FileSystem = AddTable(nameof(FileSystem), new FileSystemTable(this));
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
