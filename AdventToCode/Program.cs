using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventToCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //example string
            string example = """
                7,1                
                11,1
                11,7
                9,7
                9,5
                2,5
                2,3
                7,3
                """;

            //to use the example string enable this line and comment the line below
            IEnumerable<string> array = example.Split("\r\n");
            //IEnumerable<string> array = File.ReadAllLines(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "input.txt"));

            //get every combination of positions and order them by areasize descending -> largest area at pos 0
            List<Tuple<(double, double), (double, double)>> areas = array.SelectMany((a, i) => array.Skip(i + 1).Select(b => ((double.Parse(a.Split(",")[0]), double.Parse(a.Split(",")[1])), (double.Parse(b.Split(",")[0]), double.Parse(b.Split(",")[1]))).ToTuple()))
                .OrderByDescending(x => (Math.Abs(x.Item1.Item1 - x.Item2.Item1) + 1) * (Math.Abs(x.Item1.Item2 - x.Item2.Item2) + 1))
                .ToList();

            //get the largest area rectangle
            double result = (Math.Abs(areas[0].Item1.Item1 - areas[0].Item2.Item1) + 1) * (Math.Abs(areas[0].Item1.Item2 - areas[0].Item2.Item2) + 1);

            Console.WriteLine($"The largest area of any rectangle you can make is {result}!");

            //Different solution -> single line
            //directly calculate area sizes for every combination and order them descending then only display the first position of the list
            List<double> areas2 = array.SelectMany((a, i) => array.Skip(i + 1).Select(b => (Math.Abs(double.Parse(a.Split(",")[0]) - double.Parse(b.Split(",")[0])) + 1) * (Math.Abs(double.Parse(a.Split(",")[1]) - double.Parse(b.Split(",")[1])) + 1)))
                .OrderByDescending(x => x)
                .ToList();

            Console.WriteLine($"Different approach, same solution: {areas2[0]}");
        }
    }
}