using GEOBOX.OSC.IM.Genx2File.Converters;
using GEOBOX.OSC.IM.Genx2File.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GEOBOX.OSC.IM.Genx2File.Readers
{
    public sealed class GENXReader
    {
        private ConvertControler convertControler;

        private XmlTextReader reader;
        private XmlDocument xmlReadDoc;

        public GENXReader(ConvertControler controler)
        {
            convertControler = controler;
        }

        public bool ReadFile()
        {
            if (!OpenXmlReader())
            {
                Console.WriteLine("Vorgang abgebrochen:");
                Console.WriteLine("GENX-Datei wurde nicht gefunden.");
                return false;
            }

            if (!ReadXmlDocument())
            {
                Console.WriteLine("Vorgang abgebrochen:");
                Console.WriteLine("GENX-Datei konnte nicht korrekt geöffnet werden.");
                return false;
            }

            if (!ReadGENX())
            {
                Console.WriteLine("Vorgang abgebrochen:");
                Console.WriteLine("GENX-Datei konnte nicht korrekt gelesen werden werden.");
                return false;
            }

            return true;
        }

        private bool OpenXmlReader()
        {

            if (System.IO.File.Exists(convertControler.GetGenxFilePath()))
            {
                reader = new XmlTextReader(convertControler.GetGenxFilePath());
                return true;
            }

            return false;
        }

        private bool ReadXmlDocument()
        {
            try
            {
                xmlReadDoc = new XmlDocument();
                xmlReadDoc.Load(reader);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ReadGENX()
        {
            XmlNodeList nameList = xmlReadDoc.GetElementsByTagName("UpdateVersion");

            foreach (XmlNode xn in nameList)
            {
                string version = xn.Attributes["Version"].Value;
                //Console.WriteLine(version);

                XmlNodeList childNode = xn.ChildNodes;

                foreach (XmlNode xnc in childNode)
                {
                    if (xnc.Name == "Topics")
                    {
                        XmlNodeList xnl = xmlReadDoc.GetElementsByTagName("FeatureClass");
                        foreach (XmlNode x1 in xnl)
                        {
                            string fClassName = x1.Attributes["Name"].Value;
                            string fClassOperation = x1.Attributes["Operation"].Value;

                            string fClassCaption = String.Empty;
                            if (x1["Caption"] != null)
                            {
                                fClassCaption = x1["Caption"].InnerXml;
                            }

                            OutputRow outputRowForCopy = new OutputRow(version);
                            outputRowForCopy.SetFeatureClass(fClassOperation, fClassName, fClassCaption);

                            if (x1["Attributes"] == null) continue;
                            foreach (XmlNode x2 in x1["Attributes"])
                            {
                                string attributName = x2.Attributes["Name"].Value;
                                string attributOperation = x2.Attributes["Operation"].Value;

                                string attributCatption = String.Empty;
                                if (x2["Caption"] != null)
                                {
                                    attributCatption = x2["Caption"].InnerXml;
                                }

                                OutputRow outPutRow = (OutputRow)outputRowForCopy.Clone();
                                outPutRow.SetAttribut(attributOperation, attributName, attributCatption);
                                convertControler.AddFeatureClassRow(outPutRow);

                            }
                        }
                    }

                    else if (xnc.Name == "DomainTables")
                    {
                        foreach (XmlNode xnd in xnc)
                        {
                            string domainClassName = xnd.Attributes["Name"].Value;
                            string domainClassOperation = xnd.Attributes["Operation"].Value;

                            string domainClassDescription = String.Empty;
                            if (xnd["Caption"] != null)
                            {
                                domainClassDescription = xnd["Caption"].InnerXml;
                            }

                            OutputRow outputRowForCopy = new OutputRow(version);
                            outputRowForCopy.SetDomainClass(domainClassOperation, domainClassName, domainClassDescription);

                            foreach (XmlNode xne in xnd.ChildNodes)
                            {
                                if (xne.Name != "DomainEntries") continue;
                                foreach (XmlNode xna in xne.ChildNodes)
                                {
                                    string id = xna.Attributes["Id"].Value;
                                    string value = String.Empty;
                                    if (xna["Value"] != null)
                                    {
                                        value = xna["Value"].InnerXml;
                                    }
                                    string operation = xna.Attributes["Operation"].Value;
                                    string active = xna.Attributes["Active"].Value;

                                    OutputRow domainRow = (OutputRow)outputRowForCopy.Clone();
                                    domainRow.SetDomainEntry(operation, value, id, active);

                                    convertControler.AddFeatureClassRow(domainRow);
                                }
                            }
                        }
                    }
                }

            }

            return true;
        }
    }
}