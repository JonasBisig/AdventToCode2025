using System;
using System.Collections.Generic;
using System.Drawing;
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
            string example = """
                7,1                
                11,1
                11,7
                9,7
                9,5
                2,5
                2,3
                7,3
                """;

            //to use the example string enable this line and comment the line below
            //IEnumerable<string> array = example.Split("\r\n");
            IEnumerable<string> array = File.ReadAllLines(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "input.txt"));

            //save the polygon points
            List<(double X, double Y)> polygon = array.Select(a => (X: double.Parse(a.Split(",")[0]), Y: double.Parse(a.Split(",")[1]))).ToList();

            List<(double X, double Y)> debug = polygon.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            //get every combination of positions and order them by areasize descending -> largest area at pos 0
            List<Tuple<(double X, double Y), (double X, double Y)>> areaPoints = array.SelectMany((a, i) => array.Skip(i + 1).Select(b => ((double.Parse(a.Split(",")[0]), double.Parse(a.Split(",")[1])), (double.Parse(b.Split(",")[0]), double.Parse(b.Split(",")[1]))).ToTuple()))
                .OrderByDescending(x => (Math.Abs(x.Item1.Item1 - x.Item2.Item1) + 1) * (Math.Abs(x.Item1.Item2 - x.Item2.Item2) + 1))
                .ToList();

            Tuple<(double X, double Y), (double X, double Y)> largestValidPoints = new Tuple<(double X, double Y), (double X, double Y)>((0, 0), (0, 0));

            for (int i = 0; i < areaPoints.Count; i++)
            {
                (double X, double Y) invertedPointA = (areaPoints[i].Item2.X, areaPoints[i].Item1.Y);
                (double X, double Y) invertedPointB = (areaPoints[i].Item1.X, areaPoints[i].Item2.Y);

                largestValidPoints = areaPoints[i];

                if (IsPointInPolygon(polygon, invertedPointA) && IsPointInPolygon(polygon, invertedPointB)) break;
            }

            double result = (Math.Abs(largestValidPoints.Item1.X - largestValidPoints.Item2.X) + 1) * (Math.Abs(largestValidPoints.Item1.Y - largestValidPoints.Item2.Y) + 1);

            Console.WriteLine($"The largest area of any rectangle which contains only red and green tiles is { result } tiles big!");
        }

        private static bool IsPointInPolygon(List<(double X, double Y)> polygon, (double X, double Y) pointToCheck)
        {
            bool insidePolygon = false;
            //check if the pointToCheck is on the border of the polygon
            for (int i = 0; i < polygon.Count; i++)
            {
                var point = polygon[i];
                var nextPoint = polygon[(i + 1) % polygon.Count];

                //check if the pointToCheck is on the same line as point and nextPoint -> collinear
                //calculate the area of the triangle made by those 3 points
                //(Ax, Ay) | (Bx, By) | (Cx, Cy) --> A = point, B = nextPoint, C = pointToCheck
                //
                //(Bx - Ax) * (Cy - Ay) - (By - Ay) * (Cx - Ax) = area of the triangle

                double cross = (nextPoint.X - point.X) * (pointToCheck.Y - point.Y) - (nextPoint.Y - point.Y) * (pointToCheck.X - point.X);

                //if the area is 0, the points are collinear
                insidePolygon = cross == 0;
                if (!insidePolygon) continue;

                //check if the collinear pointToCheck is between the other points
                //vectorfactor k = (Cx - Ax) / (Bx - Ax) -> if 0 <= k <= 1, then the pointToCheck is between point and nextPoint -> <= because if the pointToCheck is equal to point or nextPoint, k will be 0 or 1
                double dot = (pointToCheck.X - point.X) / (nextPoint.X - point.X);

                //if they are kolinear on the Y axis then dot will be NaN, so check with Y axis
                if (double.IsNaN(dot))
                {
                    dot = (pointToCheck.Y - point.Y) / (nextPoint.Y - point.Y);
                }

                insidePolygon = dot is >= 0 and <= 1;
                if (insidePolygon) break;
            }

            //if it is on the border then the point is inside the polygon
            if (insidePolygon) return insidePolygon;

            int intersect = 0;
            //check if the pointToCheck is inside the polygon
            for (int i = 0; i < polygon.Count; i++)
            {
                var point = polygon[i];
                var nextPoint = polygon[(i + 1) % polygon.Count];

                //check if pointToCheck is vertically between point and nextPoint
                insidePolygon = Math.Min(point.Y, nextPoint.Y) <= pointToCheck.Y && pointToCheck.Y <= Math.Max(point.Y, nextPoint.Y);
                if (!insidePolygon) continue;

                //ray-casting: check if the pointToCheck cuts the line between point and nextPoint if the pointToCheck is extended to the right infinitely
                //if the line intersect an odd number then the point is inside the polygon if even then outside
                //the formula is: (Ax, Ay) | (Bx, By) | (Cx, Cy) --> A = point, B = nextPoint, C = pointToCheck
                //
                //((Bx - Ax) * (Cy - Ay) / (By - Ay)) + Ax = Xs -> if Xs > Cx then the line intersects

                double Xs = ((nextPoint.X - point.X) * (pointToCheck.Y - point.Y) / (nextPoint.Y - point.Y)) + point.X;
                if (Xs > pointToCheck.X) intersect++;
            }
            insidePolygon = intersect % 2 != 0;
            return insidePolygon;
        }
    }
}