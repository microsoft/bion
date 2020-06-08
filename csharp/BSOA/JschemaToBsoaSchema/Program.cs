using BSOA.Generator.Extensions;
using BSOA.Generator.Schema;
using BSOA.Json;

using Microsoft.Json.Schema;

using System;
using System.Collections.Generic;
using System.IO;

namespace JschemaToBsoaSchema
{
    class Program
    {
        static Dictionary<string, string> NameRenames = new Dictionary<string, string>()
        {
            ["$schema"] = "SchemaUri"
        };

        static Dictionary<string, string> TypeRenames = new Dictionary<string, string>()
        {
            ["Exception"] = "ExceptionData",
            ["$schema"] = "SchemaUri",
            ["SarifLogVersion"] = "SarifVersion",
            ["RootVersion"] = "SarifVersion",
            ["ExternalPropertiesVersion"] = "SarifVersion",
            ["RunColumnKind"] = "ColumnKind",
            ["ResultBaselineState"] = "BaselineState",
            ["ResultLevel"] = "FailureLevel",
            ["NotificationLevel"] = "FailureLevel",
            ["ReportingConfigurationLevel"] = "FailureLevel"
        };

        static void Main(string[] args)
        {
            string jschemaPath = (args.Length > 0 ? args[0] : @"C:\Code\sarif-sdk\src\Sarif\Schemata\sarif-2.1.0-rtm.5.json");
            string outputPath = (args.Length > 1 ? args[1] : @"..\..\..\..\BSOA.Generator\Schemas\Sarif-2.1.0-rtm.5.schema.json");

            Console.WriteLine($"Converting jschema\r\n  '{jschemaPath}' to \r\n  '{outputPath}'...");
            JsonSchema schema = null;

            using (StreamReader sr = File.OpenText(jschemaPath))
            {
                schema = SchemaReader.ReadSchema(sr, jschemaPath);
            }

            schema = JsonSchema.Collapse(schema);

            Database db = new Database("SarifLogDatabase", "Microsoft.CodeAnalysis.Sarif", "SarifLog");

            Table root = new Table("SarifLog");
            db.Tables.Add(root);
            AddColumns(root, schema);

            foreach (KeyValuePair<string, JsonSchema> type in schema.Definitions)
            {
                string tableName = type.Key.ToPascalCase();
                if (TypeRenames.TryGetValue(tableName, out string renamed)) { tableName = renamed; }

                Table table = new Table(tableName);
                AddColumns(table, type.Value);
                db.Tables.Add(table);
            }

            AsJson.Save(outputPath, db, verbose: true);
            Console.WriteLine("Done.");
            Console.WriteLine();
        }

        static void AddColumns(Table table, JsonSchema schema)
        {
            // PropertyInfoDictionary.PropertyInfoDictionaryFromSchema
            foreach (KeyValuePair<string, JsonSchema> prop in schema.Properties)
            {
                string columnName = prop.Key.ToPascalCase();
                if (columnName == "Properties") { continue; }

                table.Columns.Add(ToColumn(table.Name, columnName, prop.Value));
            }
        }

        static Column ToColumn(string tableName, string columnName, JsonSchema schema)
        {
            SchemaType type = schema.SafeGetType();
            string defaultValue = schema.Default?.ToString();
            if (NameRenames.TryGetValue(columnName, out string columnRename)) { columnName = columnRename; }

            switch (type)
            {
                case SchemaType.Boolean:
                    return Column.Simple(columnName, "bool", defaultValue?.ToLowerInvariant());
                case SchemaType.Integer:
                    return Column.Simple(columnName, "int", defaultValue);
                case SchemaType.Number:
                    return Column.Simple(columnName, "double", defaultValue);
                case SchemaType.String:
                    if (schema.Format == "uri" || schema.Format == "uri-reference")
                    {
                        return Column.Simple(columnName, "Uri");
                    }
                    else if (schema.Format == "date-time")
                    {
                        return Column.Simple(columnName, "DateTime");
                    }
                    else
                    {
                        if (defaultValue != null) { defaultValue = "\"" + defaultValue + "\""; }
                        return Column.Simple(columnName, "string", defaultValue);
                    }
                case SchemaType.Object:
                    if (schema.Reference != null)
                    {
                        string refType = schema.Reference.GetDefinitionName().ToPascalCase();
                        if (TypeRenames.TryGetValue(refType, out string renamed)) { refType = renamed; }
                        return Column.Ref(columnName, refType);
                    }
                    else
                    {
                        string valueType = ToColumn(tableName, columnName, schema.AdditionalProperties.Schema).Type;
                        return Column.Simple(columnName, $"IDictionary<string, {valueType}>");
                    }
                case SchemaType.Array:
                    Column itemType = ToColumn(tableName, columnName, schema.Items.Schema);

                    if (itemType.Category == ColumnTypeCategory.Enum)
                    {
                        // NOTE: DefaultValue not translated for FlagsEnum
                        string enumType = itemType.Type;
                        return Column.Enum(columnName, enumType, "int", $"default({enumType})");
                    }
                    else if (itemType.ReferencedTableName != null)
                    {
                        return Column.RefList(columnName, itemType.ReferencedTableName);
                    }
                    else
                    {
                        return Column.Simple(columnName, $"IList<{itemType.Type}>");
                    }
                case SchemaType.None:
                    if (schema.Enum != null)
                    {
                        string enumType = $"{tableName}{columnName}";
                        if (TypeRenames.TryGetValue(enumType, out string renamedType)) { enumType = renamedType; }

                        if (defaultValue == null)
                        {
                            defaultValue = $"default({enumType})";
                        }
                        else
                        {
                            defaultValue = $"{enumType}.{defaultValue.ToPascalCase()}";
                        }

                        return Column.Enum(columnName, enumType, "int", defaultValue);
                    }

                    break;
            }

            throw new NotImplementedException($"Type translation for {tableName}.{columnName} of type {type} not available.");
        }
    }
}
