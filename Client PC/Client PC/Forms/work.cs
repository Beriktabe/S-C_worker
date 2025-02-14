using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Beriktabe.SqlWorker;
using Beriktabe.Settings;
using System.IO;
using System.Xml.Linq;
using Beriktabe.TcpWorker;
using System.Net.Sockets;

namespace Client_PC.Forms
{
    public partial class work : Form
    {
        #region vars
        public WorkList WorkLst = new WorkList();
        string item;
        static string Data;
        static StringDialog StrDial;

        Dictionary<string, string> dic = new Dictionary<string, string>();
        Dictionary<string, pair> LoadDict = new Dictionary<string, pair>();
        #endregion
        public struct pair
        {
            public string data;
            public string type;
        }

        public work(string name)
        {
            InitializeComponent();
            item = name;
            label1.Text = item; 
            if(File.Exists(Settings.XmlPath) == false)
            {
                XDocument doc2 = new XDocument();
                XElement root = new XElement("root");           
                doc2.Add(root);
                doc2.Save(Settings.XmlPath);
            }
        }

        private void work_Load(object sender, EventArgs e)
        {
            sql_commands.ConnectToFile(Settings.path);
            string[] data1;
            string[] data2;
            sql_commands.GetAll(item,out data1,out data2);
            for(int a = 0; a != data1.Length; a++)
            {
                comboBox1.Items.Add(data1[a]);
                dic.Add(data1[a], data2[a]);
            }
        }
        public static void setData(string p1)
        {
            Data = p1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pt;
            if (comboBox1.SelectedItem != null)
            {
                if (textBox1.Text == "фото")
                {
                    openFileDialog1.Filter = "фото файлы(*.png; *.jpg; *.jpeg) | *.png; *.jpg; *.jpeg | Все файлы(*.*) | *.* ";
                    if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return;
                    pt = Path.GetFileName(openFileDialog1.FileName);
                    dataGridView1.Rows.Add(comboBox1.SelectedItem.ToString(), Path.GetFileName(openFileDialog1.FileName));
                }
                else
                {
                    StrDial = new StringDialog();
                    StrDial.ShowDialog();
                    //richTextBox1.Text += comboBox1.SelectedItem.ToString() + Environment.NewLine;
                    //richTextBox2.Text += Data + Environment.NewLine;
                    pt = Data;
                    dataGridView1.Rows.Add(comboBox1.SelectedItem.ToString(), Data);
                }

                LoadDict.Add(comboBox1.SelectedItem.ToString(), new pair { data = pt, type = textBox1.Text });
                comboBox1.Items.Remove(comboBox1.SelectedItem);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            save();
            TcpClient client = new TcpClient(Settings.ip, Settings.port);
            TcpWorker a = new TcpWorker();
            a.netStream = client.GetStream();
            a.Send("Recieve");
            a.SendFile(Settings.XmlPath);
            a.CloseStream();
            client.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            textBox1.Text = dic[comboBox1.SelectedItem.ToString()];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            save();
        }
        private void save()
        {
            XDocument doc = XDocument.Load(Settings.XmlPath);

            XElement element = new XElement("name", new XAttribute("value", label1.Text));
            foreach (KeyValuePair<string, pair> rw in LoadDict)
            {
                element.Add(new XElement(rw.Key, new XAttribute("value", rw.Value.data), new XAttribute("type", rw.Value.type)));
            }

            doc.Root.Add(element);
            doc.Save(Settings.XmlPath);

            MessageBox.Show("Writed");
        }
    }
}
