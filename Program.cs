using System;
using System.IO;
using System.Xml.Serialization;
using System.Net;

namespace Atlas
{
    public class Program
    {
        struct Day
        {
            public Day(DateTime date)
            {
                this.date = date;
            }
            DateTime date;
        }
        static void Main(string[] args)
        {
            Console.Title = "Atlas";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Hello!");
                Query.Dialog(
                    new Tuple<string, string, Action>[]
                    {
                        new Tuple<string, string, Action>("calendar", "Opens the timeline calendar view.", Calendar.Launch),
                        new Tuple<string, string, Action>("tasks", "Shows all days with scheduled tasks.", () => { })
                    }
                );
            }
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
