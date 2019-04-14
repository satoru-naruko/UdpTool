using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpTool.Model
{
    public class RtpPacket
    {
        private byte[] header = new byte[12];
        byte[] payload;
        byte[] packet;
        private static int interval = 100;
        private byte[] someArray;
        private int seqNo;
        private int length;

        public RtpPacket(byte[] data, int seqNo)
        {
            int timestamp = seqNo * interval;
            CreateHeader(seqNo, timestamp);
            payload = new byte[data.Length];
            payload = data;
            packet = new byte[data.Length + 12];
            for (int i = 0; i < 12; i++)
            {
                packet[i] = header[i];
            }
            for (int j = 12; j < data.Length + 12; j++)
            {
                packet[j] = payload[j - 12];
            }
        }

        public RtpPacket(byte[] payload, int seqNo, int length)
        {
            this.someArray = payload;
            this.seqNo = seqNo;
            this.length = length;

            int timestamp = seqNo * interval;
            CreateHeader(seqNo, timestamp);
            this.payload = new byte[length];
            this.payload = payload;
            this.packet = new byte[length + 12];
            // ヘッダーをパケットに詰める
            for (int i = 0; i < 12; i++)
            {
                this.packet[i] = header[i];
            }
            // ペイロードを設定
            for (int j = 12; j < length + 12; j++)
            {
                this.packet[j] = this.payload[j - 12];
            }
        }
        public void CreateHeader(int sequenceNum, int tStamp)
        {
            int version = 2;
            int padding = 0;
            int extension = 0;
            int csrcCount = 0;
            int marker = 0;
            int payloadType = 26;
            int sequenceNumber = sequenceNum;
            long timestamp = tStamp;
            long SSRC = 0;

            header[0] = (byte)((version & 0x3) << 6 | (padding & 0x1) << 5 | (extension & 0x0) << 4 | (csrcCount & 0x0));

            // 2BYTE目
            // M(7bit) | PayloadType( 6-0bit)
            header[1] = (byte)((marker & 0x1) << 7 | payloadType & 0x7f);

            // シーケンス番号
            header[2] = (byte)((sequenceNumber & 0xff00) >> 8);
            header[3] = (byte)(sequenceNumber & 0x00ff);

            //packet timestamp , 4 bytes in big endian format
            header[4] = (byte)((timestamp & 0xff000000) >> 24);
            header[5] = (byte)((timestamp & 0x00ff0000) >> 16);
            header[6] = (byte)((timestamp & 0x0000ff00) >> 8);
            header[7] = (byte)(timestamp & 0x000000ff);

            //our CSRC , 4 bytes in big endian format
            header[8] = (byte)((SSRC & 0xff000000) >> 24);
            header[9] = (byte)((SSRC & 0x00ff0000) >> 16);
            header[10] = (byte)((SSRC & 0x0000ff00) >> 8);
            header[11] = (byte)(SSRC & 0x000000ff);

        }

        public byte[] GetPacket()
        {
            return packet;
        }
    }
}
