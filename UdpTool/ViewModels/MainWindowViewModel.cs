using Prism.Mvvm;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using UdpTool.Model;
using Prism.Commands;

namespace UdpTool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand SendCommand { get; private set; }

        private string _title = "UdpTool";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isuni = true;
        public bool IsUni
        {
            get { return _isuni; }
            set { SetProperty(ref _isuni, value); }
        }

        private bool _isbld = false;
        public bool IsBld
        {
            get { return _isbld; }
            set { SetProperty(ref _isbld, value); }
        }

        private bool _ismut = false;
        public bool IsMut
        {
            get { return _ismut; }
            set { SetProperty(ref _ismut, value); }
        }

        // IPアドレス(v4)
        private string _ip1 = "192";
        public string Ip1
        {
                get { return _ip1; }
                set { SetProperty(ref _ip1, value);}
        }

        private string _ip2 = "168";
        public string Ip2
        {
            get { return _ip2; }
            set { SetProperty(ref _ip2, value); }
        }

        private string _ip3 = "0";
        public string Ip3
        {
            get { return _ip3; }
            set { SetProperty(ref _ip3, value); }
        }

        private string _ip4 = "249";
        public string Ip4
        {
            get { return _ip4; }
            set { SetProperty(ref _ip4, value); }
        }

        /// <summary>
        /// 送信ポート番号
        /// </summary>
        /// <value>UDP送信ポート</value>
        private string _port = "5001";
        public string Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }

        public int IntPort => int.Parse(Port);

        private string IPV4Address
        {
            get { return Ip1 + "." + Ip2 + "." + Ip3 + "." + Ip4; }
        }

        private string _sendData;
        public string SendData
        {
            get { return _sendData; }
            set { SetProperty(ref _sendData, value); }
        }

        public int SendDataByteSize
        {
            get { return Encoding.GetEncoding("UTF-8").GetByteCount(SendData); }
        }

        public ObservableCollection<UnicastIPAddressInformation> NetworkInterFaces { get; private set; } = new ObservableCollection<UnicastIPAddressInformation>();

        public MainWindowViewModel()
        {
            SendCommand = new DelegateCommand(
                () => {
                    // 送信データ
                    var buffer = Encoding.UTF8.GetBytes(SendData);

                    var sender = new UnicastUdpSender(new IPEndPoint(IPAddress.Parse("127.0.0.1"), IntPort), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1024), Encoding.UTF8);

                    var rtpPacket = new RtpPacket(buffer, 1, buffer.Length);

                    var senddatabyte = sender.SendData(rtpPacket.GetPacket());

                    System.Console.WriteLine("send data byte => {0}", senddatabyte);
                }
            );

            SendData = @"{
  ""array"": [
    1,
    2,
    3
  ],
  ""boolean"": true,
  ""color"": ""#82b92c"",
  ""null"": null,
  ""number"": 123,
  ""object"": {
                ""a"": ""b"",
    ""c"": ""d"",
    ""e"": ""f""
  },
  ""string"": ""Hello World""
}
";

            // 起動時に受信するようにしておく
            var r = new UdpReciver(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000), Encoding.UTF8);

            r.UdpPacketRecived += (s, e) => {

                System.Console.WriteLine("recive packet!!");

            };
            r.ReciveDataAsync();

            // ---------------すべてのNICで送信するサンプル----------------
            
            //var nis = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
            //.Where(v => v.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
            //.Where(v => v.GetIPProperties().UnicastAddresses.Count(y => y.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) >= 1);

            //// ユニキャストアドレスをすべて列挙
            //var UniAddresses = nis.SelectMany(v => v.GetIPProperties().UnicastAddresses).Where(v => v.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            //NetworkInterFaces.AddRange(UniAddresses);

            //foreach (var address in UniAddresses)
            //{
            //    if (address.Address.ToString().Contains("192.168"))
            //    {
            //        // アドレス毎に送信(port 0なら任意のポートから) 
            //        using (var UdpClient = new System.Net.Sockets.UdpClient(new IPEndPoint(address.Address, 0)))
            //        {
            //            UdpClient.Send(buffer, buffer.Length, new IPEndPoint( IPAddress.Parse("224.0.0.10"), port));
            //        }
            //    }
            //}

            //// ブロードキャスト送信(IPV4)
            //var client = new UdpClient(port, AddressFamily.InterNetwork);
            //client.EnableBroadcast = true;

            //client.Ttl = 10;

            //client.Connect(new IPEndPoint(IPAddress.Broadcast, port));

            //client.Send(buffer, buffer.Length);
            //client.Close();

        }
    }
}
