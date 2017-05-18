using Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class P
    {
        [Csv("p1", 1)]
        public int P1 { get; set; }
        [Csv("p2", 0)]
        public string P2 { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<P>();
            list.Add(new P { P1 = 1, P2 = "Hi" });
            list.Add(new P { P1 = 2, P2 = "My" });
            list.Add(new P { P1 = 3, P2 = "Name" });
            list.Add(new P { P1 = 4, P2 = "Is" });
            list.Add(new P { P1 = 5, P2 = "Lenny" });

            var m = new Csv.CsvWriter<P>(';').RunAsync(list).Result;
        }
    }
}
