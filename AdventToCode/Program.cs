using System;
using System.Collections.Generic;
using System.IO;
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
                    //check each split possibility for the id
                    for (int x = 2; x <= id.ToString().Length; x++)
                    {
                        List<string> sequences = new List<string>();

                        //if this variable is not 0 then the id cant be devided in x many parts
                        int canDevide = id.ToString().Length % x;
                        if (canDevide == 0)
                        {
                            //devide the id in x many parts and save it in sequences
                            for (int i = 0; i < x; i++)
                            {
                                sequences.Add(id.ToString().Substring(i * id.ToString().Length / x, id.ToString().Length / x));
                            }

                            //if all parts of the splited id are identical then the id is an invalid id and its added to the result
                            if(sequences.All(x => x == sequences[0]))
                            {
                                Console.WriteLine("Invalid ID: " + id);
                                result += id;
                                break;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("The Result is: " + result);
        }
    }
}