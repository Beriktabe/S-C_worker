using Server_PC;
using System;
using System.Net;
using System.Net.Sockets;

using System.Threading.Tasks;
using System.Threading;

using System.Windows.Forms;

namespace Classes
{
    class Server : IDisposable
    {
        TcpListener Listener;
        bool Enabled = false;
   //     IPAddress ip = IPAddress.Any;

        public Server()
        {
            Enabled = true;
            Listener = new TcpListener(Settings.ip, Settings.port); // Создаем "слушателя" для указанного порта
            Listener.Start(); // Запускаем его
            Settings.state = "Включен";
            Run();
        }

        public async void Run()
        {
            while(Enabled == true)
            {
              //  try
              //  {
                    TcpClient client = await Listener.AcceptTcpClientAsync();
                    ClientThread(client);
              //  }
              //  catch
              //  {
              //      if (Enabled)
              //          throw;
              //  }
            }
        }

        static void ClientThread(Object StateInfo)
        {
            // Просто создаем новый экземпляр класса Client и передаем ему приведенный к классу TcpClient объект StateInfo
            new Client((TcpClient)StateInfo);
        }
        public void Dispose()
        {
            if (Enabled == true)
            {

                // Если "слушатель" был создан
                if (Listener != null)
                {
                    // Остановим его

                    Settings.state = "Выключен";

                    Listener.Stop();
                    Listener = null;

                    Enabled = false;
                }
            }
        }
        // Остановка сервера
        ~Server()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
                Settings.state = "Выключен";
            }
        }
    }
}
