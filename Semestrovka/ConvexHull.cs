using System.Collections;
using System.Drawing;

namespace Semestrovka;

public class ConvexHull
{
    public static Point[] Chan(Point[] points)
    {
        int m = 1;
        for (int i = 2; i < points.Length; i++)
        {
            Counter.Count += 1;
            if (points.Length % i == 0)
            {
                m = i;
                break;
            }
        }
        int n = points.Length / m;
        Point[][] data = new Point[0][];
        for (int i = 0; i < m; i++)
        {
            data = data.Append(Greham(points[(i * n)..((i + 1) * n)])).ToArray();
            Counter.Count += 1;
        }
        if (data.Length == 1)
            return data[0];
        Point[] sr = new Point[0];
        foreach (var el in data)
        {
            Counter.Count += 1;
            sr = sr.Append(el[0]).ToArray();
        }
        sr = new Point[]{Tools.LeftPoint(sr)};
        Point[] newPoints = new Point[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            Counter.Count += 1;
            Point[] newArray = data[i];
            newArray = Tools.DelElem(points,sr[0]);
            newArray = Tools.PlusOrMinusToAll(points, sr[0], false);
            Array.Sort(newArray, new PointComparer());
            newArray = Tools.PlusOrMinusToAll(points, sr[0], true);
            newPoints[i] = newArray[0];
        }
        newPoints = Tools.PlusOrMinusToAll(newPoints, sr[0],false);
        Array.Sort(newPoints, new PointComparer());
        newPoints = Tools.PlusOrMinusToAll(newPoints, sr[0],true);
        sr = sr.Append(newPoints[0]).ToArray();
        data = Tools.DelElem(data, sr[1]);
        int count = 1;
        while (true)
        {
            newPoints = new Point[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                Counter.Count += 1;
                newPoints[i] = Tools.Min_Cos(Tools.AddToBeginning(sr[count - 1], sr[count], data[i]));
            }
            Point sr1 = Tools.Min_Cos(Tools.AddToBeginning(sr[count - 1], sr[count], newPoints));
            if (sr1.X == sr[0].X && sr1.Y == sr[0].Y)
                break;
            sr = sr.Append(sr1).ToArray();
            count += 1;
            data = Tools.DelElem(data, sr[count]);
        }
        return sr;
    }
    public static Point[] Greham(Point[] points)
    {
        if (points.Length <= 1)
            return points;
        Point left = Tools.LeftPoint(points);
        points = Tools.DelElem(points,left);
        points = Tools.PlusOrMinusToAll(points, left, false);
        Array.Sort(points, new PointComparer());
        points = Tools.PlusOrMinusToAll(points, left, true);
        points = Tools.AddToBeginning(left, points);
        Point[] buble = new Point[0];
        int count = -1;
        for (int  i = 0;  i < points.Length; i++)
        {
            buble = buble.Append(points[i]).ToArray();
            count+=1;
            while (count >= 2 && Tools.Rotate(buble[count - 2], buble[count - 1], buble[count]))
            {
                Counter.Count += 1;
                count-=1;
                buble = buble[0..count];
                buble = buble.Append(points[i]).ToArray();
            }
        }
        return buble;
    }
    
}
public class Tools
{
    public static Point LeftPoint(Point[] points)
    {
        Point left = points[0];
        for (int i = 1; i < points.Length; i++)
        {
            Counter.Count += 1;
            if (points[i].X < left.X)
                left = points[i];
        }
        return left;
    }

    public static Point[] PlusOrMinusToAll(Point[] points,Point a,bool f)
    {
        if (f)
        {
            for (int i = 0; i < points.Length; i++)
            {
                Counter.Count += 1;
                points[i].X += a.X;
                points[i].Y += a.Y;
            }
        }
        else
        {
            for (int i = 0; i < points.Length; i++)
            {
                Counter.Count += 1;
                points[i].X -= a.X;
                points[i].Y -= a.Y;
            }
        }

        return points;
    }
    public static Point[] DelElem(Point[] array, Point a)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].X == a.X && array[i].Y == a.Y)
            {
                Point[] newArray = new Point[array.Length - 1];
                int count = 0;
                foreach (var point in array)
                {
                    Counter.Count += 1;
                    if (point.X == a.X && point.Y == a.Y)
                    {
                    }
                    else
                    {
                        newArray[count] = point;
                        count += 1;
                    }
                }

                array = newArray;
                return array;
            }
        }

        return array;
    }

    public static bool Rotate(Point a,Point b,Point c)
    {
        return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X) <= 0;
    }
    public static Point Min_Cos(Point[] array)
    {
        double minCos = 2;
        if (array.Length == 2)
            return array[1];
        Point point = Point.Empty;
        for (int i = 2; i < array.Length; i++)
        {
            Counter.Count += 1;
            double cos =
                ((array[0].X - array[1].X) * (array[i].X - array[1].X) +
                 (array[0].Y - array[1].Y) * (array[i].Y - array[1].Y)) /
                (Math.Sqrt((array[0].X - array[1].X) * (array[0].X - array[1].X) +
                            (array[0].Y - array[1].Y) * (array[0].Y - array[1].Y)) * Math.Sqrt(
                    (array[i].X - array[1].X) * (array[i].X - array[1].X) +
                    (array[i].Y - array[1].Y) * (array[i].Y - array[1].Y)));
            if (cos < minCos)
            {
                minCos = cos;
                point = array[i];
            }
        }

        return point;
    }
    public static Point[][] DelElem(Point[][] array,Point a)
    {
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = 0; j < array[i].Length; j++)
            {
                if (array[i][j].X == a.X && array[i][j].Y == a.Y)
                {
                    Point[] newArray = new Point[array[i].Length - 1];
                    int count = 0;
                    foreach (var point in array[i])
                    {
                        Counter.Count += 1;
                        if (point.X == a.X && point.Y == a.Y)
                        {}
                        else
                        {
                            newArray[count] = point;
                            count += 1;
                        }
                    }

                    array[i] = newArray;
                    return array;
                }
            }
        }
        return array;
    }
    public static Point[] AddToBeginning(Point a, Point[] array)
    {
        Point[] newArray = new Point[array.Length+1];
        newArray[0] = a;
        for (int i = 0; i < array.Length; i++)
        {
            Counter.Count += 1;
            newArray[i + 1] = array[i];
        }
        return newArray;
    }
    public static Point[] AddToBeginning(Point a,Point b, Point[] array)
    {
        Point[] newArray = new Point[array.Length+2];
        newArray[0] = a;
        newArray[1] = b;
        for (int i = 0; i < array.Length; i++)
        {
            Counter.Count += 1;
            newArray[i + 2] = array[i];
        }
        return newArray;
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


public class PointComparer : IComparer
{
    public int Compare(object x, object y)
    {
        var point1 = (Point) x;
        var point2 = (Point) y;
        return Math.Atan2(point1.Y, point1.X).CompareTo(Math.Atan2(point2.Y, point2.X));
    }
}
public class Counter
{
    public static int Count;
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
