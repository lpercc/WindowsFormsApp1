using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.add
{
    public partial class bhfp : Form
    {
        DataTable dt = new DataTable();
        Form1 f1;
        public int myindex;
        public bhfp( Form1 F1,int Myindex)
        {
            InitializeComponent();
            myindex = Myindex;
            f1 = F1;
            dt.Columns.Add("index");
            dt.Columns.Add("bh");
            dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].ReadOnly = true;
        }
        public void bind(ArrayList bhlist)
        {
            dt.Rows.Clear();
            for (int i = 0; i < 50; i++)
            {
                dt.Rows.Add(i + 1, bhlist[i]);
            }
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArrayList bhlist = new ArrayList();
            for (int i = 0; i < 50; i++)
            {
                bhlist.Add((i+1)+"");
            }
            bind(bhlist);
        }
        public void set()
        {
            try
            {
                ArrayList bhlist = new ArrayList();
                for (int i = 0; i < 50; i++)
                {
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString() != "")
                    {

                        if (bhlist.IndexOf(int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString())+"") < 0)
                        {
                            bhlist.Add(int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString())+"");
                        }
                        else
                        {
                            MessageBox.Show("有相同的分机号，请重新设置!");
                            return;
                        }
                    }
                    else
                    {
                        bhlist.Add("");
                    }
                }
                f1.devices[myindex].bhlist = bhlist;
                f1.saveini();  
                this.Close();
            }
            catch
            {
                MessageBox.Show("设置错误");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            set();
        
        }
    }
}
