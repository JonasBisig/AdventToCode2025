
namespace AdventToCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read Input File
            IEnumerable<string> inputLines = File.ReadLines("C:\\Users\\jonas\\Downloads\\input1.txt");

            //Dial starts at 50
            int dial = 50;
            int zeroCount = 0;
            foreach (string line in inputLines)
            {
                //Extract Direction -> R = +, L = -
                string direction = line.Substring(0, 1);

                //Extract the click count
                int clicks = int.Parse(line.Substring(1, line.Length - 1));

                //Add or Subtract the clicks from the dial
                if (direction == "R")
                {
                    dial += clicks;
                }
                else
                {
                    dial -= clicks;
                }

                //limit dial value from 0 - 99
                dial = (dial % 100 + 100) % 100;

                //Increment the zeroCount Value
                if (dial == 0)
                {
                    zeroCount++;
                }
            }

            //Display the final Password
            Console.WriteLine(zeroCount);
        }
    }
}