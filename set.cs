using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class set : Form
    {
        Form1 f1;
        public set(Form1 F1)
        {
            InitializeComponent();
            f1 = F1;
            //textBox1.Text = f1.kepIP;
            //numericUpDown1.Value = f1.port;
            //numericUpDown2.Value = f1.keptimer1.Interval;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                //f1.kepIP = textBox1.Text;
                //f1.port = Convert.ToInt32(numericUpDown1.Value);
                //f1.keptimer1.Interval = Convert.ToInt32(numericUpDown2.Value);
                //try
                //{
                //    if (f1.kepsever == null || f1.kepsever.isrun == false)
                //    {
                //        f1.kepsever = new tcpclient();
                //        f1.kepsever.setOnSetback(f1.kepsever_callback);
                //        f1.kepsever.Client_Connect(f1.kepIP, f1.port);
                //        f1.keptimer1.Start();
                //    }
                //    else
                //    {
                //        f1.keptimer1.Stop();
                //        f1.kepsever.Client_Close();
                //        f1.kepsever.Client_Connect(f1.kepIP, f1.port);
                //        f1.keptimer1.Start();
                //    }
                //    MessageBox.Show("设置成功");
                //    f1.saveini();
                //    this.Close();
                //}
                //catch(Exception ex)
                //{
                //    MessageBox.Show("设置失败");
                //}
            }
        }
    }
}
