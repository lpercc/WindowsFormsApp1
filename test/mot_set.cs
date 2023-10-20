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
    public partial class mot_set : Form
    {
        Form1 f1;
        public mot_set(Form1 F1)
        {
            InitializeComponent();
            f1 = F1;
            numericUpDown1.Value = f1.mot_timecs / (60 * 1000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f1.mot_timecs =(int)numericUpDown1.Value * 60 * 1000;
            var tc = f1.tcs.FindAll(d => { return "上海勋飞微波车检器" == d.device_type; });
            for(int i=0;i<tc.Count;i++)
            {
                if (tc[i].isrun)
                {
                    tc[i].timer.Change(f1.mot_timecs / 2, f1.mot_timecs / 2);
                }
            }
            MessageBox.Show("设置成功");
            this.Close();
        }
    }
}
