// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using BSOA.Generator.Schema;

namespace BSOA.Generator
{
    /// <summary>
    ///  TemplateDefaults knows the values used in the Template code; these are used to string.Replace
    ///  the defaults from the template with the correct value for the desired column.
    /// </summary>
    /// <remarks>
    ///  Template Defaults were chosen carefully to avoid string.Replace over-replacing.
    ///  Avoid the type 'int', which will be replaced in 'internal'.
    ///  Avoid names which include the type name, causing incorrect replacements.
    /// </remarks>
    public static class TemplateDefaults
    {
        public static string Namespace = "BSOA.Generator.Templates";
        public static string DatabaseName = "CompanyDatabase";
        public static string RootTableName = "Company";
        public static string TableName = "Team";

        public static Dictionary<ColumnTypeCategory, Schema.Column> Columns = new Dictionary<ColumnTypeCategory, Schema.Column>()
        {
            [ColumnTypeCategory.Simple]     = Schema.Column.Simple("Id", "long", "99"),
            [ColumnTypeCategory.Enum]       = Schema.Column.Enum("JoinPolicy", "SecurityPolicy", "byte", "SecurityPolicy.Open"),
            [ColumnTypeCategory.Ref]        = Schema.Column.Ref("Owner", "Employee"),
            [ColumnTypeCategory.RefList]    = Schema.Column.RefList("Members", "Employee")
        };
    }
}
