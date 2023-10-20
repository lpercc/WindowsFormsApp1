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
    public partial class lead_set : Form
    {
        Form1 f1;
        public lead_set(Form1 F1)
        {
            InitializeComponent();
            f1 = F1;
            comboBox1.SelectedIndex = f1.leadset-1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f1.leadset = comboBox1.SelectedIndex+1;
            MessageBox.Show("设置成功");
            this.Close();
        }
    }
}
