using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MS_Paint_Code_Writter
{
    public partial class MyPopUp : Form
    {
        public MyPopUp(string message)
        {
            InitializeComponent();
            timer1.Enabled = true;
            label2.Text = message;
        }

        private void MyPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Activate();
        }
    }
}
