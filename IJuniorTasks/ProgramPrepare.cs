using System;
using System.Collections.Generic;
using System.Linq;

namespace IJuniorTasks
{
    internal class ProgramPrepare
    {
        static void MainPrepare(string[] args)
        {
            var listFirst = new List<string> { "1", "a", "s", "1" };
            var listSecond = new List<string> { "a", "d", "d" };
            var hashSet = new HashSet<string>();

            foreach (var item in listFirst)
            {
                hashSet.Add(item);
            }

            foreach (var item in listSecond)
            {
                hashSet.Add(item);
            }

            var listResult = hashSet.ToList();
            Console.WriteLine(string.Join(", ", listResult));

           
        }
    }
}