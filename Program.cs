using System;
using System.IO;
using System.Xml.Serialization;
using System.Net;

namespace Atlas
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");

            Console.Read();
        }

        public static void Write(string s, ConsoleColor color)
        {
            ConsoleColor start_color = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(s);
            Console.ForegroundColor = start_color;
        }
    }
}
