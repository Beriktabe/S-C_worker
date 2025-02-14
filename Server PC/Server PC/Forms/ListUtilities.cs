using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using Server_PC;
using Classes;

namespace Forms
{
    public partial class ListUtilities : Form
    {
        public ListUtilities()
        {
            InitializeComponent();
        }
        private static string path = Settings.path;

        private static SQLiteConnection connection = new SQLiteConnection("Data source = " + path);
        static AddUtilities addUtilities;

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            refreshLoad();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                richTextBox1.Text = "";
                richTextBox2.Text = "";

                string[] data1;
                string[] data2;

                sql_commands.GetAll(listBox1.SelectedItem.ToString(),out data1, out data2);

                for(int a = 0; a != data1.Length; a++)
                {
                    richTextBox1.Text += data1[a] + Environment.NewLine;
                    richTextBox2.Text += data2[a] + Environment.NewLine;
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            refreshList();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                sql_commands.DeleteTable(listBox1.SelectedItem.ToString());
            }
            // DROP TABLE IF EXISTS table2;
            refreshLoad();
        }

        private static void refreshList()
        {
            addUtilities = new AddUtilities();
            addUtilities.Show();
        }
        private void refreshLoad()
        {
            listBox1.Items.Clear();
            foreach(string a in sql_commands.GetTables())
            {
                listBox1.Items.Add(a);
            }
         //   listBox1.Items.Clear();
         //   SQLiteCommand command = new SQLiteCommand(connection);
         //   connection.Open();
         //   command.CommandText = "SELECT tbl_name FROM sqlite_master";
         //   using (SQLiteDataReader rdr = command.ExecuteReader())
         //   {
         //       while (rdr.Read())
         //       {
         //           //  Console.Write("{0} ", rdr[0]);
         //           listBox1.Items.Add(rdr[0].ToString());
         //          //treeView1.Nodes[0].Nodes.Add("111");
         //       }
         //       rdr.Close();
         //   }
         //   command.Dispose();
         //   connection.Close();
        }
    }
}

    