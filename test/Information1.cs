using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.parameter;

namespace WindowsFormsApp1.test
{
    public partial class Information1 : Form
    {
        Form1 f1;
        public Information1(Form1 F1)
        {
            InitializeComponent();
            f1 = F1;
            textBox1.Text = f1.ifd.speed_limit;
            textBox2.Text = f1.ifd.typeface;
            textBox3.Text = f1.ifd.red;
            textBox4.Text = f1.ifd.light;
            textBox5.Text = f1.ifd.light_regulate;
            textBox7.Text = f1.ifd.onoff;
            textBox6.Text = f1.timer1.Interval+"";

        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.') e.Handled = true;
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1) e.Handled = true;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Infomation1_data ifd = f1.ifd;
            try
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "")
                {
                    f1.ifd.speed_limit = textBox1.Text;
                    f1.ifd.typeface = textBox2.Text;
                    f1.ifd.red = textBox3.Text;
                    f1.ifd.light = textBox4.Text;
                    f1.ifd.light_regulate = textBox5.Text;
                    f1.ifd.onoff = textBox7.Text;
                    f1.timer1.Interval = int.Parse(textBox6.Text);
                    f1.saveini();
                    MessageBox.Show("设置成功");
                    this.Close();
                }
            }
            catch
            {  f1.ifd = ifd;
                MessageBox.Show("设置错误");
              
            }
        }
    }
}
