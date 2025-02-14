using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Beriktabe.TcpWorker;

namespace Classes
{
    class Client
    {
        public TcpClient cl;
        public NetworkStream netStream;
        TcpWorker worker = new TcpWorker();
        // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
        public Client(TcpClient Client)
        {
            cl = Client;
            
            netStream = cl.GetStream();
            worker.netStream = netStream;

            Settings.count++;
            byte[] Buffer = new byte[1024]; // Буфер для хранения принятых от клиента данных
            int Count = netStream.Read(Buffer, 0, Buffer.Length); // Переменная для хранения количества байт, принятых от клиента
            if(Count > 0)
            {
                switch (Encoding.UTF8.GetString(Buffer, 0, Count))
                {
                    case "Get db": SendFile(Settings.path); break;
                    case "Recieve": /* worker.ReceiveFile(Environment.CurrentDirectory + "\\", "Hi.xml"); */break;
                    default: worker.Send("ERROR"); break;
                }
            }
            Settings.count--;
            OnDisconnect();
        }

        private void OnDisconnect()
        {
            netStream.Dispose();
            netStream.Close();
            cl.Close();
            worker.CloseStream();
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
    }
}