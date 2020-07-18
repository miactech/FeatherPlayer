using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatherPlayer
{
    /// <summary>
    /// different kinds of file's file head.
    /// </summary>
    public class Formats
    {
        public readonly byte[] flac = new byte[4] { 0x66, 0x4C, 0x61, 0x43 }; //*.flac   fLaC
        public readonly byte[] wave = new byte[4] { 0x52, 0x49, 0x46, 0x46 }; //*.wav    RIFF
        public bool checksum()
        {
            
            return false;

        }
        
    }
}
