using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Semestrovka;

public class Chan
{
    public static Point[] ConvexHull(Point[] points)
    {
        int m = 1;
        for (int i = 2; i < points.Length; i++)
        { 
            if (points.Length % i == 0)
            {
                m = i;
                break;
            }
        }
        int n = points.Length / m;
        Point[][] data = new Point[0][];
        for (int i = 0; i < m; i++)
            data = data.Append(Greham.ConvexHull(points[(i * n)..((i + 1) * n)])).ToArray();
        if (data.Length == 1)
            return data[0];
        Point[] sr = new Point[0];
        foreach (var el in data)
        {
            sr = sr.Append(el[0]).ToArray();
        }
        sr = new Point[]{Greham.LeftPoint(sr)};
        Point[] newPoints = new Point[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            Point[] newArray = data[i];
            newArray = Greham.DelElem(points,sr[0]);
            newArray = Greham.PlusOrMinusToAll(points, sr[0], false);
            Array.Sort(newArray, new PointComparer());
            newArray = Greham.PlusOrMinusToAll(points, sr[0], true);
            newPoints[i] = newArray[0];
        }
        newPoints = Greham.PlusOrMinusToAll(newPoints, sr[0],false);
        Array.Sort(newPoints, new PointComparer());
        newPoints = Greham.PlusOrMinusToAll(newPoints, sr[0],true);
        sr = sr.Append(newPoints[0]).ToArray();
        int count = 1;
        while (true)
        {
            newPoints = new Point[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                newPoints[i] = Min_Cos(AddToBeginning(sr[count - 1], sr[count], data[i]));
            }
            Point sr1 = Min_Cos(AddToBeginning(sr[count - 1], sr[count], newPoints));
            if (sr1.X == sr[0].X && sr1.Y == sr[0].Y)
                break;
            sr = sr.Append(sr1).ToArray();
            count += 1;
            data = DeleteElem(data, sr[count]);
        }
        return sr;
    }

    public static Point Min_Cos(Point[] array)
    {
        double minCos = 2;
        if (array.Length == 2)
            return array[1];
        Point point = Point.Empty;
        for (int i = 2; i < array.Length; i++)
        {
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
            else if(cos == minCos && (Math.Sqrt(
                        (array[i].X - array[1].X) * (array[i].X - array[1].X) +
                        (array[i].Y - array[1].Y) * (array[i].Y - array[1].Y)) > Math.Sqrt(
                        (point.X - array[1].X) * (point.X - array[1].X) +
                        (point.Y - array[1].Y) * (point.Y - array[1].Y))))
            {
                minCos = cos;
                point = array[i];
            }
        }

        return point;
    }
    private static Point[][] DeleteElem(Point[][] array,Point a)
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
            newArray[i + 1] = array[i];
        return newArray;
    }
    private static Point[] AddToBeginning(Point a,Point b, Point[] array)
    {
        Point[] newArray = new Point[array.Length+2];
        newArray[0] = a;
        newArray[1] = b;
        for (int i = 0; i < array.Length; i++)
            newArray[i + 2] = array[i];
        return newArray;
    }
    private static Point MaxTan(params Point[] a)
    {
        if (a.Length == 1)
            return a[0];
        double max = Math.Atan2(a[1].X - a[0].X, a[1].Y - a[0].Y);
        Point maxPoint = a[1];
        for (int i = 2; i < a.Length; i++)
        {
            double angle = Math.Atan2(a[i].X - a[0].X, a[i].Y - a[0].Y);
            if (angle > max && (a[i].X != a[0].X && a[i].Y != a[0].Y))
            {
                max = angle;
                maxPoint = a[i];
            }
                
        }
        return maxPoint;
    }
}

public class Greham
{
    public static Point LeftPoint(Point[] points)
    {
        Point left = points[0];
        for (int i = 1; i < points.Length; i++)
        {
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
                points[i].X += a.X;
                points[i].Y += a.Y;
            }
        }
        else
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X -= a.X;
                points[i].Y -= a.Y;
            }
        }

        return points;
    }
    public static Point[] ConvexHull(Point[] points)
    {
        if (points.Length <= 1)
            return points;
        Point left = LeftPoint(points);
        points = DelElem(points,left);
        points = PlusOrMinusToAll(points, left, false);
        Array.Sort(points, new PointComparer());
        points = PlusOrMinusToAll(points, left, true);
        points = Chan.AddToBeginning(left, points);
        Point[] buble = new Point[0];
        int count = -1;
        for (int  i = 0;  i < points.Length; i++)
        {
            buble = buble.Append(points[i]).ToArray();
            count+=1;
            while (count >= 2 && Rotate(buble[count - 2], buble[count - 1], buble[count]))
            {
                count-=1;
                buble = buble[0..count];
                buble = buble.Append(points[i]).ToArray();
            }
        }
        return buble;
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
