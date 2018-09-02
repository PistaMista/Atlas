using System;
using System.Linq;

namespace Atlas
{
    static class Calendar
    {
        enum Layer
        {
            YEAR,
            MONTH,
            DAY
        }
        static Layer browsing;
        static DateTime[] options;
        static DateTime date;
        public static void LaunchGUI()
        {
            date = DateTime.Today.Date;
            browsing = Layer.DAY;
            RefreshOptions();
            UpdateDisplay();

            while (true)
            {
                int vertical;
                int horizontal;
                while (!Program.ReadArrowKeys(out vertical, out horizontal)) Console.Beep();
                Tuple<int, int, int> position = date.GetPositionData();

                if (vertical != 0)
                {
                    int new_position = position.Item2 - vertical;
                    if (new_position >= position.Item1 && new_position <= position.Item3) date = date.SetPosition(new_position);
                }
                else
                {
                    int new_layer = (int)browsing + horizontal;
                    if (new_layer > 2)
                    {
                        Task.ViewDay(date);
                        break;
                    }
                    else if (new_layer < 0)
                    {
                        break;
                    }
                    else
                    {
                        browsing = (Layer)new_layer;
                        RefreshOptions();
                    }
                }

                UpdateDisplay();
            }
        }
        static void RefreshOptions()
        {
            options = new DateTime[browsing == Layer.YEAR ? 8 : (browsing == Layer.MONTH ? 12 : DateTime.DaysInMonth(date.Year, date.Month))];

            for (int i = 0; i < options.Length; i++)
            {
                switch (browsing)
                {
                    case Layer.YEAR:
                        options[i] = new DateTime(DateTime.Today.Date.Year + i, 1, 1);
                        break;
                    case Layer.MONTH:
                        options[i] = new DateTime(date.Year, i + 1, 1);
                        break;
                    case Layer.DAY:
                        options[i] = new DateTime(date.Year, date.Month, i + 1);
                        break;
                }
            }
        }
        static void UpdateDisplay()
        {
            int space = Console.WindowHeight;
            int currentPosition = browsing == Layer.YEAR ? date.Year - options.First().Year : (browsing == Layer.MONTH ? date.Month - 1 : date.Day - 1);
            Console.Clear();

            for (int i = currentPosition - space / 2; i < currentPosition + space / 2; i++)
            {
                if (i >= 0 && i < options.Length)
                {
                    if (i == currentPosition)
                    {
                        ConsoleColor prevColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(options[i].ToLongDateString());
                        Console.ForegroundColor = prevColor;
                    }
                    else Console.WriteLine(options[i].GetMenuString());
                }
                else
                {
                    Console.WriteLine("");
                }
            }
        }

        static string GetMenuString(this DateTime x)
        {
            switch (browsing)
            {
                case Layer.YEAR:
                    return x.Year.ToString();
                case Layer.MONTH:
                    return x.Month.ToString() + " " + x.Year.ToString();
                case Layer.DAY:
                    return x.Day.ToString() + ". " + x.DayOfWeek.ToString();
            }

            return x.ToLongDateString();
        }

        static Tuple<int, int, int> GetPositionData(this DateTime x)
        {
            return new Tuple<int, int, int>
            (browsing == Layer.YEAR ? options.First().Year : 1,
            browsing == Layer.YEAR ? x.Year : (browsing == Layer.MONTH ? x.Month : x.Day),
            browsing == Layer.YEAR ? options.Last().Year : (browsing == Layer.MONTH ? 12 : DateTime.DaysInMonth(date.Year, date.Month)));
        }

        static DateTime SetPosition(this DateTime x, int position)
        {
            switch (browsing)
            {
                case Layer.YEAR:
                    return new DateTime(position, 1, 1);
                case Layer.MONTH:
                    return new DateTime(x.Year, position, 1);
                case Layer.DAY:
                    return new DateTime(x.Year, x.Month, position);
            }

            return new DateTime();
        }
    }
}