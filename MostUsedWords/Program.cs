using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MostUsedWords
{
    // есть папка с несколькими большими файлами (нельзя загружать полностью в память)
    // нужно прочитать все файлы, и выписать топ 10 самых часто встречающихся слов
    // слова выбирать с минимальным  размером minLength, заданным в конфиге

    class Program
    {
        public class Counter
        {
            public int WordCounter { get; set; }
        }
        //static Regex regex;

        static Task Main(string[] args)
        {
            var dirPath = Initialize(out var minLength);

            const int topLines = 10;
            var dictionary = new Dictionary<string, Counter>();
            var list = new ConcurrentBag<Dictionary<string, Counter>>();
            var files = Directory.EnumerateFiles(dirPath);
            var sw = Stopwatch.StartNew();
            var regexPattern = $"\\b\\w{{{minLength},}}\\b";
            //regex = new Regex(regexPattern, RegexOptions.Compiled);
            

            Parallel.ForEach(files, file =>
            {
                FileRead(file, regexPattern, list);
            });
            Console.WriteLine($"Elapsed on read: {sw.Elapsed.TotalSeconds: 0.00}");
            foreach (var dict in list)
            {
                foreach (var kv in dict)
                {
                    if (dictionary.TryGetValue(kv.Key, out var val))
                    {
                        dictionary[kv.Key].WordCounter = val.WordCounter + kv.Value.WordCounter;
                    }
                    else
                    {

                        dictionary[kv.Key] = kv.Value;
                    }
                }
            }

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

        private static void FileRead(string file, string regexPattern, ConcurrentBag<Dictionary<string, Counter>> list)
        {
            var dictionary = new Dictionary<string, Counter>();
            
            var regex = new Regex(regexPattern, RegexOptions.Compiled);
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
                    if (dictionary.TryGetValue(word, out var counter))
                    {
                        counter.WordCounter++;
                        continue;
                    }

                    dictionary[word] = new Counter
                    {
                        WordCounter = 1
                    };
                }
            }

            list.Add(dictionary);
        }
    }
}
