using System;
using System.Collections.Generic;

namespace FeatherPlayer
{
    public class XmlStruct
    {
        public struct Artist
        {
            public string Name;
            public List<Album> Albums;
        }
        public struct Album
        {
            public string Name;
            public string Year;
            public List<Song> Songs;
        }
        public struct Song
        {
            public string Name;
            public uint Index;
            public uint Duration;
            public uint Bitrate;
            public string Path;
            public System.IO.FileInfo fiSong { get { return new System.IO.FileInfo(Path); } }

        }

    }
}
