using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    class sql_commands
    {
        public static SQLiteConnection connection = new SQLiteConnection("Data source = " + Settings.path);

        public static string[] GetTables()
        {
            List<string> mass = new List<string>();

            // listBox1.Items.Clear();
            SQLiteCommand command = new SQLiteCommand(connection);
            connection.Open();
            command.CommandText = "SELECT tbl_name FROM sqlite_master";

            using (SQLiteDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    mass.Add(rdr[0].ToString());
                }
                rdr.Close();
            }
            command.Dispose();
            connection.Close();

            return mass.ToArray<string>();
        }

        public static bool GetAll(string name, out string[] data1, out string[] data2)
        {
            List<string> mass1 = new List<string>();
            List<string> mass2 = new List<string>();

            string a = "'" + name + "'";
            SQLiteCommand command = new SQLiteCommand(connection);
            connection.Open();
            command.CommandText = string.Format("SELECT * FROM {0}", a);
            using (SQLiteDataReader rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    mass1.Add(rdr[0].ToString());
                    mass2.Add(rdr[1].ToString());
                }
                rdr.Close();
            }
            command.Dispose();
            connection.Close();
            data1 = mass1.ToArray<string>();
            data2 = mass2.ToArray<string>();
            return true;

        }

        public static bool CreateTable(string name)
        {
            string a = "'" + name + "'";
            SQLiteCommand command = new SQLiteCommand(connection);
            connection.Open();
            command.CommandText = string.Format("DROP TABLE IF EXISTS {0};", a);
            command.ExecuteNonQuery();

            command.Dispose();
            connection.Close();

            return true;
        }
        public static bool DeleteTable(string name)
        {
            string a = "'" + name + "'";
            SQLiteCommand command = new SQLiteCommand(connection);
            connection.Open();
            command.CommandText = string.Format("DROP TABLE IF EXISTS {0};", a);
            command.ExecuteNonQuery();

            command.Dispose();
            connection.Close();

            return true;
        }

        public static bool InsertCommands(string[] commands)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            connection.Open();
            foreach (string cmd in commands)
            { 
                try
                {
                   command.CommandText = cmd;
                   command.ExecuteNonQuery();
                }
                catch { return false; }
            }
            command.Dispose();
            connection.Close();

            return true;
        }
    }
}
