// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = (args.Length > 0 ? args[0] : @"C:\Download\Demo\V2\Inputs\CodeAsData.sarif"); // .20000.sarif");
            string workingFolderPath = @"C:\Download\Demo\V2";
            bool forceReconvert = (args.Length > 1 ? bool.Parse(args[1]) : false);

            // Tiny demo file
            //inputFilePath = @"C:\Download\Demo\V2\Inputs\elfie-arriba.sarif";
            forceReconvert = true;

            Benchmarker benchmarker = new Benchmarker(inputFilePath, workingFolderPath);
            benchmarker.Run(forceReconvert);
        }
    }
}
