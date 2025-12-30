using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventToCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //example string
            string example = "987654321111111,811111111111119,234234234234278,818181911112111";

            //to use the example string enable this line and comment the line below
            IEnumerable<string> batteries = example.Split(",");
            //IEnumerable<string> batteries = File.ReadLines(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "input.txt"));

            long result = 0;
            foreach (string battery in batteries)
            {
                //convert the string in a List full of integer
                List<int> searchRange = battery.Select(c => int.Parse(c.ToString())).ToList();

                //create a reserve list, containing the last 12 integer of searchRange
                List<int> reserve = searchRange.GetRange(searchRange.Count - 12, 12);

                //delete the last 12 integer in searchRange
                searchRange.RemoveRange(searchRange.Count - 12, 12);

                string joltage = "";
                int i = 0;
                while (reserve.Count > 0)
                {
                    //change searchRange to -> last found int - end of list
                    searchRange = searchRange.GetRange(i, searchRange.Count - i);

                    //add the first reserve int to the searchRange
                    searchRange.Add(reserve[0]);

                    //remove this int from reserve
                    reserve.RemoveRange(0, 1);

                    //get the index of the max int in the searchRange
                    i = searchRange.IndexOf(searchRange.Max());

                    //add the int to the joltage
                    joltage += searchRange[i];

                    //increment i to exclude this int in the next loop
                    i++;
                }

                //add joltage to result
                result += long.Parse(joltage);
            }
            Console.WriteLine("Highest joltage is: " + result);
        }
    }
}