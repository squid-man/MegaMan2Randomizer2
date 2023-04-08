using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MM2Randomizer.Data
{
    [Serializable]
    public class SoundTrack
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Title")]
        public String Title { get; set; } = "UNKNOWN";

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
