using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MostUsedWords
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var dirPath = Initialize(out var minLength);

            const int topLines = 10;
            var dictionary = new ConcurrentDictionary<string, int>();
            var files = Directory.EnumerateFiles(dirPath);
            var sw = Stopwatch.StartNew();


            // simpliest way
            //Parallel.ForEach(files, file =>
            //{
            //    FileRead(file, minLength, dictionary);
            //});


            var tasks = new List<Task>();
            foreach (var file in files)
            {
                var task = Task.Factory.StartNew(() => FileRead(file, minLength, dictionary));
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);


            var ordered = dictionary.OrderByDescending(o => o.Value).Take(topLines);
            var result = ordered.Select(o => $"{o.Key}: {o.Value}");
            
            Console.WriteLine($"Elapsed: {sw.Elapsed.TotalSeconds : 0.00}");
            Console.WriteLine(string.Join(Environment.NewLine, result));
            Console.ReadKey();
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

        private static void FileRead(string file, int minLength, ConcurrentDictionary<string, int> dictionary)
        {
            using var f = new StreamReader(file);
            while (!f.EndOfStream)
            {
                var line = f.ReadLine();
                var words = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var word in words)
                {
                    if (word.Length < minLength)
                    {
                        continue;
                    }
                    if (dictionary.TryGetValue(word, out var count))
                    {
                        dictionary[word] = count + 1;
                        continue;
                    }

                    dictionary[word] = 1;
                }
            }
        }
    }
}
