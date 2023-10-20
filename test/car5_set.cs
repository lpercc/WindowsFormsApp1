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
    public partial class car5_set : Form
    {
        Form1 f1;
        public car5_set(Form1 F1)
        {
            InitializeComponent();
            f1 = F1;
        }

        private void btncar5set_Click(object sender, EventArgs e)
        {

            MessageBox.Show("设置成功");
            this.Close();
        }
    }
}
