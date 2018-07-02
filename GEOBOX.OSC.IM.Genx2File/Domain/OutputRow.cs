using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOBOX.OSC.IM.Genx2File.Domain
{
    public class OutputRow : ICloneable
    {
        public string Version { get; private set; }
        public FeatureClassType FeatureClassType { get; private set; }
        public string FeatureClassOperation { get; set; }
        public string FeatureClassName { get; private set; }
        public string FeatureClassDescription { get; set; }
        public AttributType AttributType { get; set; }
        public string AttributOperation { get; set; }
        public string AttributName { get; private set; }
        public string AttributDescription { get; set; }
        public string DomainActive { get; set; }

        public OutputRow(string version)
        {
            Version = version;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void SetFeatureClass(string opertation, string name, string description)
        {
            FeatureClassType = FeatureClassType.FeatureClass;
            SetFeatureClassInfos(opertation, name, description);
        }

        public void SetDomainClass(string opertation, string name, string description)
        {
            FeatureClassType = FeatureClassType.DomainTable;
            SetFeatureClassInfos(opertation, name, description);
        }

        private void SetFeatureClassInfos(string opertation, string name, string description)
        {
            FeatureClassOperation = opertation;
            FeatureClassName = name;
            FeatureClassDescription = description;
        }

        public void SetAttribut(string opertation, string name, string description)
        {
            AttributType = AttributType.Attribut;
            SetAttributInfos(opertation, name, description);
        }

        public void SetDomainEntry(string opertation, string value, string id, string active)
        {
            AttributType = AttributType.DomainEntry;
            SetAttributInfos(opertation, value, $"ID: {id}");
            DomainActive = active;
        }

        private void SetAttributInfos(string opertation, string name, string description)
        {
            AttributOperation = opertation;
            AttributName = name;
            AttributDescription = description;
        }

        public string GetCsvString()
        {
            return GetExportString(";");
        }

        public string GetTextString()
        {
            return GetExportString(",");
        }

        private string GetExportString(string delimiter)
        {
            return $"{Version}{delimiter}{GetExportStringFeatureClass(delimiter)}{delimiter}{GetExportStringAttribut(delimiter)}";
        }

        private string GetExportStringFeatureClass(string delimiter)
        {
            return $"{FeatureClassType.ToString()}{delimiter}{FeatureClassOperation}{delimiter}{FeatureClassName}{delimiter}{FeatureClassDescription}";
        }

        private string GetExportStringAttribut(string delimiter)
        {
            return $"{AttributType.ToString()}{delimiter}{AttributOperation}{delimiter}{AttributName}{delimiter}{AttributDescription}{delimiter}{DomainActive}";
        }

        public string GetCsvHeaderString()
        {
            return GetRowHeader(";");
        }

        public string GetTextHeaderString()
        {
            return GetRowHeader(",");
        }

        private string GetRowHeader(string delimiter)
        {
            return $"Version:{delimiter}Typ:{delimiter}Operation:{delimiter}Klassenname:{delimiter}Beschreibung:{delimiter}Typ:{delimiter}Operation:{delimiter}Name:{delimiter}Beschreibung:{delimiter}Aktiv:";
        }
    }
}
