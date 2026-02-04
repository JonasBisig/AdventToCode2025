using System;
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
            string example = "";

            //to use the example string enable this line and comment the line below
            //IEnumerable<string> array = example.Split(",");
            IEnumerable<string> array = File.ReadLines(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "input.txt"));
        }
    }
}