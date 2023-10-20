using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.test
{
    public partial class Information2 : Form
    {
        Form1 f1;
        public Information2(Form1 F1)
        {
            InitializeComponent();
            f1 = F1;
            comboBox1.SelectedIndex = Convert.ToInt32(f1.vmsset.Substring(0, 4), 16);
            numericUpDown1.Value = Convert.ToInt32(f1.vmsset.Substring(4, 4), 16);
            textBox1.Text = Class1.ZHTOAC(Class1.ZH16(f1.vmsset.Substring(8, 8)));
            numericUpDown2.Value = f1.timer2.Interval;
            numericUpDown3.Value = f1.vmsout;
        }

        private void button1_Click(object sender, EventArgs e)
        { string vmsset = f1.vmsset;

            try
            {
               
                if (textBox1.Text != "")
                {
                    f1.vmsset = Class1.ZHTO16(comboBox1.SelectedIndex, 2);
                    f1.vmsset += Class1.ZHTO16(Convert.ToInt32(numericUpDown1.Value), 2);
                    string cmd = textBox1.Text;
                    for (int i = cmd.Length; i < 4; i++)
                    {
                        cmd = " " + cmd;
                    }
                    f1.vmsset += Class1.ZHTO16(Class1.ZHAC(cmd));
                    f1.timer2.Interval = Convert.ToInt32(numericUpDown2.Value);
                    f1.vmsout = Convert.ToInt32(numericUpDown3.Value);
                    MessageBox.Show("设置成功");
                    this.Close();
                }
            }
            catch
            { f1.vmsset = vmsset;
                MessageBox.Show("设置错误");
               
            }
        }
    }
}
