using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MostUsedWords
{

    class Program
    {
        public class Counter
        {
            public int WordCounter { get; set; }
        }

        static Task Main(string[] args)
        {
            var dirPath = Initialize(out var minLength);

            const int topLines = 10;
            var dictionary = new ConcurrentDictionary<string, Counter>();
            var files = Directory.EnumerateFiles(dirPath);
            var sw = Stopwatch.StartNew();

            //var parallelOptions = new ParallelOptions
            //{
            //    MaxDegreeOfParallelism = Environment.ProcessorCount
            //};
            Parallel.ForEach(files, file =>
            {
                FileRead(file, minLength, dictionary);
            });


            var ordered = dictionary
                .OrderByDescending(o => o.Value.WordCounter)
                .Take(topLines);
            var result = ordered.Select(o => $"{o.Key}: {o.Value.WordCounter}");

            Console.WriteLine($"Elapsed: {sw.Elapsed.TotalSeconds: 0.00}");
            Console.WriteLine(string.Join(Environment.NewLine, result));
            Console.ReadKey();
            return Task.CompletedTask;
        }

        private static string Initialize(out int minLength)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var dirPath = configuration["DirPath"];
            minLength = int.Parse(configuration["MinLength"]);
            return dirPath;
        }

        private static void FileRead(string file, int minLength, ConcurrentDictionary<string, Counter> dictionary)
        {
            var regex = new Regex(@"\b\w+\b");
            using var f = new StreamReader(file);
            while (!f.EndOfStream)
            {
                var line = f.ReadLine();
                if (line == null)
                {
                    continue;
                }
                var matches = regex.Matches(line);
                foreach (Match match in matches)
                {
                    var word = match.Value;
                    if (word.Length < minLength)
                    {
                        continue;
                    }
                    if (dictionary.TryGetValue(word, out var counter))
                    {
                        dictionary[word].WordCounter++;
                        continue;
                    }

                    dictionary[word] = new Counter
                    {
                        WordCounter = 1
                    };
                }
            }
        }
    }
}
