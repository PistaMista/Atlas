using System;
using System.Linq;

namespace Atlas
{
    static class Query
    {
        static int Display<T>(Tuple<string, string, T>[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Program.Write("'" + items[i].Item1 + "' | ", ConsoleColor.Red);
                Program.Write(items[i].Item3.GetType().ToString() + " | ", ConsoleColor.Yellow);
                Program.Write(items[i].Item2, ConsoleColor.Green);
                Console.WriteLine("");
            }

            while (true)
            {
                string input = Console.ReadLine();
                for (int i = 0; i < items.Length; i++)
                {
                    if (input == items[i].Item1) return i;
                }
            }
        }
        public static void Dialog(Tuple<string, string, Action>[] actions)
        {
            actions[Display<Action>(actions)].Item3();
        }

        public static T ValueSelector<T>(Tuple<string, string, T>[] options)
        {
            return options[Display<T>(options)].Item3;
        }


    }
}