using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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

            List<(long X, long Y)> polygon = array.Select(a => (X: long.Parse(a.Split(",")[0]), Y: long.Parse(a.Split(",")[1]))).ToList();

            var stopwatch = Stopwatch.StartNew();

            long p1 = 0, p2 = 0;

            List<((long X, long Y) a, (long X, long Y) b)> combinations = polygon.SelectMany((a, i) => polygon.Skip(i + 1).Select(b => (a, b))).ToList();

            long debug = 0;

            foreach (var comb in combinations)
            {
                Console.CursorLeft = 0;
                Console.Write($"{debug}/{combinations.Count}                                               ");
                debug++;
                long f = Flaeche(comb.a.X, comb.a.Y, comb.b.X, comb.b.Y);
                if (f < p2) continue;

                bool isInPolygon = false;

                for (long i = Math.Min(comb.a.X, comb.b.X); i <= Math.Max(comb.a.X, comb.b.X); i++)
                {
                    for (long j = Math.Min(comb.a.Y, comb.b.Y); j <= Math.Max(comb.a.Y, comb.b.Y); j++)
                    {
                        isInPolygon = IsPointInPolygon(polygon, (i, j));

                        if (!isInPolygon) break;
                    }
                    if (!isInPolygon) break;
                }

                if (isInPolygon) p2 = Math.Max(p2, f);
            }

            Console.WriteLine("Lösung: " + p2);
        }
        private static bool IsPointInPolygon(List<(long X, long Y)> polygon, (long X, long Y) pointToCheck)
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
                double dot = 0; // (pointToCheck.X - point.X) / (nextPoint.X - point.X);

                //if they are kolinear on the Y axis then dot will be NaN, so check with Y axis
                if (nextPoint.X - point.X is 0)
                {
                    dot = (pointToCheck.Y - point.Y) / (nextPoint.Y - point.Y);
                }
                else
                {
                    dot = (pointToCheck.X - point.X) / (nextPoint.X - point.X);
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

                try
                {
                    double Xs = ((nextPoint.X - point.X) * (pointToCheck.Y - point.Y) / (nextPoint.Y - point.Y)) + point.X;
                    if (Xs > pointToCheck.X) intersect++;
                }catch (DivideByZeroException e)
                {

                }
            }
            insidePolygon = intersect % 2 != 0;
            return insidePolygon;
        }
        static long Flaeche(long x1, long y1, long x2, long y2)
        {
            return (Math.Abs(x1 - x2) + 1) * (Math.Abs(y1 - y2) + 1);
        }
    }
}

//    foreach (var ((x1, y1), (x2, y2)) in combinations)
//    {
//        long f = Flaeche(x1, y1, x2, y2);
//        p1 = Math.Max(p1, f);

//        //if (f < p2) continue;

//        bool isInPolygon = false;

//        for (long i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++)
//        {
//            for (long j = Math.Min(y1, y2); j <= Math.Max(y1, y2); j++)
//            {
//                isInPolygon = PointInPolygon(i, j, polygon);

//                if (!isInPolygon) break;
//            }
//            if (!isInPolygon) break;
//        }

//        if (isInPolygon) p2 = Math.Max(p2, f);
//    }

//    stopwatch.Stop();

//    Console.WriteLine($"Solution: ({p1}, {p2})");
//    Console.WriteLine($"Solved in {stopwatch.Elapsed.TotalSeconds:F5} Sec.");

//    long result = p2;

//    Console.WriteLine($"The largest area of any rectangle which contains only red and green tiles is { result } tiles big!");
//}
//static long Flaeche(long x1, long y1, long x2, long y2)
//{
//    return (Math.Abs(x1 - x2) + 1) * (Math.Abs(y1 - y2) + 1);
//}

//static bool PointInPolygon(long x, long y, List<(long x, long y)> polygon)
//{
//    bool inside = false;
//    int j = polygon.Count - 1;

//    for (int i = 0; i < polygon.Count; i++)
//    {
//        if (IsPointOnSegment(polygon[j], polygon[i], x, y)) return true;

//        if (polygon[i].y == y && polygon[i].x == x) return true; 
//        if ((polygon[i].y > y) != (polygon[j].y > y) &&
//            x < (polygon[j].x - polygon[i].x) * (y - polygon[i].y) /
//                (polygon[j].y - polygon[i].y) + polygon[i].x)
//        {
//            inside = !inside;
//        }
//        j = i;
//    }
//    return inside;
//}

//static bool IsPointOnSegment((long x, long y) a, (long x, long y) b, long px, long py)
//{
//    // Kreuzprodukt ~ 0 => kollinear
//    long cross = (py - a.y) * (b.x - a.x) - (px - a.x) * (b.y - a.y);

//    if (px == 3 && py == 5)
//    {

//    }
//    if (cross == 0)
//    {
//        double dot = (px - a.x) / (b.x - a.x);

//        //if they are kolinear on the Y axis then dot will be NaN, so check with Y axis
//        if (double.IsNaN(dot))
//        {
//            dot = (py - a.y) / (b.x - a.y);
//        }
//        if (dot is >= 0 and <= 1) return false;
//    }

//    // innerhalb der Bounding-Box (mit Toleranz)
//    long minX = Math.Min(a.x, b.x), maxX = Math.Max(a.x, b.x);
//    long minY = Math.Min(a.y, b.y), maxY = Math.Max(a.y, b.y);
//    return px >= minX && px <= maxX && py >= minY && py <= maxY;
//}

//public static bool PointInPolygon2(long x, long y, List<(long x, long y)> polygon)
//{
//    if (polygon == null || polygon.Count < 3) return false;

//    // 1) Prüfe, ob Punkt auf einer Kante liegt (inkl. Scheitelpunkte)
//    for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i, i++)
//    {
//        if (IsPointOnSegment(polygon[j], polygon[i], x, y)) return true;
//    }

//    // 2) Ray-casting (Winkelzählung) für Punkt-in-Polygon
//    bool inside = false;
//    for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i, i++)
//    {
//        long yi = polygon[i].y, yj = polygon[j].y;
//        // Prüfen ob Kante die horizontale Halbgerade schneidet
//        if ((yi > y) != (yj > y))
//        {
//            // X-Koordinate der Schnittstelle berechnen
//            long intersectX = polygon[i].x + (y - yi) * (polygon[j].x - polygon[i].x) / (yj - yi);
//            if (x < intersectX)
//                inside = !inside;
//        }
//    }
//    return inside;
//}
//static bool RectangleInPolygon(long x1, long y1, long x2, long y2, List<(long x, long y)> polygon)
//{
//    // Prüfe alle 4 Ecken des Rechtecks
//    long minX = Math.Min(x1, x2);
//    long maxX = Math.Max(x1, x2);
//    long minY = Math.Min(y1, y2);
//    long maxY = Math.Max(y1, y2);

//    return PointInPolygon(minX, minY, polygon) &&
//           PointInPolygon(maxX, minY, polygon) &&
//           PointInPolygon(maxX, maxY, polygon) &&
//           PointInPolygon(minX, maxY, polygon);
//}
//    }
//}