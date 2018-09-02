using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Atlas
{
    public static class Schedule
    {
        public struct Subject
        {
            public readonly string name;
            public readonly string short_name;
            public readonly int group_size;
            public static void Add()
            {

                Save();
            }

            public static void Remove()
            {

                Save();
            }
        }

        static Subject[] subject_palette;
        static Subject[][] schedule;
        public static void Load()
        {
            Console.WriteLine("Loading schedule and subjects.");
            XmlSerializer serializer = new XmlSerializer(typeof(Subject[][]));
            FileStream stream = File.Open("data/schedule.txt", FileMode.Open);
            try
            {
                Subject[][] data = (Subject[][])serializer.Deserialize(stream);
                subject_palette = data.First();
                schedule = data.Skip(1).ToArray();
            }
            catch
            {
                subject_palette = new Subject[0];
                schedule = new Subject[8][];

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
                bool exit = false;
                Query.Dialog
                (
                    new Tuple<string, string, Action>[]
                    {
                        new Tuple<string, string, Action>("schedule", "Edit the schedule.", EditSchedule),
                        new Tuple<string, string, Action>("addsub", "Adds a subject to the palette.", Subject.Add),
                        new Tuple<string, string, Action>("remsub", "Remove a subject from the palette.", Subject.Remove),
                        new Tuple<string, string, Action>("exit", "", () => { exit = true; })
                    }
                );
                if (exit) break;
            }
        }

        static void EditSchedule()
        {

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