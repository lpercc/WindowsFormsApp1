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
    public partial class Log : Form
    {

        public string id = "";
        public Log(string Id)
        {
            InitializeComponent();
            id = Id;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (tl != null)
            //{
            //    try
            //    {
            //        richTextBox1.Text = tl.log.ToString();
            //    }
            //    catch
            //    {
            //        richTextBox1.Text = "";
            //        tl.log.Clear();
            //    }
            //}
            //if (us != null)
            //{
            //    try
            //    {
            //        richTextBox1.Text = us.log.ToString();
            //    }
            //    catch
            //    {
            //        richTextBox1.Text = "";
            //        us.log.Clear();
            //    }
            //}
            //if (uc != null)
            //{
            //    try
            //    {
            //        richTextBox1.Text = "";
            //        richTextBox1.Text = uc.log.ToString();
            //    }
            //    catch
            //    {
            //        richTextBox1.Text = "";
            //        uc.log.Clear();
            //    }
            //}
            //if (kep != null)
            //{
            //    try
            //    {
            //        richTextBox1.Text = kep.log.ToString();
            //    }
            //    catch
            //    {
            //        richTextBox1.Text = "";
            //        kep.log.Clear();
            //    }
            //}
        }
        public delegate void addtextback(string text);
        public void addtext(string text)
        {
            addtextback add1 = delegate (string txt)
            {
                if (!isstop)
                {
                    if (richTextBox1.TextLength > 2047483647)
                    {
                        richTextBox1.Text = "";
                    }
                    DateTime now = DateTime.Now;
                    richTextBox1.Text = "[" + now.ToString() + "]" + txt + richTextBox1.Text;
                }
            };
            if (!IsDisposed && !isstop && (add1 !=null) && (text!=null))    //htj:原来没有if，关闭时有异常
            {
                try //htj:有异常，加入
                {
                    Invoke(add1, text);
                }
                catch
                {

                }

            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (button1.Text == "暂停显示")
            //    {
            //        richTextBox1.SelectionStart = richTextBox1.Text.Length;
            //        richTextBox1.ScrollToCaret();
            //    }
            //}
            //catch
            //{
            //    if (tl != null)
            //    {

            //        richTextBox1.Text = "";
            //        tl.log.Clear();

            //    }
            //    if (us != null)
            //    {

            //        richTextBox1.Text = "";
            //        us.log.Clear();

            //    }
            //    if (uc != null)
            //    {

            //        richTextBox1.Text = "";
            //        uc.log.Clear();

            //    }
            //    if (kep != null)
            //    {

            //        richTextBox1.Text = "";
            //        kep.log.Clear();

            //    }
            //}
        }

        private void Log_FormClosing(object sender, FormClosingEventArgs e)
        {
            //htj，服务器日志关闭偶尔会异常。修改为先停止，并清空
            timer1.Stop();
            isstop = true;
            richTextBox1.Text = "";
        }
        bool isstop = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "暂停显示")
            {
               // timer1.Stop();
                button1.Text = "继续显示";
                isstop = true;
            }
            else
            {
               // timer1.Start();
                button1.Text = "暂停显示";
                isstop = false;
            }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
