using GEOBOX.OSC.IM.Genx2File.Converters;
using GEOBOX.OSC.IM.Genx2File.Readers;
using GEOBOX.OSC.IM.Genx2File.Writers;
using System;

namespace ConvertGENX2XLS
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("GEOBOX - GENX2FILE");
            Console.WriteLine("Datei wird konvertiert....");
            ConvertControler convertControler = new ConvertControler();

            GENXReader genxReader = new GENXReader(convertControler);
            genxReader.ReadFile();

            FileWriter fileWriter = new FileWriter(convertControler);
            fileWriter.WriteFile();

            Console.WriteLine("En(t)e gut, alles gut?");
            Console.ReadLine();
        }
    }
}
