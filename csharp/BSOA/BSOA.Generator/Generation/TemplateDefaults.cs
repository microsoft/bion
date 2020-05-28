using BSOA.Generator.Schema;

using System;
using System.Collections.Generic;

namespace BSOA.Generator.Generation
{
    public static class TemplateDefaults
    {
        public static string Populate(Dictionary<string, string> templates, string templateName, Schema.Column column, Table table, Database database)
        {
            string template;
            if (templates.TryGetValue($"{column.Category}{templateName}", out template))
            {
                switch (column.Category)
                {
                    case ColumnTypeCategory.Simple:
                        return template
                            .Replace("EmployeeId", column.Name)
                            .Replace("employeeId", CamelCase(column.Name))
                            .Replace("long", column.Type)
                            .Replace("-1", column.Default);
                    case ColumnTypeCategory.DateTime:
                        return template
                            .Replace("WhenFormed", column.Name)
                            .Replace("whenFormed", CamelCase(column.Name))
                            .Replace("DateTime.MinValue", column.Default)
                            .Replace("DateTime", column.Type);
                    case ColumnTypeCategory.Enum:
                        return template
                            .Replace("JoinPolicy", column.Name)
                            .Replace("joinPolicy", CamelCase(column.Name))
                            .Replace("SecurityPolicy.Open", column.Default)
                            .Replace("SecurityPolicy", column.Type)
                            .Replace("byte", column.UnderlyingType);
                    case ColumnTypeCategory.FlagsEnum:
                        return template
                            .Replace("GroupAttributes", column.Type)
                            .Replace("GroupAttributes.None", column.Default)
                            .Replace("Attributes", column.Name)
                            .Replace("attributes", CamelCase(column.Name))
                            .Replace("long", column.UnderlyingType);
                    case ColumnTypeCategory.Ref:
                        return template
                            .Replace("Manager", column.Name)
                            .Replace("manager", CamelCase(column.Name))
                            .Replace("Employee", column.Type)
                            .Replace("CompanyDatabase", database.Name);
                    case ColumnTypeCategory.RefList:
                        return template
                            .Replace("Members", column.Name)
                            .Replace("members", CamelCase(column.Name))
                            .Replace("IList<Employee>", column.Type)
                            .Replace("Employee", column.ReferencedTableName)
                            .Replace("CompanyDatabase", database.Name);
                    default:
                        throw new NotImplementedException($"Populate not implemented with defaults to replace for category {column.Category} ({column.Name})");
                }
            }

            if (templates.TryGetValue(templateName, out template))
            {
                return template
                    .Replace("EmployeeId", column.Name)
                    .Replace("employeeId", CamelCase(column.Name))
                    .Replace("long", column.Type)
                    .Replace("-1", column.Default);
            }

            throw new NotSupportedException($"Could not find template '{column.Category}{templateName}' or '{templateName}' in collection: ({string.Join(", ", templates.Keys)}");
        }

        public static string CamelCase(string value)
        {
            return Char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
