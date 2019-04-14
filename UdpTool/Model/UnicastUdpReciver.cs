using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UdpTool.Model
{
    public class UdpPacketReciveEventArgs : EventArgs
    {
        public UdpPacketReciveEventArgs(string _data)
        {
            Data = _data;
        }
        public string Data { get; private set; }
    }

    public class UdpReciver : UdpClient
    {
        public event EventHandler<UdpPacketReciveEventArgs> UdpPacketRecived;

        private readonly Encoding _encoding;

        public UdpReciver(IPEndPoint local, Encoding encoding) : base(local)
        {
            _encoding = encoding;
        }

        public async void ReciveDataAsync()
        {
            while (true)
            {
                // データ受信待機
                var result = await this.ReceiveAsync();

                // 受信したデータを変換
                var data = Encoding.UTF8.GetString(result.Buffer);

                // Receive イベント を実行
                this.UdpPacketRecived(this, new UdpPacketReciveEventArgs(data) );
            }
        }
    }
}
