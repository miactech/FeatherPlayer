using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatherPlayer
{
    public class AudioInfo
    {
        /// <summary>
        /// Code From: https://blog.csdn.net/WZh0316/article/details/88414292
        /// </summary>
        public class wav
        {

            public struct WavInfo
            {
                public string groupid;
                public string rifftype;
                public long filesize;
                public string chunkid;
                /// <summary>
                /// 位深
                /// </summary>
                public long chunksize;
                public short wformattag; //记录着此声音的格式代号
                /// <summary>
                /// 声道数
                /// </summary>
                public ushort wchannels; //记录声音的频道数。
                /// <summary>
                /// 采样率
                /// </summary>
                public ulong dwsamplespersec;
                /// <summary>
                ///记录每秒的数据量。
                /// </summary>
                public ulong dwavgbytespersec;
                public ushort wblockalign;//记录区块的对齐单位。
                public ushort wbitspersample;//记录每个取样所需的位元数。
                public string datachunkid;
                public long datasize;
                public int Duration
                {
                    get {
                        return Convert.ToInt32(datasize / Convert.ToInt64(dwavgbytespersec));
                    }
                }

            }

            public static WavInfo GetWavInfo(string fileName)
            {
                WavInfo wavInfo = new WavInfo();
                FileInfo fi = new FileInfo(fileName);
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs.Length >= 44)
                    {
                        byte[] bInfo = new byte[44];
                        fs.Read(bInfo, 0, 44);
                        Encoding.Default.GetString(bInfo, 0, 4);
                        if (Encoding.Default.GetString(bInfo, 0, 4) == "RIFF" && Encoding.Default.GetString(bInfo, 8, 4) == "WAVE" && Encoding.Default.GetString(bInfo, 12, 4) == "fmt ")
                        {
                            wavInfo.groupid = Encoding.Default.GetString(bInfo, 0, 4);
                            System.BitConverter.ToInt32(bInfo, 4);
                            wavInfo.filesize = System.BitConverter.ToInt32(bInfo, 4);
                            wavInfo.rifftype = Encoding.Default.GetString(bInfo, 8, 4);
                            wavInfo.chunkid = Encoding.Default.GetString(bInfo, 12, 4);
                            wavInfo.chunksize = System.BitConverter.ToInt32(bInfo, 16);
                            wavInfo.wformattag = System.BitConverter.ToInt16(bInfo, 20);
                            wavInfo.wchannels = System.BitConverter.ToUInt16(bInfo, 22);
                            wavInfo.dwsamplespersec = System.BitConverter.ToUInt32(bInfo, 24);
                            wavInfo.dwavgbytespersec = System.BitConverter.ToUInt32(bInfo, 28);
                            wavInfo.wblockalign = System.BitConverter.ToUInt16(bInfo, 32);
                            wavInfo.wbitspersample = System.BitConverter.ToUInt16(bInfo, 34);
                            wavInfo.datachunkid = Encoding.Default.GetString(bInfo, 36, 4);
                            wavInfo.datasize = System.BitConverter.ToInt32(bInfo, 40);
                        }
                    }
                }

                return wavInfo;
            }
        }
    }
}
