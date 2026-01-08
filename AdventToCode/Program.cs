using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            //only to visualize a progress spinner
            List<string> progress = new List<string>(["/", "-", "\\", "|"]);
            int progressIndex = 0;

            var progressUpdate = new System.Timers.Timer(100);
            progressUpdate.AutoReset = true;
            progressUpdate.Elapsed += (sender, e) => {
                Console.CursorLeft = 0;
                Console.Write(progress[progressIndex % 4]);
                progressIndex++;
            };
            progressUpdate.Start();

            //only to measure time elapsed
            var timer = Stopwatch.StartNew();
            timer.Start();

            //example string
            string example = """
                162,817,812
                57,618,57
                906,360,560
                592,479,940
                352,342,300
                466,668,158
                542,29,236
                431,825,988
                739,650,466
                52,470,668
                216,146,977
                819,987,18
                117,168,530
                805,96,715
                346,949,466
                970,615,88
                941,993,340
                862,61,35
                984,92,344
                425,690,689
                """;

            //to use the example string enable this line and comment the line below
            //IEnumerable<string> array = example.Split("\r\n"); int connectionCount = 10;
            IEnumerable<string> array = File.ReadLines(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "input.txt")); int connectionCount = 1000;

            List<List<int>> dimensions = array.Select(line => line.Split(",").Select(num => int.Parse(num)).ToList()).ToList();

            //create all possible distinct combinations of points
            List<List<List<int>>> connections = dimensions.SelectMany((a, i) => dimensions.Skip(i + 1).Select(b => new List<List<int>>([a.ToList(), b.ToList()]))).OrderBy(pair => Math.Sqrt(Math.Pow(pair[0][0] - pair[1][0], 2) + Math.Pow(pair[0][1] - pair[1][1], 2) + Math.Pow(pair[0][2] - pair[1][2], 2))).ToList();

            //create one circuit for each point in dimensions
            Dictionary<int, List<List<int>>> circuits = dimensions.Select((pos, i) => new { i, pos }).ToDictionary(x => x.i, x => new List<List<int>>([x.pos]));

            //do it for each connection but only until all connections are in the same circuit
            for (int i = 0; i < connectionCount; i++)
            {
                //get the next connection
                List<List<int>> connection = connections[i];

                //determine the circuit key from the first point
                int circuitsId1 = circuits.First(kvp => kvp.Value.Any(inner => inner.SequenceEqual(connection[0]))).Key;

                //determine the circuit key from the second point
                int circuitsId2 = circuits.First(kvp => kvp.Value.Any(inner => inner.SequenceEqual(connection[1]))).Key;

                //if the keys are the same, the points are already connected in the circuit, if not then merge the circuits and remove one
                if (circuitsId1 != circuitsId2)
                {
                    circuits[circuitsId1].AddRange(circuits[circuitsId2]);
                    circuits.Remove(circuitsId2);
                }
            }

            //get the 3 circuits with the most points and get the count of each list as a list
            List<int> top3 = circuits.Values.Select(c => c.Count).OrderByDescending(x => x).Take(3).ToList();

            //sum the counts of the top 3 circuits
            int result = 1;
            foreach (int count in top3)
            {
                result *= count;
            }

            //only for measuring time elapsed
            timer.Stop();
            float timeElapsed = (float)timer.ElapsedMilliseconds / 1000 < 0.9 ? timer.ElapsedMilliseconds : (float)timer.ElapsedMilliseconds / 1000;
            string unit = (float)timer.ElapsedMilliseconds / 1000 < 0.9 ? "milliseconds" : "seconds";
            
            Console.CursorLeft = 0;
            Console.WriteLine($"The size of the three largest circuits multiplied is {result}!\nThe program took {timeElapsed} {unit} to process the input!");
        }
    }
}