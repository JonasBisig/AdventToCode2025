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
            string example = """
                123 328  51 64 
                 45 64  387 23 
                  6 98  215 314
                *   +   *   +  
                """;

            //to use the example string enable this line and comment the line below
            //IEnumerable<string> array = example.Split("\r\n");
            IEnumerable<string> array = File.ReadAllLines(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "input.txt"));

            List<List<char>> calculations = new List<List<char>>();
            
            //fill the list with in each row a list of each char of the input
            foreach (string line in array)
            {
                List<char> calcLine = line.ToList();
                calculations.Add(calcLine);
            }
            
            long result = 0;
            long singleProblemSolution = 0;

            //go through each column
            for (int col = calculations[0].Count - 1; col >= 0; col--)
            {
                string addOrMult = " ";
                int offset = col;
                
                //get the operator by going through each char at the most bottom row from the current col backwards
                while (addOrMult == " ")
                {
                    addOrMult = calculations[calculations.Count - 1][offset].ToString();
                    offset--;
                }

                string mergedNumber = "";

                //merge each char to one number
                for (int row = 0; row < calculations.Count - 1; row++)
                {
                    mergedNumber += calculations[row][col];
                }

                //trim the access whitespace
                mergedNumber = mergedNumber.Trim();

                //if a column is only whitespace then dont add or multiply 
                if (mergedNumber == "")
                {
                    continue;
                }

                //add or multiply the merged number to the singleProblemSolution
                if (addOrMult == "+")
                {
                    singleProblemSolution += long.Parse(mergedNumber);
                }
                else
                {
                    //because we multiply here, set the singleProblemSolution to 1 if 0 because 0 x X is still 0
                    if (singleProblemSolution == 0)
                    {
                        singleProblemSolution = 1;
                    }
                    singleProblemSolution *= long.Parse(mergedNumber);
                }

                //if the loop is at the column with the operator, add the singleProblemSolution to the result
                if (calculations[calculations.Count - 1][col] != ' ')
                {
                    result += singleProblemSolution;
                    singleProblemSolution = 0;
                }
            }
            Console.WriteLine(result + " is the grand total found by adding together all of the answers of the problems!");
        }
    }
}