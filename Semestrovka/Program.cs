using System.Diagnostics.CodeAnalysis;

namespace Semestrovka
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("I want chicken burger");
            Mama.Sum();
        }
    }

    public static class Mama
    {
        public static void Sum()
        {
            Console.WriteLine("NO.");
        }
    }
}
