using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Data
{
    [Serializable]
    public class EnemyWeakness
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; } = false;

        [XmlElement("Name")]
        public String Name { get; set; } = "UNKNOWN";

        [XmlElement("Offset")]
        public String Offset { get; set; } = "0";

        [XmlElement("Buster")]
        public String Buster { get; set; } = "0";

        [XmlElement("Heat")]
        public String Heat { get; set; } = "0";

        [XmlElement("Air")]
        public String Air { get; set; } = "0";

        [XmlElement("Wood")]
        public String Wood { get; set; } = "0";

        [XmlElement("Bubble")]
        public String Bubble { get; set; } = "0";

        [XmlElement("Quick")]
        public String Quick { get; set; } = "0";

        [XmlElement("Crash")]
        public String Crash { get; set; } = "0";

        [XmlElement("Metal")]
        public String Metal { get; set; } = "0";
    }


    [Serializable]
    [XmlRoot("EnemyWeaknessSet")]
    public class EnemyWeaknessSet : IEnumerable<EnemyWeakness>
    {
        [XmlArrayItem("EnemyWeakness", typeof(EnemyWeakness))]
        public List<EnemyWeakness> EnemyWeaknesses { get; set; } = new List<EnemyWeakness>();

        public IEnumerator<EnemyWeakness> GetEnumerator()
        {
            return ((IEnumerable<EnemyWeakness>)this.EnemyWeaknesses).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.EnemyWeaknesses.GetEnumerator();
        }

        public void Add(EnemyWeakness in_Element)
        {
            this.EnemyWeaknesses.Add(in_Element);
        }
    }
}
