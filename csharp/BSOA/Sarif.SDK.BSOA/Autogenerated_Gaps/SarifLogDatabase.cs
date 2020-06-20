﻿using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    internal partial class SarifLogDatabase
    {
        public override IColumn BuildColumn(string tableName, string columnName, Type type, object defaultValue = null)
        {
            if (type == typeof(IDictionary<String, MultiformatMessageString>))
            {
                return new DictionaryColumn<String, MultiformatMessageString>(
                    new DistinctColumn<string>(new StringColumn()),
                    new MultiformatMessageStringColumn(this));
            }
            else if(type == typeof(IDictionary<String, ArtifactLocation>))
            {
                return new DictionaryColumn<String, ArtifactLocation>(
                    new DistinctColumn<string>(new StringColumn()),
                    new ArtifactLocationColumn(this));
            }
            else if (type == typeof(IDictionary<String, SerializedPropertyInfo>))
            {
                return new DictionaryColumn<String, SerializedPropertyInfo>(
                    new DistinctColumn<string>(new StringColumn()),
                    new SerializedPropertyInfoColumn());
            }
            else
            {
                return ColumnFactory.Build(type, defaultValue, recurseTo: (t, d) => BuildColumn(tableName, columnName, t, d));
            }
        }
    }
}
