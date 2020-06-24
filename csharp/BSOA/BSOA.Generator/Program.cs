// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace BSOA.Generator
{
    /// <summary>
    ///  BSOA.Generator generates a BSOA object model (Entity classes, Table classes, and a Database class)
    ///  from schema information in a JSON format. See Schemas\ for examples.
    ///  
    ///  This code uses Regexes and string replacement to generate the output files from the templates.
    ///  Roslyn is much more flexible, but Roslyn generation code is long and complex.
    ///  [Ex: See https://github.com/microsoft/jschema/blob/master/src/Json.Schema.ToDotNet/ClassGenerator.cs#L1129]
    ///  
    ///  You provide templates for each class you want generated.
    ///  Templates use known values for the namespace, database name, table name, and column properties.
    ///  See Generation\TemplateDefaults.cs for the expected known values.
    ///  
    ///  Within each template, the code will find all &lt;[TemplateName]List&gt; comment blocks.
    ///  It will generate per-column or per-table replacements by replacing the value from the
    ///  &lt;[ColumnTypeCategory][TemplateName]&gt; or &lt;[TemplateName]&gt; block, and then replace
    ///  the list block with the created output. The code then replaces the Database name, Table name, and namespace.
    ///  
    ///  This logic is straightforward and means you can make a working template which can be unit tested,
    ///  annotate it with comments indicating where to make replacements, and then get predictable generated
    ///  outputs for any schema.
    /// </summary>
    class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: BSOA.Generator <SchemaJsonFile> <OutputFolder> [<TemplateOverridesFolderPath>]? [<PostReplacementsJsonPath>]?");
                return -2;
            }

            try
            {
                FullGenerator generator = new FullGenerator(
                    schemaPath: args[0],
                    outputFolder: (args.Length > 1 ? args[1] : @"Model"),
                    templateOverridesFolderPath: (args.Length > 2 ? args[2] : null),
                    postReplacementPath: (args.Length > 3 ? args[3] : null)
                );

                generator.Generate();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return -1;
            }
        }
    }
}
