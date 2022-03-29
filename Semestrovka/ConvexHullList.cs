using System.Drawing;

namespace Semestrovka;

public class ConvexHullList
{
    public List<Point> Chan(List<Point> points)
    {
        return ConvexHull.Chan(points.ToArray()).ToList();
    }
}