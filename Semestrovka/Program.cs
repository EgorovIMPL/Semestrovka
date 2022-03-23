using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using System.Runtime.Intrinsics;

namespace Semestrovka
{
    class Program
    {
        static void Main()
        {
            
            int n = 1;
            Point[] array = UniquePoints(Generation.ArraySetPoints(n));
            PointPrint(array);
            PointPrint(Greham.ConvexHull(array));
            Point[] array1 = UniquePoints(Generation.ArraySetPoints(n));
            PointPrint(Chan.ConvexHull(array1));
        }

        public static void Print(string[][] str)
        {
            foreach (var el in str)
            {
                Console.WriteLine(String.Join(" ",el));
            }
        }

        public static void PointPrint(Point[] array)
        {
            Console.WriteLine();
            foreach (var el in array)
            {
                Console.Write(el.ToString());
            }
            Console.WriteLine();
        }

        public static Point[] UniquePoints(Point[] array)
        {
            Couple[] library = new Couple[0];
            for (int i = 0; i < array.Length; i++)
            {
                int count = 0;
                foreach (var el in library)
                    if (array[i].X == el.Inf.X && array[i].Y == el.Inf.Y)
                    {
                        el.Count += 1;
                        count += 1;
                        break;
                    }
                if (count == 0)
                    library = library.Append(new Couple(array[i])).ToArray();
            }

            Point[] result = new Point[0];
            foreach (var couple in library)
            {
                result = result.Append(couple.Inf).ToArray();
            }

            return result;
        }
    }
    public class Couple
    {
        public Point Inf;
        public int Count;
        public Couple(Point inf)
        {
            Inf = inf;
            Count = 1;
        }
    }

}
