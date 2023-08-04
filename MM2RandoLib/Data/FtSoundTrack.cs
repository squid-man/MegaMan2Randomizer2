using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Data
{
    [Serializable]
    public class FtSong
    {
        [XmlElement(DataType = "boolean")]
        public Boolean? Enabled { get; set; } = null;

        [XmlElement(DataType = "int")]
        public Int32 Number { get; set; } = 0;

        [XmlElement]
        public String Title { get; set; } = "";

        [XmlElement(DataType = "boolean")]
        public Boolean? SwapSquareChans { get; set; } = null;

        [XmlArray(ElementName = "Uses")]
        [XmlArrayItem(ElementName = "Usage")]
        public HashSet<String> Uses { get; set; } = new(StringComparer.InvariantCultureIgnoreCase);

        [XmlIgnore]
        public ESoundTrackUsage Usage
        {
            get { return SoundTrackUsage.FromStrings(Uses); }
        }
    }

    [Serializable]
    public class FtModule
    {
        [XmlElement(DataType = "boolean")]
        public Boolean Enabled { get; set; } = true;

        [XmlElement]
        public String Title { get; set; } = "";

        [XmlElement]
        public String Author { get; set; } = "";

        [XmlElement(DataType = "boolean")]
        public Boolean SwapSquareChans { get; set; } = false;

        [XmlArray(ElementName = "Uses")]
        [XmlArrayItem(ElementName = "Usage")]
        public HashSet<String> Uses { get; set; } = new(StringComparer.InvariantCultureIgnoreCase);

        [XmlIgnore]
        public ESoundTrackUsage Usage
        {
            get { return SoundTrackUsage.FromStrings(Uses); }
        }

        [XmlElement("StartAddress")]
        public String StartAddressString { get; set; } = "0";

        [XmlIgnore]
        public Int32 StartAddress
        {
            get { return Convert.ToInt32(StartAddressString, 16); }
            set { StartAddressString = $"{value:X}";  }
        }

        [XmlElement(DataType = "hexBinary")]
        public Byte[] TrackData { get; set; } = new Byte[]{};

        [XmlIgnore]
        public Int32 Size { get { return TrackData.Length; } }

        [XmlArray(ElementName = "Songs")]
        [XmlArrayItem(ElementName = "Song", Type = typeof(FtSong))]
        public List<FtSong> Songs { get; set; } = new List<FtSong>();

        /*[XmlAnyElement]
        public object[] AllElements;*/
    }


    [Serializable]
    [XmlRoot("ModuleSet")]
    public class FtModuleSet// : IEnumerable<FtModule>
    {
        [XmlArray(ElementName = "Modules")]
        [XmlArrayItem("Module", typeof(FtModule))]
        public List<FtModule> Modules { get; set; } = new List<FtModule>();

        /*[XmlAnyElement]
        public object[] AllElements;*/

        public IEnumerator<FtModule> GetEnumerator()
        {
            return ((IEnumerable<FtModule>)this.Modules).GetEnumerator();
        }

        /*IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Modules.GetEnumerator();
        }*/

        public void Add(FtModule in_Element)
        {
            this.Modules.Add(in_Element);
        }
    }
}
