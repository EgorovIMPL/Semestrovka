using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;

namespace Semestrovka
{
    class Program
    {
        static void Main()
        {
            string[] data = new string[78];
            for (int i = 0; i < 78; i++)
            {
                var time1 = new Stopwatch();
                StringBuilder sb = new StringBuilder();
                time1.Start();
                Point[] array1 = Tools.UniquePoints(Generation.ArraySetPoints(i+1));
                int count = array1.Length;
                ConvexHull.Chan(array1);
                time1.Stop();
                sb.Append(count + " " + time1.Elapsed.TotalSeconds + " " + Counter.Count);
                Counter.Count = 0;
                data[i] = sb.ToString();
            }
            File.WriteAllLines(@"C:\SomeDir2\result.txt",data);
        }
    }
}
