// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;

using BSOA.Console.Extensions;
using BSOA.Console.Model;
using BSOA.Console.Model.Normal;
using BSOA.Json;
using BSOA.Test.Components;

using Newtonsoft.Json;

namespace BSOA.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = (args.Length > 0 ? args[0] : @"C:\Code\bion\csharp\BSOA");
            bool rebuild = (args.Length > 1 ? bool.Parse(args[1]) : true);

            string saveAsPath = "Crawl.json";
            Folder root = null;
            if (rebuild || !System.IO.File.Exists(saveAsPath))
            {
                root = FileSystemCrawler.Crawl(rootPath);
                AsJson.Save(saveAsPath, root, verbose: true);
            }
            else
            {
                root = AsJson.Load<Folder>(saveAsPath);
            }

            //System.Console.WriteLine($"{root.SizeBytes.Megabytes():n1} MB   {root.Name}");
            Reporter.WriteHierarchyWithSize(root, System.Console.Out);
        }

        static void Old()
        {
            Sample s = new Sample()
            {
                Age = 39,
                Count = 512,
                IsActive = true,
                Position = 16 * 1024 * 1024,
                Data = new byte[] { 0, 1, 2, 3 }
            };

            string json = JsonConvert.SerializeObject(s);
            Sample t = JsonConvert.DeserializeObject<Sample>(json);

            JsonTextReader r = new JsonTextReader(new StringReader(json));
            while (r.Read())
            {
                JsonToken token = r.TokenType;
                object value = r.Value;
            }

            bool equal = s.Equals(t);

            List<Sample> samples = new List<Sample>()
            {
                s, new Sample()
            };

            json = JsonConvert.SerializeObject(samples);
            List<Sample> rt = JsonConvert.DeserializeObject<List<Sample>>(json);
            bool equal2 = s.Equals(rt[0]);
        }
    }
}
