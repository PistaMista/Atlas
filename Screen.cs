using System;

namespace Atlas
{
    static class Screen
    {
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