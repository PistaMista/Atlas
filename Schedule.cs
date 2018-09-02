using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Atlas
{
    public static class Schedule
    {
        public struct Subject
        {
            public string name;
            public string short_name;
            public ConsoleColor color;
            public int group_size;
            public static void Add()
            {
                Subject s = new Subject();
                Console.Clear();
                Console.WriteLine("Name: ");
                s.name = Console.ReadLine();
                Console.WriteLine("Abbreviation: ");
                s.short_name = Console.ReadLine();
                Console.WriteLine("Class size: ");
                s.group_size = Query.NumberRange(1, 40);

                ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));

                Tuple<string, string, ConsoleColor>[] options = new Tuple<string, string, ConsoleColor>[colors.Length];
                for (int i = 0; i < colors.Length; i++)
                {
                    options[i] = new Tuple<string, string, ConsoleColor>(i.ToString(), colors[i].ToString(), colors[i]);
                }

                Console.WriteLine("Color: ");
                s.color = Query.ValueSelector(options);

                subject_palette = subject_palette.Append(s).ToArray();
                Save();
            }

            public static void Remove()
            {
                if (subject_palette.Length == 0)
                {
                    Console.WriteLine("No subjects to remove.");
                    Console.Read();
                    return;
                }

                Tuple<string, string, int>[] options = new Tuple<string, string, int>[subject_palette.Length];
                for (int i = 0; i < options.Length; i++)
                {
                    Subject s = subject_palette[i];
                    options[i] = new Tuple<string, string, int>(s.short_name, s.name, i);
                }
                Console.Clear();
                Console.WriteLine("Remove a subject: ");

                List<Subject> list = subject_palette.ToList();
                list.RemoveAt(Query.ValueSelector(options));
                subject_palette = list.ToArray();

                Save();
            }
        }

        static Subject[] subject_palette;
        static Subject[][] schedule;
        public static void Load()
        {
            Console.WriteLine("Loading schedule and subjects.");
            XmlSerializer serializer = new XmlSerializer(typeof(Subject[][]));
            FileStream stream = File.Open("data/schedule.txt", FileMode.OpenOrCreate);
            try
            {
                Subject[][] data = (Subject[][])serializer.Deserialize(stream);
                subject_palette = data.First();
                schedule = data.Skip(1).ToArray();
            }
            catch
            {
                subject_palette = new Subject[0];
                schedule = new Subject[7][];

                for (int i = 0; i < schedule.Length; i++)
                {
                    schedule[i] = new Subject[0];
                }
            }
            stream.Dispose();
            stream.Close();
            Save();
        }
        public static void LaunchGUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Schedule and subject manager.");
                DisplaySchedule();
                bool exit = false;
                Query.Dialog
                (
                    new Tuple<string, string, Action>[]
                    {
                        new Tuple<string, string, Action>("rewrite_schedule", "Rewrite the schedule.", RewriteSchedule),
                        new Tuple<string, string, Action>("addsub", "Adds a subject to the palette.", Subject.Add),
                        new Tuple<string, string, Action>("remsub", "Remove a subject from the palette.", Subject.Remove),
                        new Tuple<string, string, Action>("exit", "", () => { exit = true; })
                    }
                );
                if (exit) break;
            }
        }
        public static void DisplaySchedule()
        {
            for (int day_index = 0; day_index < schedule.Length; day_index++)
            {
                Console.Write(((DayOfWeek)day_index).ToString() + ": ");
                Subject[] day = schedule[day_index];

                for (int subject_index = 0; subject_index < day.Length; subject_index++)
                {
                    Subject subject = day[subject_index];
                    Program.Write(" " + subject.short_name, subject.color);
                }

                Console.WriteLine();
            }
        }

        static void RewriteSchedule()
        {
            Tuple<string, string, Subject>[] options = new Tuple<string, string, Subject>[subject_palette.Length];
            for (int i = 0; i < options.Length; i++)
            {
                Subject s = subject_palette[i];
                options[i] = new Tuple<string, string, Subject>(s.short_name, s.name, s);
            }

            schedule = new Subject[7][];
            for (int day_index = 0; day_index < schedule.Length; day_index++)
            {
                DayOfWeek day = (DayOfWeek)day_index;

                Console.Clear();
                Console.WriteLine("How many subjects on " + day.ToString() + "?");
                Subject[] subjects = new Subject[Query.NumberRange(0, 10)];

                for (int i = 0; i < subjects.Length; i++)
                {
                    Console.Clear();
                    Console.WriteLine("Subject #" + i + " on " + day.ToString() + ": ");
                    subjects[i] = Query.ValueSelector(options);
                }

                schedule[day_index] = subjects;
            }

            Save();
        }

        static void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Subject[][]));

            FileStream stream = File.Open("data/schedule.txt", FileMode.Create);
            Subject[][] data = schedule.Prepend(subject_palette).ToArray();
            serializer.Serialize(stream, data);

            stream.Dispose();
            stream.Close();
        }
    }
}