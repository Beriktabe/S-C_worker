using System;
using System.Data;
using System.Windows.Forms;
using Classes;
using Forms;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Server_PC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static ListUtilities listUtilities;
        //Thread srv;
        Task srv;
        static Server s;

        private void button1_Click(object sender, EventArgs e)
        {
            listUtilities = new ListUtilities();
            listUtilities.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if(Settings.Started == false)
            {
                srv = new Task(new Action(delegate { s = new Server(); }));
                srv.Start();

                StateUpdate();
                button2.Text = "Выключить";
                label1.Text = "Состояние: Включен";
                Settings.Started = true;
            }
            else
            {
                s.Dispose();
                srv.Dispose();
                StateUpdate();
                button2.Text = "Включить";
                Settings.Started = false;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            StateUpdate();
        }
        private void StateUpdate()
        {
            label1.Text = "Состояние: " + Settings.state;
            label3.Text = "port: " + Settings.port;
            label5.Text = "Клиентов: " + Settings.count + "//Bad work";
        }
    }
}
