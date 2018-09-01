using System;
using System.IO;
using System.Xml.Serialization;
using System.Net;

namespace Atlas
{
    public class Program
    {
        public struct Data
        {
            public string name;
            public int number;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");

            Data data;

            Console.WriteLine("Name: ");
            data.name = Console.ReadLine();
            do
            {
                Console.WriteLine("Number: ");
            } while (!int.TryParse(Console.ReadLine(), out data.number));



            FileStream stream = File.Create("test.txt");
            XmlSerializer serializer = new XmlSerializer(typeof(Data));

            serializer.Serialize(stream, data);

            stream.Flush();
            stream.Close();
        }
    }
}
