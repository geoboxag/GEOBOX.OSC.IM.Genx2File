using GEOBOX.OSC.IM.Genx2File.Converters;
using GEOBOX.OSC.IM.Genx2File.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOBOX.OSC.IM.Genx2File.Writers
{
    public sealed class FileWriter
    {
        private ConvertControler convertControler;

        public FileWriter(ConvertControler controler)
        {
            convertControler = controler;
        }

        public void WriteFile()
        {
            switch (convertControler.GetConvertMethod())
            {
                case ConvertMethod.GENX2CSV:
                    WriteCsv();
                    break;
                case ConvertMethod.GENX2TXT:
                    WriteText();
                    break;
            }
        }

        private void WriteCsv()
        {
            if (convertControler.OutputRows.Count == 0)
            {
                return;
            }

            StreamWriter streamWriter = new StreamWriter(convertControler.GetSaveFilePath());
            streamWriter.WriteLine(convertControler.OutputRows.FirstOrDefault().GetCsvHeaderString());

            foreach (OutputRow featureClass in convertControler.OutputRows)
            {
                streamWriter.WriteLine(featureClass.GetCsvString());
            }

            streamWriter.Close();
        }

        private void WriteText()
        {
            if (convertControler.OutputRows.Count == 0)
            {
                return;
            }

            StreamWriter streamWriter = new StreamWriter(convertControler.GetSaveFilePath());
            streamWriter.WriteLine(convertControler.OutputRows.FirstOrDefault().GetTextHeaderString());

            foreach (OutputRow featureClass in convertControler.OutputRows)
            {
                streamWriter.WriteLine(featureClass.GetTextString());
            }

            streamWriter.Close();
        }
    }
}
