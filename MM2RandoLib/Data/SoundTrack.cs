﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Data
{
    [Serializable]
    public class SoundTrack
    {
        [XmlElement("Enabled")]
        public Boolean Enabled { get; set; }

        [XmlElement("Title")]
        public String Title { get; set; } = "UNKNOWN";

        [XmlArray(ElementName = "Uses")]
        [XmlArrayItem(ElementName = "Usage")]
        public String[]? Uses { get; set; } = null;

        [XmlIgnore]
        public ESoundTrackUsage? Usage
        {
            get
            {
                if (Uses is null)
                    return null;
                
                ESoundTrackUsage usage = (ESoundTrackUsage)0;
                foreach (String str in Uses)
                    usage |= Enum.Parse<ESoundTrackUsage>(str);

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
