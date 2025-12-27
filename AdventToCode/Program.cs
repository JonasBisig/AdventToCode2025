using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AdventToCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //example string
            string example = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";

            //to use the example string enable this line and comment the line below
            //string[] ranges = example.Split(",");
            string[] ranges = File.ReadAllText("C:\\Users\\jonas\\source\\repos\\JonasBisig\\AdventToCode\\AdventToCode\\bin\\input.txt").Split(",");
            long result = 0;

            //go through each range
            foreach (string range in ranges)
            {
                //go through each id in the range start from the first id and end with the last id
                for (long id = long.Parse(range.Split("-")[0]); id <= long.Parse(range.Split("-")[1]); id++)
                {
                    //split the id in half with substring
                    string firstHalf = id.ToString().Substring(0, id.ToString().Length / 2);
                    string secondHalf = id.ToString().Substring(id.ToString().Length / 2);
                    
                    //check if the two halfes are identical, then the id is invalid
                    if (firstHalf == secondHalf)
                    {
                        //add the invalid id to the result
                        Console.WriteLine("Invalid ID: " + id);
                        result += id;
                    }
                }
            }
            Console.WriteLine("The Result is: " + result);
        }
    }
}