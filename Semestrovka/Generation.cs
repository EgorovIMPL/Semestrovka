using System.Drawing;
using System.Text;

namespace Semestrovka;

public class Generation
{
    public static void Start()
    {
        string[] Data = new string[new Random().Next(50, 100)];
        for (int i = 0; i < Data.Length; i++)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new Random().Next(1, 50) + " " + new Random().Next(1, 50));
            for (int g = 0; g < new Random().Next(100, 1000); g++)
            {
                sb.Append("," + new Random().Next(1, 50) + " " + new Random().Next(1, 50));
            }
            Data[i] = sb.ToString();
        }
        File.WriteAllLines(@"C:\SomeDir2\Semestrovka.txt", Data);
    }

    public static string[][] FileReading(string path)
    {
        string[] file = File.ReadAllLines(path);
        string[][] result = new string[file.Length][];
        for (int i = 0; i < file.Length; i++)
            result[i] = file[i].Split(",");
        return result;
    }

    public static Point[] ArraySetPoints(int stringNumber)
    {
        string[][] str = FileReading(@"C:\SomeDir2\Semestrovka.txt");
        if (stringNumber >= str.Length)
            throw new Exception("Выход за массив");
        Point[] result = new Point[str[stringNumber-1].Length];
        for (int i = 0; i < str[stringNumber-1].Length; i++)
        {
            int[] s = Array.ConvertAll(str[stringNumber-1][i].Split(" "),Int32.Parse);
            result[i] = new Point(s[0],s[1]);
        }
        return result;
    }
}