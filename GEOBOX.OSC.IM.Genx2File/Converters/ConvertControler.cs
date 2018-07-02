using GEOBOX.OSC.IM.Genx2File.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOBOX.OSC.IM.Genx2File.Converters
{
    public sealed class ConvertControler
    {
        private const string OUTPUTFILENAME = "Datamodel";
        private string inputGenxFileName;
        private string outputFileName;
        private ConvertMethod convertMethod;
        private string executionDirectory;
        public List<OutputRow> OutputRows { get; private set; }

        #region Constructor
        public ConvertControler()
        {
            inputGenxFileName = "Datamodel.genx";
            SetMethodToCSVFile();
            SetExecutionDirectory();
            OutputRows = new List<OutputRow>();
        }
        #endregion

        #region Handle Execution Directory
        public string GetDirectory()
        {
            if (!String.IsNullOrEmpty(executionDirectory) & System.IO.Directory.Exists(executionDirectory))
            {
                return executionDirectory;
            }

            return System.IO.Path.GetTempPath();
        }

        private void SetExecutionDirectory()
        {
            try
            {
                executionDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception ex)
            {
                executionDirectory = String.Empty;
                Debug.WriteLine(ex.Message);
            }
        } 
        #endregion

        public void AddFeatureClassRow(OutputRow listEntry)
        {
            OutputRows.Add(listEntry);
        }

        #region Set Convert Methods
        public ConvertMethod GetConvertMethod()
        {
            return convertMethod;
        }
        public void SetMethodToCSVFile()
        {
            convertMethod = ConvertMethod.GENX2CSV;
            SetOutputFilename("csv");
        }

        public void SetMethodToTextFile()
        {
            convertMethod = ConvertMethod.GENX2TXT;
            SetOutputFilename("txt");
        }
        #endregion

        private void SetOutputFilename(string fileExtension)
        {
            outputFileName = $"{OUTPUTFILENAME}.{fileExtension}";
        }

        public string GetGenxFilePath()
        {
            return System.IO.Path.Combine(executionDirectory, inputGenxFileName);
        }

        public string GetSaveFilePath()
        {
            return System.IO.Path.Combine(executionDirectory, outputFileName);
        }
    }
}
