// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

using BSOA.Generator.Extensions;
using BSOA.Generator.Schema;
using BSOA.Json;
using SColumn = BSOA.Generator.Schema.Column;

using Microsoft.Json.Schema;

namespace BSOA.FromJSchema
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

        static int Main(string[] args)
        {
            if(args.Length < 4)
            {
                Console.WriteLine("Usage: BSOA.FromJSchema <JSchemaPath> <OutputBsoaSchemaPath> <RootTypeName> <OutputNamespace>");
                return -2;
            }

            try
            {
                string jschemaPath = args[0];
                string outputPath = args[1];
                string rootTypeName = args[2];
                string outputNamespace = args[3];

                Console.WriteLine($"Converting jschema\r\n  '{jschemaPath}' to \r\n  '{outputPath}'...");
                JsonSchema schema = null;

                using (StreamReader sr = File.OpenText(jschemaPath))
                {
                    schema = SchemaReader.ReadSchema(sr, jschemaPath);
                }

                schema = JsonSchema.Collapse(schema);

                Database db = new Database($"{rootTypeName}Database", outputNamespace, rootTypeName);

                Table root = new Table(rootTypeName);
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

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
                return -1;
            }
        }

        static void AddColumns(Table table, JsonSchema schema)
        {
            // PropertyInfoDictionary.PropertyInfoDictionaryFromSchema
            foreach (KeyValuePair<string, JsonSchema> prop in schema.Properties)
            {
                string columnName = prop.Key.ToPascalCase();
                table.Columns.Add(ToColumn(table.Name, columnName, prop.Value));
            }
        }

        static SColumn ToColumn(string tableName, string columnName, JsonSchema schema)
        {
            SchemaType type = schema.SafeGetType();
            string defaultValue = schema.Default?.ToString();
            if (NameRenames.TryGetValue(columnName, out string columnRename)) { columnName = columnRename; }

            if (columnName == "Properties")
            {
                return SColumn.Simple("Properties", "IDictionary<String, SerializedPropertyInfo>");
            }

            switch (type)
            {
                case SchemaType.Boolean:
                    return SColumn.Simple(columnName, "bool", defaultValue?.ToLowerInvariant());
                case SchemaType.Integer:
                    return SColumn.Simple(columnName, "int", defaultValue);
                case SchemaType.Number:
                    return SColumn.Simple(columnName, "double", defaultValue);
                case SchemaType.String:
                    if (schema.Format == "uri" || schema.Format == "uri-reference")
                    {
                        return SColumn.Simple(columnName, "Uri");
                    }
                    else if (schema.Format == "date-time")
                    {
                        return SColumn.Simple(columnName, "DateTime");
                    }
                    else
                    {
                        if (defaultValue != null) { defaultValue = "\"" + defaultValue + "\""; }
                        return SColumn.Simple(columnName, "String", defaultValue);
                    }
                case SchemaType.Object:
                    if (schema.Reference != null)
                    {
                        string refType = schema.Reference.GetDefinitionName().ToPascalCase();
                        if (TypeRenames.TryGetValue(refType, out string renamed)) { refType = renamed; }
                        return SColumn.Ref(columnName, refType);
                    }
                    else
                    {
                        string valueType = ToColumn(tableName, columnName, schema.AdditionalProperties.Schema).Type;
                        return SColumn.Simple(columnName, $"IDictionary<String, {valueType}>");
                    }
                case SchemaType.Array:
                    SColumn itemType = ToColumn(tableName, columnName, schema.Items.Schema);

                    if (itemType.Category == ColumnTypeCategory.Enum)
                    {
                        // NOTE: DefaultValue not translated for FlagsEnum
                        string enumType = itemType.Type;
                        return SColumn.Enum(columnName, enumType, "int", $"default({enumType})");
                    }
                    else if (itemType.ReferencedTableName != null)
                    {
                        return SColumn.RefList(columnName, itemType.ReferencedTableName);
                    }
                    else
                    {
                        return SColumn.Simple(columnName, $"IList<{itemType.Type}>");
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

                        return SColumn.Enum(columnName, enumType, "int", defaultValue);
                    }

                    break;
            }

            throw new NotImplementedException($"Type translation for {tableName}.{columnName} of type {type} not available.");
        }
    }
}
