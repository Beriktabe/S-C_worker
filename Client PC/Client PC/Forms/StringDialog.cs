using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_PC.Forms
{
    public partial class StringDialog : Form
    {
        public StringDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            work.setData(richTextBox1.Text);
            this.Dispose();
        }
    }
}
