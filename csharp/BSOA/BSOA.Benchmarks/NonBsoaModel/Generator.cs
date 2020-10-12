﻿using System;
using System.Collections.Generic;

namespace BSOA.Benchmarks.NonBsoaModel
{
    public class Generator
    {
        // NOTE: Copy these from BSOA 'Generator' class to get identical measurements

        private const int RuleCount = 20;
        private const int ResultCount = 1000;

        private static readonly string[] Messages = new string[]
        {
            "This is the first message option",
            "Here is another message option",
            "Messages can be quite long sometimes, though other times they aren't so long at all."
        };

        private static readonly string[] Commits = new string[] { "105adc4", "105adc4", "f56a516", "f3181ce", "2579f8d", "3b22a90", "cd18eb6" };

        private Run Build()
        {
            DateTime when = DateTime.UtcNow;
            Random r = new Random();
            
            Run run = new Run();
            run.Rules = new List<Rule>();
            run.Results = new List<Result>();

            for (int i = 0; i < RuleCount; ++i)
            {
                Rule rule = new Rule();
                rule.Id = $"SCAN{i:D4}";
                rule.Guid = $"{Guid.NewGuid()}";
                rule.HelpUri = new Uri($"https://example.org/rules/{rule.Guid}");

                run.Rules.Add(rule);
            }

            for (int i = 0; i < ResultCount; ++i)
            {
                Result result = new Result();
                result.RuleId = null; // $"SCAN{r.Next(RuleCount - 2):D4}";
                result.IsActive = (i % 3 == 0);
                result.BaselineState = (i % 10 == 4 ? BaselineState.New : BaselineState.Unchanged);
                result.StartLine = r.Next(1000);
                result.Message = Messages[r.Next(Messages.Length)];

                result.Properties = new Dictionary<string, string>();
                result.Properties["ProjectName"] = "BSOA";
                result.Properties["FirstDetectedUtc"] = $"{when.AddMinutes(r.Next(-100000, -10000)):u}";
                result.Properties["LastDetectedUtc"] = $"{when.AddMinutes(r.Next(-500000, 0)):u}";
                result.Properties["Commit"] = Commits[r.Next(Commits.Length)];

                run.Results.Add(result);
            }

            return run;
        }

        public static Run CreateOrLoad()
        {
            return new Generator().Build();
        }
    }
}
