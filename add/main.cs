using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.add
{
    public partial class main : Form
    {
        Form1 f1;
        public main(Form1 F1)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            f1 = F1;
            bind();
        }
        public void bind()
        {
            dataGridView1.DataSource =device_cmd.ListToDataTable(f1.devices);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add ad = new add(f1, this);
            ad.Show();
        }
        int select = 0;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "delete")
                {
                    device de = f1.devices[e.RowIndex];
                    if (de.nettype == "TCP服务器")
                    {
                        tcpclient tl = f1.tcs.Find(d => { return de.device_type == d.device_type && de.addr == d.addr; });
                        f1.devices.Remove(de);
                        try
                        {
                            if (tl != null)
                            {
                                f1.tcs.Remove(tl);
                                tl.isagain = false;
                                tl.Client_Close();
                            }
                        }
                        catch
                        { }
                    }
                    if (de.nettype == "UDP服务器")
                    {
                        udpclient tl = f1.ucs.Find(d => { return de.device_type == d.device_type && de.addr == d.addr; });
                        f1.devices.Remove(de);
                        try
                        {
                            if (tl != null)
                            {
                                f1.ucs.Remove(tl);
                  
                                tl.close();
                            }
                        }
                        catch
                        { }
                    }
                    if (de.nettype == "UDP客户端")
                    {
                        udpsever us = f1.uds.Find(d => { return de.Lport == d.Port.ToString() && de.device_type == d.device_type; });
                        f1.devices.Remove(de);
                        List<device> des = f1.devices.FindAll(d => { return de.Lport == d.Lport && de.device_type == d.device_type; });
                        if (des.Count == 0)
                        {
                            try
                            {
                                f1.uds.Remove(us);
                   
                                us.close();
                            }
                            catch
                            {

                            }
                        }
                    }
                    if (de.nettype == "TCP客户端")
                    {
                        tcpsever us = f1.tls.Find(d => { return de.Lport == d.Port.ToString() && de.device_type == d.device_type; });
                        f1.devices.Remove(de);
                        if (us != null)
                        {
                            Socket so = us.getclient(de.IP);
                            try
                            {
                                us.removeclient(de.IP);
                                so.Close();
                                so.Dispose();
                            }
                            catch
                            { }

                            List<device> des = f1.devices.FindAll(d => { return de.Lport == d.Lport && de.device_type == d.device_type; });
                            if (des.Count == 0)
                            {
                                try
                                {
                                    f1.tls.Remove(us);
                                    us.stop_sever();
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    kepclient kep = f1.kepsevers.Find(d => { return de.kepsever_ip == d.ip && de.kepsever_port == d.port; });                 
                        if (kep != null)
                        {
                            f1.kepsevers.Remove(kep);
                            kep.isagain = false;
                            kep.Client_Close();
                        }
                    
                    f1.dataGridView1.DataSource = device_cmd.ListToDataTable(f1.devices);
                    bind();
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bhfp bh = new bhfp(f1, select);
            bh.Show();
            bh.bind(f1.devices[select].bhlist);
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    device de = f1.devices[e.RowIndex];
                    if(de.device_type== "北京公科飞达交通应急广播")
                    {
                        select = e.RowIndex;
                        cms.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }
    }
}
