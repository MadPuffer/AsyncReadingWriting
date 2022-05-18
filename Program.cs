using System;
using System.IO;
using System.Collections.Generic;

namespace Parser
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string sourceDirName = Console.ReadLine();
            string destinationFileName = Console.ReadLine();
            string[] files = Directory.GetFiles(sourceDirName);
            List<List<string>> parsedFiles = new List<List<string>>();

            foreach(var i in files)
            {
                parsedFiles.Add(FileParser.Parse(i));
            }

            File.Create(destinationFileName);
            {
                foreach (var records in parsedFiles)
                {
                    File.WriteAllLines(destinationFileName ,records);
                }
            }

        }
    }
}
