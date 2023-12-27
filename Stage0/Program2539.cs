using System;
namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome2539();
            Welcome0644();

        }
        private static partial void Welcome0644();
        private static void Welcome2539()
        {
            Console.WriteLine("Enter your name");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, Welcome to my first console application",userName);
        }
    }
}
