using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Data
{
    [Serializable]
    public class CreditText
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; } = false;

        [XmlElement("Value")]
        public String Value { get; set; } = "0";

        [XmlElement("Text")]
        public String Text { get; set; } = "UNKNOWN";
    }


    [Serializable]
    [XmlRoot("CreditTextSet")]
    public class CreditTextSet : IEnumerable<CreditText>
    {
        [XmlArrayItem("CreditText", typeof(CreditText))]
        public List<CreditText> CreditTexts { get; set; } = new List<CreditText>();

        public IEnumerator<CreditText> GetEnumerator()
        {
            return ((IEnumerable<CreditText>)this.CreditTexts).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.CreditTexts.GetEnumerator();
        }

        public void Add(CreditText in_Element)
        {
            this.CreditTexts.Add(in_Element);
        }
    }
}
