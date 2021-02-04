using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Randomizers
{
    [Serializable]
    public class CompanyName
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        public String GetCompanyName()
        {
            String prefix = this.Name ?? String.Empty;

            // Truncate the prefix to the max length
            String truncatedPrefix = prefix.Substring(0, Math.Min(CountryName.MAX_PREFIX_LENGTH, prefix.Length));
            return truncatedPrefix.PadRight(CountryName.MAX_PREFIX_LENGTH).AsIntroString();
        }

        private const Int32 MAX_NAME_LENGTH = 13;
    }

    [Serializable]
    [XmlRoot("CompanyNameSet")]
    public class CompanyNameSet : IEnumerable<CompanyName>
    {
        [XmlArrayItem("CompanyName", typeof(CompanyName))]
        public List<CompanyName> CompanyNames { get; set; } = new List<CompanyName>();

        public IEnumerator<CompanyName> GetEnumerator()
        {
            return ((IEnumerable<CompanyName>)this.CompanyNames).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.CompanyNames.GetEnumerator();
        }

        public void Add(CompanyName in_Element)
        {
            this.CompanyNames.Add(in_Element);
        }
    }
}
