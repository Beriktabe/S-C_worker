using Client_PC.Forms;
using System;
using System.Windows.Forms;

namespace Client_PC
{
    public partial class Form1 : Form
    {
        public static WorkList WorkLst = new WorkList();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WorkLst.Show();
        }
    }
}
