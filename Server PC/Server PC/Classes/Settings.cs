using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Classes
{
    class Settings
    {
        public static IPAddress ip = IPAddress.Any;
        public static int port = 1010;
        public static string path = Environment.CurrentDirectory + @"\BD\Services_db.db";
        public static int count = 0;

        public static string state = "Выключен";
        public static bool Started = false;
    }
}
