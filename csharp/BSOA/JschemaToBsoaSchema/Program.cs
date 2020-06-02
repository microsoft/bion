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

            Database db = new Database("SarifLog", "Microsoft.CodeAnalysis.Sarif");

            schema = JsonSchema.Collapse(schema);

            foreach (KeyValuePair<string, JsonSchema> type in schema.Definitions)
            {
                string tableName = type.Key.ToPascalCase();
                Table table = new Table(tableName);
                db.Tables.Add(table);

                // PropertyInfoDictionary.PropertyInfoDictionaryFromSchema
                foreach (KeyValuePair<string, JsonSchema> prop in type.Value.Properties)
                {
                    string columnName = prop.Key.ToPascalCase();

                    if (columnName != "Properties")
                    {
                        table.Columns.Add(ToColumn(tableName, columnName, prop.Value));
                    }
                }
            }

            AsJson.Save(outputPath, db, verbose: true);
            Console.WriteLine("Done.");
        }

        static Column ToColumn(string tableName, string columnName, JsonSchema schema)
        {
            string type = Type(tableName, columnName, schema);

            if (schema.Items?.Schema?.Reference != null)
            {
                return Column.RefList(columnName, Type(tableName, columnName, schema.Items.Schema));
            }
            else if (schema.Items?.Schema.Enum != null)
            {
                return Column.FlagsEnum(columnName, Type(tableName, columnName, schema.Items.Schema), "long");
            }
            else if (schema.Reference != null)
            {
                return Column.Ref(columnName, type);
            }
            else if (schema.Enum != null)
            {
                return Column.Enum(columnName, type, "int");
            }
            else if (type == "DateTime")
            {
                return Column.DateTime(columnName);
            }
            else
            {
                return Column.Simple(columnName, type, schema.Default?.ToString());
            }
        }

        static string Type(string tableName, string columnName, JsonSchema schema)
        {
            SchemaType type = schema.SafeGetType();

            switch (type)
            {
                case SchemaType.Boolean:
                    return "bool";
                case SchemaType.Integer:
                    return "int";
                case SchemaType.Number:
                    return "double";
                case SchemaType.String:
                    if (schema.Format == "uri")
                    {
                        return "Uri";
                    }
                    else if (schema.Format == "date-time")
                    {
                        return "DateTime";
                    }
                    else
                    {
                        return "string";
                    }
                case SchemaType.Object:
                    if (schema.Reference != null)
                    {
                        return schema.Reference.GetDefinitionName().ToPascalCase();
                    }
                    else
                    {
                        string valueType = Type(tableName, columnName, schema.AdditionalProperties.Schema);
                        return $"IDictionary<string, {valueType}>";
                    }
                case SchemaType.Array:
                    string itemType = Type(tableName, columnName, schema.Items.Schema);
                    return $"IList<{itemType}>";
                case SchemaType.None:
                    if (schema.Enum != null)
                    {
                        return $"{tableName}{columnName}";
                    }

                    break;
            }

            throw new NotImplementedException($"Type translation for {tableName}.{columnName} of type {type} not available.");
        }
    }
}
