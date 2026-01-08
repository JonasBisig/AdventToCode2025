using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

            List<string> connections = new List<string>();
            List<List<int>> circuits = new List<List<int>>();

            //to for ether 10 shortest connection or 1000 shortest connections -> example or real input
            for (int z = 0; z < connectionCount; z++)
            {
                double minDistance = double.MaxValue;
                int index1 = -1, index2 = -1;

                //go through all points and find the closest two points that are not yet connected
                for (int i = 0; i < dimensions.Count; i++)
                {
                    for (int x = 1; x < dimensions.Count; x++)
                    {
                        //if the points are the same -> skip
                        if (i == x) continue;

                        //do the calculation with the formula from the task (Euclidean distance)
                        double dx = dimensions[i][0] - dimensions[x][0];
                        double dy = dimensions[i][1] - dimensions[x][1];
                        double dz = dimensions[i][2] - dimensions[x][2];
                        double distance = Math.Sqrt(dx * dx + dy * dy + dz * dz);

                        //determine if this is the shortest distance found so far and if these points are not yet connected (contained in connections)
                        if (distance < minDistance)
                        {
                            if(!connections.Contains(string.Join(",", i, x)) && !connections.Contains(string.Join(",", x, i)))
                            {
                                minDistance = distance;
                                index1 = i;
                                index2 = x;
                            }
                        }
                    }
                }

                //add the connection and get the index of each point in the circuits list
                if (index1 is not -1 && index2 is not -1 && minDistance is not double.MaxValue)
                {
                    connections.Add(string.Join(",", index1, index2));
                    int i1 = circuits.FindIndex(x => x.Contains(index1));
                    int i2 = circuits.FindIndex(x => x.Contains(index2));

                    //if both points were found in different circuits -> merge them
                    if (i1 != -1 && i2 != -1 && i1 != i2)
                    {
                        //Merge circuits
                        circuits[i1] = circuits[i1].Union(circuits[i2]).ToList();
                        circuits.RemoveAt(i2);
                    }
                    //if only one point was found -> add the other point to that cirquit but only if not already contained
                    else if (i1 != -1)
                    {
                        if(!circuits[i1].Contains(index2)) circuits[i1].Add(index2);
                    }
                    //if only one point was found -> add the other point to that cirquit but only if not already contained
                    else if (i2 != -1)
                    {
                        if (!circuits[i2].Contains(index1)) circuits[i2].Add(index1);
                    }
                    //if no index is found -> create a new cirquit with both points
                    else
                    {
                        circuits.Add(new List<int>([index1, index2]));
                    }
                }
            }

            //order circuits by cirquit size descending -> 5,4,3,2,1
            circuits = circuits.OrderByDescending(list => list.Count).ToList();

            //multiply the sizes of the three largest circuits
            long result = circuits[0].Count * circuits[1].Count * circuits[2].Count;

            //only for measuring time elapsed
            timer.Stop();
            float timeElapsed = (float)timer.ElapsedMilliseconds / 1000 < 0.9 ? timer.ElapsedMilliseconds : (float)timer.ElapsedMilliseconds / 1000;
            string unit = (float)timer.ElapsedMilliseconds / 1000 < 0.9 ? "milliseconds" : "seconds";
            Console.WriteLine($"The size of the three largest circuits multiplied is {result}!\nThe program took {timeElapsed} {unit} to process the input!");
        }
    }
}