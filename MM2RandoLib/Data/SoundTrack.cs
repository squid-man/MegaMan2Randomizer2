using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Data
{
    [Flags]
    public enum SoundTrackUsage
    {
        Intro = 1,
        Title = 2,
        StageSelect = 4,
        Stage = 8,
        Boss = 16,
        Ending = 32,
        Credits = 64,
    }

    [Serializable]
    public class SoundTrack
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Title")]
        public String Title { get; set; } = "UNKNOWN";

        [XmlIgnore]
        public static String[] DefaultUses = new String[]{"Stage", "Credits"};

        [XmlArray(ElementName = "Uses")]
        [XmlArrayItem(ElementName = "Usage")]
        public String[] Uses { get; set; } = DefaultUses;

        [XmlIgnore]
        public SoundTrackUsage Usage
        {
            get
            {
                SoundTrackUsage usage = (SoundTrackUsage)0;
                foreach (String str in Uses)
                    usage |= Enum.Parse<SoundTrackUsage>(str);

                return usage;
            }
        }

        [XmlElement("StartAddress")]
        public String StartAddress { get; set; } = "0";

        [XmlElement("TrackData")]
        public String TrackData { get; set; } = String.Empty;
    }


    [Serializable]
    [XmlRoot("SoundTrackSet")]
    public class SoundTrackSet : IEnumerable<SoundTrack>
    {
        [XmlArrayItem("SoundTrack", typeof(SoundTrack))]
        public List<SoundTrack> SoundTracks { get; set; } = new List<SoundTrack>();

        public IEnumerator<SoundTrack> GetEnumerator()
        {
            return ((IEnumerable<SoundTrack>)this.SoundTracks).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.SoundTracks.GetEnumerator();
        }

        public void Add(SoundTrack in_Element)
        {
            this.SoundTracks.Add(in_Element);
        }
    }
}
