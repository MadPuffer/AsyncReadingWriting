using System;
using System.IO;
using System.Collections.Generic;
namespace Parser
{
    public static class FileParser
    {
        public static List<string> Parse(string path)
        {
            var records = new List<string>();
            using (var reader = File.OpenText(path))
            {
                while (!reader.EndOfStream)
                {
                    var record = reader.ReadLine();
                    if (record.Split(';')[0].StartsWith("Play"))
                    {
                        var splittedRecord = record.Split(';');
                        var channel = splittedRecord[1].Split('=')[1];
                        var date = DateTime.Parse(splittedRecord[2].Split('=')[1]);
                        int duration = (int) (DateTime.Parse(splittedRecord[3]
                            .Split('=')[1]) - date).TotalSeconds;

                        records.Add($"{channel}, {date}, {duration}");
                    }
                }
                reader.Close();
            }
            return records;
        }
    }
}