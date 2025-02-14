using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Server_PC;
using Classes;

namespace Forms
{
    public partial class AddUtilities : Form
    {
        public static string path = Settings.path;
        public static string srv_name;
        public static List<string> commandsList = new List<string>();
        static AddUtilities addUtilities;
        public AddUtilities()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            srv_name = null;
            commandsList.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            srv_name = null;
            textBox1.Enabled = false;
            button1.Enabled = false;
            groupBox1.Text = textBox1.Text;
            richTextBox1.Text = "       " + textBox1.Text + Environment.NewLine;
            richTextBox2.Text = "       " + "ТИП ДАННЫХ" + Environment.NewLine;
            srv_name = "'" + textBox1.Text + "'";

            commandsList.Insert(0, string.Format("CREATE TABLE IF NOT EXISTS {0} (name TEXT, type TEXT);", srv_name));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) == false && button1.Enabled == false)
            {
                richTextBox1.Text += textBox2.Text + Environment.NewLine;
                richTextBox2.Text += comboBox1.SelectedItem.ToString() + Environment.NewLine;
                commandsList.Add(string.Format("INSERT INTO {0}(name, type) VALUES ('{1}', '{2}');", srv_name,  textBox2.Text, comboBox1.SelectedItem));
            }
            else
                MessageBox.Show("имя услуги не заполнено" + Environment.NewLine + "или" + Environment.NewLine + "не заблокировано");

        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if(!File.Exists(path)) SQLiteConnection.CreateFile(path);
            bool res = sql_commands.InsertCommands(commandsList.ToArray<string>());
            if (res == false) MessageBox.Show("ERROR");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            refreshList();
            this.Dispose();
        }

        public static void refreshList()
        {
            addUtilities = new AddUtilities();
            addUtilities.Show();
        }
    }
}
