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
            string example = "123 328 51 64,45 64 387 23,6 98 215 314,* + * +";

            //to use the example string enable this line and comment the line below
            //IEnumerable<string> array = example.Split(",");
            IEnumerable<string> array = File.ReadAllLines(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "input.txt"));

            List<List<string>> calculations = new List<List<string>>();
            long result = 0;

            foreach (string line in array)
            {
                //split each line by whitespaces, because there can be multiple whitespaces, remove all nullOrEmpty entries from the list
                List<string> calcLine = line.Split(" ").ToList();
                calcLine.RemoveAll(string.IsNullOrEmpty);
                calculations.Add(calcLine);
            }

            //go for as many colums (calculations) there are
            for (int col = 0; col < calculations[0].Count; col++)
            {
                bool addOrMult = calculations[calculations.Count - 1][col] == "+";
                long singleProblemSolution = 0;

                if (addOrMult)
                {
                    //go for as many rows each calculation has
                    for (int row = 0; row < calculations.Count - 1; row++)
                    {
                        singleProblemSolution += long.Parse(calculations[row][col]);
                    }
                }
                else
                {
                    //because we multiply here, set the singleProblemSolution to 1 because 0 x X is still 0
                    singleProblemSolution = 1;

                    //go for as many rows each calculation has
                    for (int row = 0; row < calculations.Count - 1; row++)
                    {
                        singleProblemSolution *= long.Parse(calculations[row][col]);
                    }
                }
                //add the result of each calculation to the result
                result += singleProblemSolution;
            }

            Console.WriteLine(result + " is the grand total found by adding together all of the answers of the problems!");
        }
    }
}