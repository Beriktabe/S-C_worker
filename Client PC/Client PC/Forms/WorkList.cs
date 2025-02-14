using System;
using System.Windows.Forms;
using System.Data;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Beriktabe.TcpWorker;
using Beriktabe.SqlWorker;
using Beriktabe.Settings;

namespace Client_PC.Forms
{
    public partial class WorkList : Form
    {
        #region vars
        private static NetworkStream netStream;
        private static TcpClient client;
        private static Form1 mainForm = new Form1();
        private static work workForm;
        public string item;
        Task get;
        private static bool started = false;
        private static TcpWorker worker = new TcpWorker();
        #endregion
        public WorkList()
        {
            InitializeComponent();
        }
        private void WorkList_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            mainForm.Activate();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if(listView1.FocusedItem != null)
            {
                workForm = new work(listView1.FocusedItem.Text);
                workForm.Show();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if(started == false)
            {
                get = new Task(new Action(Rec));
                get.Start();
                get.Wait();
            }
            else
            {
                MessageBox.Show("Процесс запущен, подождите");
                get.Wait();
                MessageBox.Show("Процесс выполнен");
            }
        }

        public static void Rec()
        {
            started = true;
            client = new TcpClient(Settings.ip, Settings.port);
            netStream = client.GetStream();

            worker.netStream = netStream;
            worker.Send("Get db");
            //Environment.CurrentDirectory + "\\", "Services_db.db"

            worker.ReceiveFile(Environment.CurrentDirectory + "\\BD\\", "Services_db.db");
               
            client.Close();
            worker.CloseStream();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            if (File.Exists(Settings.path))
            {
                sql_commands.ConnectToFile(Settings.path);
                string[] bd = sql_commands.GetTables();
                foreach (string a in bd)
                    listView1.Items.Add(a);
            }
            else
            {
                MessageBox.Show("Нет файла бд");
            }  
        }

        public static void ReceiveFile(string path, string filename, bool rewrite = true)
        {
            string a1 = path + filename;
            byte[] RecData = new byte[1024];
            int RecBytes;
                if (a1 != string.Empty)
                {
                    int totalrecbytes = 0;
                    if (File.Exists(a1))
                        File.Delete(a1);
                    FileStream Fs = new FileStream(a1, FileMode.CreateNew, FileAccess.Write);
                    while ((RecBytes = netStream.Read(RecData, 0, RecData.Length)) > 0)
                    {
                        Fs.Write(RecData, 0, RecBytes);
                        totalrecbytes += RecBytes;
                    }
                    Fs.Close();
                }
            netStream.Close();
            netStream.Dispose();
        }
    }
}
