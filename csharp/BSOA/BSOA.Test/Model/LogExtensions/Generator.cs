// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

namespace BSOA.Test.Model.Log
{
    public class Generator
    {
        public const string SampleFilePath = "SampleRun.bsoa";

        private static readonly string[] Messages = new string[]
        {
            "This is the first message option",
            "Here is another message option",
            "Messages can be quite long sometimes, though other times they aren't so long at all."
        };

        private static readonly string[] Commits = new string[] { "105adc4", "105adc4", "f56a516", "f3181ce", "2579f8d", "3b22a90", "cd18eb6" };

        public static Run Build(int ruleCount, int resultCount)
        {
            DateTime when = DateTime.UtcNow;
            Random r = new Random();
            
            Run run = new Run();
            run.Rules = new List<Rule>();
            run.Results = new List<Result>();

            for (int i = 0; i < ruleCount; ++i)
            {
                Rule rule = new Rule();
                rule.Id = $"SCAN{i:D4}";
                rule.Guid = $"{Guid.NewGuid()}";
                rule.HelpUri = new Uri($"https://example.org/rules/{rule.Guid}");

                run.Rules.Add(rule);
            }

            for (int i = 0; i < resultCount; ++i)
            {
                Result result = new Result();
                Rule rule = run.Rules[r.Next(ruleCount - 2)];
                
                result.RuleId = rule.Id;
                result.Rule = rule;

                // result.Guid = null;
                result.IsActive = (i % 3 == 0);
                result.BaselineState = (i % 10 == 4 ? BaselineState.New : BaselineState.Unchanged);
                result.StartLine = r.Next(1000);
                result.Message = Messages[r.Next(Messages.Length)];

                // Add properties; more properties will show Dictionary performance more clearly. Real CloudMine data has 11 properties.
                result.Properties = new Dictionary<string, string>();

                result.Properties["EtlHashCode"] = "1";
                result.Properties["EtlIngestDate"] = $"{when:u}";
                result.Properties["EtlEntity"] = "WorkItem";
                result.Properties["EtlProcessDate"] = $"{when:u}";
                result.Properties["EtlScanDate"] = $"{when:u}";

                result.Properties["State"] = "Valid";
                result.Properties["StartIndex"] = $"{r.Next(10000)}";
                result.Properties["EndIndex"] = $"{r.Next(10000)}";
                result.Properties["MatchedPatternIndex"] = $"{r.Next(5)}";

                result.Properties["ProjectName"] = "BSOA";
                result.Properties["FirstDetectedUtc"] = $"{when.AddMinutes(r.Next(-100000, -10000)):u}";
                result.Properties["LastDetectedUtc"] = $"{when.AddMinutes(r.Next(-500000, 0)):u}";
                result.Properties["Commit"] = Commits[r.Next(Commits.Length)];

                result.Tags = new List<int>();
                result.Tags.Add(i);
                result.Tags.Add(i + 1);
                result.Tags.Add(i - 1);

                run.Results.Add(result);
            }

            run.DB.Trim();

            return run;
        }

        public static Run CreateOrLoad()
        {
            if (File.Exists(SampleFilePath))
            {
                try
                {
                    return Run.ReadBsoa(SampleFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not load BSOA log from prior run. Rebuilding. Exception: \r\n{ex}.");
                }
            }

            Run run = Build(20, 1000);
            run.WriteBsoa(SampleFilePath);
            return run;
        }
    }
}
