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
    public class UnicastUdpSender : UdpClient
    {
        private readonly IPEndPoint _dst;

        private readonly Encoding _encoding;

        public UnicastUdpSender(IPEndPoint src, IPEndPoint dst, Encoding encoding) : base(src)
        {
            _dst = dst;
            _encoding = encoding;

            this.Connect(_dst);
        }

        public int SendData(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return 0;
            }

            var buffer = _encoding.GetBytes(data);

            return SendData(buffer); ;
        }

        public int SendData(byte[] data)
        {
            if (
                (data == null) || 
                (data.Count() == 0)
                )
            {
                return 0;
            };

            int sendsize = 0;
            try
            {
                sendsize = this.Send(data, data.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (sendsize == 0)
                {
                    Console.WriteLine("send data is 0 byte");
                }
            }

            return sendsize;
        }

        public async Task<int> SendDataAsync(string data)
        {
            var bytedata = _encoding.GetBytes(data);
            return await this.SendAsync(bytedata, bytedata.Length);
        }
    }
}
