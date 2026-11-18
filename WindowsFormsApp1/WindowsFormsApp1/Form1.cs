using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if( radioButton1.Checked)
            {
                radioButton2.Checked= false;
                labelStartInterval.Visible = false;
                textBox2.Visible = false;
                labelEndInterval.Visible = false;
                textBox3.Visible = false;
                labelStep.Visible = false;
                textBox4.Visible = false;
            }
        }
    }
}
