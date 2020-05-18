﻿using BSOA.Demo.Model;
using Sarif = Microsoft.CodeAnalysis.Sarif;
using System;

namespace BSOA.Demo
{
    public static class ConstructionSamples
    {
        // From C:\Code\sarif-sdk\src\Test.UnitTests.Sarif\Baseline\ResultMatching\ResultMatchingTestHelpers.cs
        // Converted nulls to default(T)
        public static Result CreateMatchingResult(string target, string location, string regionContent, string contextRegionContent = null)
        {
            Result result = new Result()
            {
                RuleId = "TEST001",
                //Level = Sarif.FailureLevel.Error,
            };

            //if (target != null)
            //{
            //    result.AnalysisTarget = new ArtifactLocation()
            //    {
            //        Uri = new Uri(target)
            //    };
            //}

            if (location != null)
            {
                result.Locations =
                    new Location[]
                   {
                        new Location()
                        {
                            PhysicalLocation = new PhysicalLocation()
                            {
                                ArtifactLocation = new ArtifactLocation()
                                {
                                    Uri = new Uri(location)
                                },
                                Region = regionContent != null ? new Region() { StartLine = 5, Snippet = new ArtifactContent() { Text = regionContent } } : default(Region),
                                ContextRegion = contextRegionContent != null ? new Region() { StartLine = 10, Snippet = new ArtifactContent { Text = contextRegionContent } } : default(Region),
                            }
                        }
                   };
            }

            return result;
        }
    }
}