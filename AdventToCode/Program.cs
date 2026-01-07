using System;
using System.Collections.Generic;
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
                .......S....... 1  .......S.......
                ............... 1  .......|.......
                .......^....... 2  ......|^|......
                ............... 2  ......|.|......
                ......^.^...... 4  .....|^|^|.....
                ............... 4  .....|.|.|.....
                .....^.^.^..... 8  ....|^|^|^|....
                ............... 8  ....|.|.|.|....
                ....^.^...^.... 13 ...|^|^|||^|...
                ............... 13 ...|.|.|||.|...
                ...^.^...^.^... 20 ..|^|^|||^|^|..
                ............... 20 ..|.|.|||.|.|..
                ..^...^.....^.. 26 .|^|||^||.||^|.
                ............... 26 .|.|||.||.||.|.
                .^.^.^.^.^...^. 40 |^|^|^|^|^|||^|
                ............... 40 |.|.|.|.|.|||.|               
                """;

            example = """
                .......S.......
                ...............
                .......^.......
                ...............
                ......^.^......
                ...............
                .....^.^.^.....
                ...............
                ....^.^...^....
                ...............
                ...^.^...^.^...
                ...............
                ..^...^.....^..
                ...............
                .^.^.^.^.^...^.
                ...............
                """;

            //to use the example string enable this line and comment the line below
            IEnumerable<string> array = example.Split("\r\n");
            //IEnumerable<string> array = File.ReadLines(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "input.txt"));

            List<List<List<char>>> timelines = new List<List<List<char>>>();
            timelines.Add(array.Select(line => line.ToList()).ToList());
            long result = 0;

            int row = 0;

            while (timelines.Count != 0)
            {
                Console.CursorLeft = 0;
                Console.Write($"Fortschritt: {timelines.Count}");
                int length = timelines.Count;
                for (int diags = 0; diags < length; diags++)
                {

                    List<List<char>> diagramm = timelines[0];

                    if (row + 1 >= diagramm.Count)
                    {
                        result = timelines.Count;
                        timelines.RemoveRange(0, timelines.Count);
                        break;
                    }

                    for (int col = 0; col < diagramm[row].Count; col++)
                    {
                        //get the current char and the char below it
                        char currentChar = diagramm[row][col];
                        char nextChar = diagramm[row + 1][col];

                        //if the current char is the start S or the Beam, then check if the beam has to split or not
                        if (currentChar is 'S' or '|')
                        {
                            if (nextChar is '|' || (nextChar == '^' && (diagramm[row + 1][col + 1] == '|' || diagramm[row + 1][col - 1] == '|')))
                            {
                                timelines.RemoveAt(0);
                                continue;
                            }

                            if (nextChar is '^')
                            {
                                List<List<char>> leftDiagramm = diagramm.Select(row => row.ToList()).ToList();
                                List<List<char>> rightDiagramm = diagramm.Select(row => row.ToList()).ToList();
                                rightDiagramm[row + 1][col + 1] = '|';
                                leftDiagramm[row + 1][col - 1] = '|';
                                timelines.Add(leftDiagramm);
                                timelines.Add(rightDiagramm);
                                timelines.RemoveAt(0);

                                //breaks outer loop
                                //row = diagramm.Count;
                                break;
                            }
                            else
                            {
                                diagramm[row + 1][col] = '|';
                                timelines.Add(diagramm);
                                timelines.RemoveAt(0);

                                //breaks outer loop
                                //row = diagramm.Count;
                                break;
                            }
                        }
                    }
                }
                row++;
            }
            Console.WriteLine("\nThe beam has split "+ result + " times!");
        }
    }
}