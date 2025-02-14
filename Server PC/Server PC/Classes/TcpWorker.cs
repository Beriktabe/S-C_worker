using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace Beriktabe.TcpWorker
{
    class TcpWorker
    {

        public NetworkStream netStream = null;
        public void Send(string message)
        {

            byte[] data = Encoding.UTF8.GetBytes(message);
            netStream.Write(data, 0, data.Length);
        }
        public void SendFile(string FileName)
        {
            byte[] SendingBuffer = null;
            FileStream Fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(1024))); // max

            int TotalLength = (int)Fs.Length;
            int CurrentPacketLength;
            for (int i = 0; i < NoOfPackets; i++)
            {
                if (TotalLength > 1024)
                {
                    CurrentPacketLength = 1024;
                    TotalLength = TotalLength - CurrentPacketLength;
                }
                else
                    CurrentPacketLength = TotalLength;
                SendingBuffer = new byte[CurrentPacketLength];

                Fs.Read(SendingBuffer, 0, CurrentPacketLength);

                netStream.Write(SendingBuffer, 0, SendingBuffer.Length);
            }
            Fs.Close();
        }
        public void ReceiveFile(string path, string filename)
        {
            string a1 = path + filename;
            byte[] RecData = new byte[1024];
            int RecBytes;
            bool tick = true;
            while (tick)
            {
                if (a1 != string.Empty)
                {
                    int totalrecbytes = 0;
                    FileStream Fs = new FileStream(a1, FileMode.CreateNew, FileAccess.Write);
                    while ((RecBytes = netStream.Read(RecData, 0, RecData.Length)) > 0)
                    {
                        Fs.Write(RecData, 0, RecBytes);
                        totalrecbytes += RecBytes;
                    }
                    Fs.Close();
                }
                tick = false;
            }
        }
        public string[] Recieve()
        {
            string[] data = null;
            int index = 0;

            byte[] Buffer = new byte[1024]; // Буфер для хранения принятых от клиента данных
            int Count = netStream.Read(Buffer, 0, Buffer.Length); // Переменная для хранения количества байт, принятых от клиента
            do
            {
                data[index] = Encoding.UTF8.GetString(Buffer, 0, Count);

                Count = netStream.Read(Buffer, 0, Buffer.Length);
                index++;
            }
            while (Count > 0);
            return data;
        }

        public void CloseStream()
        {
            netStream.Close();
            netStream.Dispose();
        }
    }
}
