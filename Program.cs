using System;
using System.IO;
using System.Xml.Serialization;

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
            Schedule.Load();
            while (true)
            {
                Console.Clear();
                Schedule.DisplaySchedule();
                Query.Dialog(
                    new Tuple<string, string, Action>[]
                    {
                        new Tuple<string, string, Action>("calendar", "Opens the timeline calendar view.", Calendar.LaunchGUI),
                        new Tuple<string, string, Action>("manager", "Manage subjects and the schedule.", Schedule.LaunchGUI),
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

        public static bool ReadArrowKeys(out int vertical, out int horizontal)
        {
            horizontal = 0;
            vertical = 0;
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            switch (keyinfo.Key.ToString())
            {
                case "UpArrow":
                    vertical = 1;
                    break;
                case "DownArrow":
                    vertical = -1;
                    break;
                case "RightArrow":
                    horizontal = 1;
                    break;
                case "LeftArrow":
                    horizontal = -1;
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}
