using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //string sourceDirName = Console.ReadLine();
            string sourceDirName = "medium";
            string destinationFileName = "test.txt";
            //string destinationFileName = Console.ReadLine();
            
            RecordsFormatter formatter = new RecordsFormatter();
            var s = new Stopwatch();
            s.Start();;
            formatter.Format(sourceDirName, destinationFileName);
            s.Stop();
            Console.WriteLine(s.Elapsed);
        }
        
    }
}