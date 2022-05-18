using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileParser
{
    public class RecordsFormatter
    {
        public void Format(string sourceDirPath, string destinationFilePath)
        {
            string[] files = Directory.GetFiles(sourceDirPath);
            ReadAndProcessFile(files, destinationFilePath);
        }

        public void ReadAndProcessFile(string[] filePaths, string distFile)
        {
            var lines = new BlockingCollection<string>();
            var parsedLines = new BlockingCollection<string>();

            var fileReadingTask = Task.Run(() =>
            {
                try
                {
                    foreach (var file in filePaths)
                    {
                        using (var reader = new StreamReader(file))
                        {
                            string line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                lines.Add(line);
                            }
                        }
                    }
                }
                finally
                {
                    lines.CompleteAdding();
                }
            });
            
            var parsingLinesTask = Task.Run(() =>
            {
                try
                {
                    var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                    Parallel.ForEach(lines.GetConsumingEnumerable(), parallelOptions, line =>
                    {
                        if (line.StartsWith("Play"))
                        {
                            try
                            {
                                var splittedRecord = line.Split(';');
                                var channel = splittedRecord[1].Split('=')[1];
                                var date = splittedRecord[2].Split('=')[1];
                                int duration = (int)(DateTime.Parse(splittedRecord[3]
                                    .Split('=')[1]) - DateTime.Parse(date)).TotalSeconds;
                                parsedLines.Add($"{channel}, {date}, {duration}");
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    });
                }
                finally
                {
                    parsedLines.CompleteAdding();
                }
            });

            var parsedLineWritingTask = Task.Run(() =>
            {
                using (StreamWriter sw = new StreamWriter(distFile))
                {
                    foreach (string line in parsedLines.GetConsumingEnumerable())
                    {
                        sw.WriteLine(line);
                    }

                    sw.Close();
                }
            });
            Task.WaitAll(fileReadingTask, parsingLinesTask, parsedLineWritingTask);
        }
    }
}