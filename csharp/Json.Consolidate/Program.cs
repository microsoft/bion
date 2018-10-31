using Newtonsoft.Json;
using System;
using System.IO;

namespace Json.Consolidate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string sourceDirectory = args[0];
            string outputDirectory = args[1];

            JsonConsolidator consolidator = new JsonConsolidator(sourceDirectory, outputDirectory);
            consolidator.Consolidate();
        }
    }

    internal class JsonConsolidator
    {
        public string SourceDirectory;
        public string OutputDirectory;
        public const long TargetSizeBytes = 512 * 1024 * 1024;   // 512 MB

        private int _countForDay;
        private string _outputPath;
        private StreamWriter _sWriter;
        private JsonTextWriter _writer;

        public JsonConsolidator(string sourceDirectory, string outputDirectory)
        {
            SourceDirectory = sourceDirectory;
            OutputDirectory = outputDirectory;

            Directory.CreateDirectory(sourceDirectory);
            Directory.CreateDirectory(outputDirectory);
        }

        public void Consolidate()
        {
            // For each day in the source directory tree, ...
            foreach (string yearFolder in Directory.GetDirectories(SourceDirectory))
            {
                foreach (string monthFolder in Directory.GetDirectories(yearFolder))
                {
                    foreach (string dayFolder in Directory.GetDirectories(monthFolder))
                    {
                        string outputDay = Path.Combine(OutputDirectory, Path.GetFileName(yearFolder), Path.GetFileName(monthFolder), Path.GetFileName(dayFolder));

                        // If the output was already written, stop
                        if (Directory.Exists(outputDay)) { continue; }

                        _countForDay = 1;

                        try
                        {
                            foreach (string jsonFile in Directory.GetFiles(dayFolder, "*.*", SearchOption.AllDirectories))
                            {
                                EnsureWriter(outputDay);

                                // Copy the JSON file (without whitespace)
                                using (JsonTextReader reader = new JsonTextReader(new StreamReader(jsonFile)))
                                {
                                    _writer.WriteToken(reader);
                                }
                            }

                            // Close the writer after the day is converted
                            CloseWriter(true);
                        }
                        finally
                        {
                            CloseWriter(true);
                        }
                    }
                }
            }
        }

        private void EnsureWriter(string outputDay)
        {
            // Close the previous writer if the file has reached the target size
            CloseWriter(false);

            // Create a new writer if there isn't one
            if (_writer == null)
            {
                Directory.CreateDirectory(outputDay);
                _outputPath = Path.Combine(outputDay, $"Block.{_countForDay}.json");
                _sWriter = new StreamWriter(_outputPath);
                _writer = new JsonTextWriter(_sWriter);

                _writer.Formatting = Formatting.None;
                _writer.WriteStartArray();
            }
        }

        private void CloseWriter(bool unconditional)
        {
            if(_writer == null) { return; }

            // Close writer if file has reached target size
            _writer.Flush();
            if (unconditional || _sWriter.BaseStream.Length >= TargetSizeBytes)
            {
                _writer.WriteEndArray();

                ((IDisposable)_writer).Dispose();
                _sWriter.Dispose();
                _writer = null;
                _sWriter = null;

                Console.WriteLine($" => {_outputPath}");
                _countForDay++;
            }
        }
    }
}
