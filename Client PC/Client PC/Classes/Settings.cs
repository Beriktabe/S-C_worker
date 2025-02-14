using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Beriktabe.Settings
{
    class Settings
    {
        public static string ip = "localhost";
        public static int port = 1010;
        public static string path = Environment.CurrentDirectory + "\\BD\\Services_db.db";
        public static string XmlPath = Environment.CurrentDirectory + "\\Saves\\data.xml";
        public static string choose = null;
    }
}
