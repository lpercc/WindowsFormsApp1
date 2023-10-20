using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsApp1.add;
using WindowsFormsApp1.parameter;
using WindowsFormsApp1.test;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
       public List<kepclient> kepsevers=new List<kepclient>();
        //public string kepIP="";
        //public int port = 5000;
        public int max = 5;
        public Form1()
        {
            InitializeComponent();
           // string ss = device_cmd.view_write_set_gb("中", "FF");
            dataGridView1.AutoGenerateColumns = false;
            ifd.light = "00";
            ifd.light_regulate = "0";
            ifd.red = "0";
            ifd.speed_limit = "000";
            ifd.typeface = "0";
            ifd.onoff = "0";

            this.Text = Program.appName;
        }
     
        public Infomation1_data ifd = new Infomation1_data();
        public string vmsset = "0000" + "0010" + Class1.ZHTO16(Class1.ZHAC(" a01"));
        public int vmsout = 3;
        public int leadset = 1, LAMPset=0;
        public int mot_timecs = 60 * 1000 * 6;

        public List<device> devices = new List<device>();
        public List<tcpclient> tcs = new List<tcpclient>();
        public List<tcpsever> tls = new List<tcpsever>();
        public List<udpsever> uds = new List<udpsever>();
        public List<udpclient> ucs = new List<udpclient>();
        public void addlg(string text,string id)
        {
            if(lg!=null && !lg.IsDisposed && lg.id==id && text!=null)   //htj：关闭时出现异常，加入text!=null
            {
                lg.addtext(text);
            }
        }
        public void addlog(string text, string id)
        {
            if (log != null && !log.IsDisposed && log.id == id && text != null)   //htj：关闭时出现异常，加入text!=null
            {
                log.addtext(text);
            }
        }
        public void kepsever_callback(string cmd, string data, kepclient tl)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    kepclient.Setback d = new kepclient.Setback(kepsever_callback);
                    Invoke(d, cmd, data, tl);
                }
                else
                {  
                    if (cmd == "Connectting")
                    {
                        for (int i = 0; i < devices.Count; i++)
                        {
                            if (devices[i].kepsever_ip == tl.ip && devices[i].kepsever_port == tl.port && devices[i].id==tl.id)
                            {
                                devices[i].kepsever_state= "在线";  
                                addlg("服务器连接成功\r\n", devices[i].id);
                                if(devices[i].device_type== "上海勋飞微波车检器")
                                {
                                    tcpclient tc = tcs.Find(d => { return devices[i].device_type == d.device_type && devices[i].addr == d.addr&&d.isrun; });
                                    if (tc == null)
                                    {
                                        string cmdstr = device_cmd.write10(devices[i].id, "0000", devices[i].addr + "0000");
                                        if (!tl.send(cmdstr, true))
                                        {
                                            addlg("发送服务器失败", devices[i].id);
                                            return;
                                        }
                                        addlg("从站地址：" + devices[i].id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", devices[i].id);
                                    }
                                    else
                                    {
                                        string cmdstr = device_cmd.write10(devices[i].id, "0000", devices[i].addr + "0001");
                                        if (!tl.send(cmdstr, true))
                                        {
                                            addlg("发送服务器失败", devices[i].id);
                                            return;
                                        }
                                        addlg("从站地址：" + devices[i].id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", devices[i].id);
                                    }
                                }
                            }
                        }
                        dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
                    
                    }
                   
                    if (cmd == "Receive")
                    {


                    }
                    if (cmd == "Receive16")
                    {

                        if (data.Length>=12&&data.Substring(0,8)=="12360000")
                        {
                            int len = Convert.ToInt32(data.Substring(8, 4), 16);
                            if(data.Length>=12+len)
                            {    
                                string id = data.Substring(12, 2);
                                addlg("从站地址：" + id + "服务器接收数据(HEX)：" + data+"\r\n",id);
                            
                                if(data.Substring(14, 2)=="03")
                                {
                                    tcpclient tc = tcs.Find(d => { return id == d.id; });
                                    device de = devices.Find(d => { return id == d.id; });
                                    udpclient uc=ucs.Find(d => { return id == d.id; });
                                    if (tc!=null)
                                    {
                                      
                                        if (tc.device_type == "消防火灾")
                                        {
                                            tc.am = data.Substring(18);
                                            addlg("从站地址：" + tc.id + "获取数据（回读字段）" + tc.am + "\r\n",id);
                                        }
                                        if (tc.device_type == "车辆检测器")
                                        {
                                            tc.am = data.Substring(18);
                                            addlg("从站地址：" + tc.id + "获取数据（回读字段）" + tc.am + "\r\n",id);
                                        }
                                    }
                                    if (de != null)
                                    {
                                        if (de.device_type == "丰海科技情报板")
                                        {
                                            addlg("从站地址：" + de.id + " 获取数据(配置字段)" + data.Substring(18, 24) + "\r\n", id);

                                            if (data.Substring(18, 24) != de.aw)
                                            {
                                                tcpsever ts = tls.Find(d => { return de.device_type == d.device_type; });
                                                //  tc.log.Append("不一致\r\n");
                                                Socket so = ts.getclient(de.IP);
                                                if (ts.isrun && so != null)
                                                {
                                                    de.aw = data.Substring(18, 24);
                                                    Infomation1_data info = device_cmd.Information1_readaw(de.aw, de.addr);
                                                    if (device_cmd.Information1_write(info, so))
                                                    {
                                                        byte[] by = Class1.ZH16(device_cmd.db(info.addr16, "24", info.speed_limit + info.typeface + info.red + "0" + info.light_regulate + info.light));
                                                        addlog("末端设备地址：" + de.addr + "发送(配置参数)" + Class1.ZHTO16(by) + "(" + Class1.ZHTOAC(by) + ")\r\n", id);
                                                        byte[] by1 = Class1.ZH16(device_cmd.db(info.addr16, "55", info.onoff));
                                                        addlog("末端设备地址：" + de.addr + "发送(显示开关)" + Class1.ZHTO16(by1) + "(" + Class1.ZHTOAC(by1) + ")\r\n", id);
                                                    }
                                                }
                                                else
                                                {
                                                    addlog("末端设备地址：" + de.addr + "不在线" + "\r\n", id);
                                                }
                                            }

                                            de.am = data.Substring(42, 36);
                                            addlg("从站地址：" + de.id + " 获取数据(回读字段)" + de.am + "\r\n", id);
                                        }
                                        if (de.device_type == "应急电话")
                                        {
                                            udpsever us = uds.Find(d => { return de.device_type == d.device_type; });


                                            de.am = data.Substring(18);

                                            addlg("从站地址：" + tc.id + "获取数据（回读字段）" + de.am + "\r\n", id);
                                        }
                                        if (de.device_type == "交通诱导灯")
                                        {
                                           
                                            addlg("从站地址：" + de.id + " 获取数据(配置字段)" + data.Substring(18, 4) + "\r\n", id);
                                            if (data.Substring(18, 4) != de.aw)
                                            {
                                                tcpsever ts = tls.Find(d => { return de.device_type == d.device_type; });
                                                Socket so = ts.getclient(de.IP);
                                                if (ts.isrun && so != null)
                                                {
                                                    if (data.Substring(18, 4) != "0000")
                                                    {
                                                        de.aw = data.Substring(18, 4);
                                                    }
                                                    try
                                                    {
                                                        byte[] by = Class1.ZH16(device_cmd.lead_write_setlight(Convert.ToInt32(de.aw)));
                                                        so.Send(by);
                                                        addlog("末端设备地址：" + de.addr + "发送(配置参数)" + Class1.ZHTO16(by) + "(" + Class1.ZHTOAC(by) + ")\r\n", id);
                                                    }
                                                    catch
                                                    { }
                                                }
                                            }
                                        }
                                        if (de.device_type == "投影灯")
                                        {
                                            addlg("从站地址：" + de.id + " 获取数据(配置字段)" + data.Substring(18, 4) + "\r\n", id);
                                            if (data.Substring(18, 4) != de.aw)
                                            {
                                                tcpsever ts = tls.Find(d => { return de.device_type == d.device_type; });
                                                Socket so = ts.getclient(de.IP);
                                                if (ts.isrun && so != null)
                                                {
                                                    de.aw = data.Substring(18, 4);
                                                    try
                                                    {
                                                        byte[] by = Class1.ZH16(device_cmd.LAMP_write_setlight(Convert.ToInt32(de.aw)));
                                                        so.Send(by);
                                                        addlog("末端设备地址：" + de.addr + "发送(配置参数)" + Class1.ZHTO16(by) + "(" + Class1.ZHTOAC(by) + ")\r\n", id);
                                                    }
                                                    catch
                                                    { }
                                                }
                                            }
                                        }
                                        if (de.device_type == "唯的美情报板")
                                        {
                                            addlg("从站地址：" + de.id + " 获取数据(配置字段)" + data.Substring(18, 4) + "\r\n", id);
                                            //20230906修改
                                            //if (data.Substring(18, 256+4+4) != de.aw)
                                            //20230911修改
                                            //if (data.Substring(18, 280) != de.aw)
                                            if (data.Substring(18, 284) != de.aw)
                                            {
                                               
                                                if (tc!=null&&tc.isrun)
                                                {
                                                    tc.timer.Change(-1, -1);
                                                    //20230906修改
                                                    //string aw = data.Substring(18, 256 + 4 + 4);
                                                    //20230911修改
                                                    //string aw = data.Substring(18, 280);
                                                    string aw = data.Substring(18, 284);
                                                    bool mynew = false;
                                                    if (aw.Substring(0,4) == "0000")
                                                    {
                                                      
                                                        tl.send(device_cmd.write10(id, "0000", "0001"+aw.Substring(4)), true);
                                                        aw = "0001" + aw.Substring(4);
                                                        mynew = true;
                                                    }
                                            
                                                    try
                                                    {


                                                        if (aw.Substring(0, 4) == "00FF")
                                                        {
                                                            //原正确代码
                                                            ///int gb32len = Convert.ToInt32(aw.Substring(4, 4), 16);
                                                            //string gb32 = Class1.ZHTOGB(Class1.ZH16(aw.Substring(8, gb32len * 2))).Replace("\0", "").Trim();
                                                            //tc.send(device_cmd.view_write_set_gb(gb32, de.cardid), true);
                                                            //addlog("末端设备地址：" + de.addr + "发送(配置参数)" + "(" + gb32 + ")\r\n", id);

                                                            ////20230906根据粟工的代码修改
                                                            //int gb32len = Convert.ToInt32(aw.Substring(4, 4), 16); //取出待显字符长度
                                                            //byte[] type16 = Class1.ZH16(aw.Substring(8, gb32len * 4)); //转换出16进制数
                                                            //string gb321 = Class1.ZHTOGB(type16);//16进制转字符串
                                                            //type16 = Class1.ZH16(gb321);
                                                            //string gb32 = Class1.ZHTOGB(type16).Replace("\0", "").Trim();
                                                            ////string gb32 = Class1.ZHTOGB(Class1.ZH16(aw.Substring(8, gb32len * 2))).Replace("\0", "").Trim();
                                                            //tc.send(device_cmd.view_write_set_gb(gb32, de.cardid), true);
                                                            ////tc.send(device_cmd.view_write_set_gb(type16, de.cardid), true);
                                                            //addlog("末端设备地址：" + de.addr + "发送(配置参数)" + "(" + gb32 + ")\r\n", id);
                                                            ////end


                                                            //20230911修改增加
                                                            string hSiz = aw.Substring(279 + 4, 1);

                                                            //20230906根据粟工的代码再修改
                                                            string op = aw.Substring(279, 1); //取颜色控制字符
                                                            //这里有可能是传GB2312  这里写待显不字符
                                                            int gb32len = Convert.ToInt32(aw.Substring(4, 4), 16); //取出待显字符长度
                                                            byte[] type16 = Class1.ZH16(aw.Substring(8, gb32len * 4)); //转换出16进制数
                                                            string gb321 = Class1.ZHTOGB(type16);//16进制转字符串
                                                            type16 = Class1.ZH16(gb321);
                                                            string gb32 = Class1.ZHTOGB(type16).Replace("\0", "").Trim();
                                                            //20230911修改
                                                            //tc.send(device_cmd.view_write_set_gb_RGY(gb32, de.cardid, op), true);
                                                            tc.send(device_cmd.view_write_set_gb_RGY(gb32, de.cardid, op, hSiz), true);
                                                            //tc.send(device_cmd.view_write_set_gb(gb32, de.cardid), true);
                                                            addlog("末端设备地址：" + de.addr + "发送(配置参数)" + "(" + gb32 + ")\r\n", id);
                                                        }
                                                        else
                                                        {
                                                            if (mynew || aw.Substring(0, 4) != de.aw.Substring(0, 4) || data.Substring(256 + 4 + 4 + 18 + 8, 4) != de.aw.Substring(0, 4))
                                                            {
                                                                byte[] by = Class1.ZH16(device_cmd.view_write_set(aw.Substring(2, 2), de.cardid));
                                                                tc.send(device_cmd.view_write_set(aw.Substring(2, 2), de.cardid), true);
                                                                addlog("末端设备地址：" + de.addr + "发送(配置参数)" + Class1.ZHTO16(by) + "(" + Class1.ZHTOAC(by) + ")\r\n", id);
                                                            }
                                                        }
                                                        de.aw =aw;                                                                                          
                                                    }
                                                    catch
                                                    { }
                                                 
                                                    tc.timer.Change(5000,5000);
                                                }
                                            }
                                            de.am = data.Substring(256+4+4+18);
                                        }
                                        if (de.device_type == "上海勋飞微波车检器")
                                        {
                                            de.am = data.Substring(18);
                                            tc.am = de.am;
                                            addlg("从站地址：" + tc.id + "获取数据（回读字段）" + tc.am + "\r\n", id);
                                        }
                                        if (de.device_type == "天津五维地磁车检器")
                                        {
                                            de.am = data.Substring(18);
                                            //htj 原先代码中tc为空，异常，注释掉
                                            //addlg("从站地址：" + tc.id + "获取数据（回读字段）" + tc.am + "\r\n", id);

                                            //htj
                                            tcpsever ts = tls.Find(d => { return de.device_type == d.device_type; });
                                            Socket so = ts.getclient(de.IP);
                                            if (ts.isrun && so != null)
                                            {
                                            }
                                            addlg("从站地址：" + de.id + "获取数据（回读字段）" + de.am + "\r\n", id);
                                        }
                                    }
                                    if (uc != null)
                                    {
                                        if (uc.device_type == "可变信息标志")
                                        {
                                            string aw = data.Substring(18, 16);
                                            addlg("从站地址：" + de.id + " 获取数据(配置字段)" + aw + "\r\n", id);

                                            if (aw != uc.aw)
                                            {
                                                //  addlog("不一致\r\n");
                                                if (uc.isrun)
                                                {
                                                    if (aw == "0000 0000 0000 0000".Replace(" ", ""))
                                                    {
                                                        aw = uc.aw;
                                                        tl.send(device_cmd.write10(id, "0000", aw), true);
                                                    }
                                                    uc.aw = aw;
                                                    de.aw = uc.aw;
                                                    string lighttype = device_cmd.VMS_write_setlighttype(Convert.ToInt32(aw.Substring(0, 4), 16), uc.addr);
                                                    string light = device_cmd.VMS_write_setlight(Convert.ToInt32(aw.Substring(4, 4), 16), uc.addr);
                                                    string bmp = device_cmd.VMS_write_bmp(Class1.ZHTOAC(Class1.ZH16(aw.Substring(8, 8))), uc.addr);
                                                    uc.send(lighttype, uc.IP, uc.PORT + "", true);

                                                    addlog("末端设备地址：" + uc.addr + "发送数据（亮度调节）" + lighttype + "(" + Class1.ZHTOAC(Class1.ZH16(lighttype)) + ")\r\n", id);
                                                    uc.send(light, uc.IP, uc.PORT + "", true);
                                                    addlog("末端设备地址：" + uc.addr + "发送数据（亮度）" + light + "(" + Class1.ZHTOAC(Class1.ZH16(light)) + ")\r\n", id);
                                                    uc.send(bmp, uc.IP, uc.PORT + "", true);
                                                    addlog("末端设备地址：" + uc.addr + "发送数据（图片）" + bmp + "(" + Class1.ZHTOAC(Class1.ZH16(bmp)) + ")\r\n", id);
                                                    tl.send(device_cmd.write10(id, "0008", aw.Substring(8, 8)), true);

                                                }
                                                else
                                                {
                                                    addlog("设备不在线\r\n", id);
                                                }
                                            }
                                            de.am = data.Substring(34);
                                        }
                                        if (uc.device_type == "北京公科飞达交通应急广播")
                                        {
                                            de.am = data.Substring(18);

                                            addlg("从站地址：" + de.id + "获取数据（回读字段）" + de.am + "\r\n", id);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (cmd == "掉线"|| cmd == "Connect_error")
                    {
                       
                   
                        if (!tl.isagain)
                        {
                            for (int i = 0; i < devices.Count; i++)
                            {
                                if (devices[i].kepsever_ip == tl.ip && devices[i].kepsever_port == tl.port)
                                {
                                    devices[i].kepsever_state = "不在线";
                                    addlg("服务器不在线\r\n", devices[i].id);
                                }
                            }
                          
                        }
                        else
                        {
                            for (int i = 0; i < devices.Count; i++)
                            {
                                if (devices[i].kepsever_ip == tl.ip && devices[i].kepsever_port == tl.port)
                                {
                                    devices[i].kepsever_state = "连接中...";
                                    addlg("服务器重连中...\r\n", devices[i].id);
                                }
                            }
                            tl.Client_Connect(tl.ip, tl.port);
                           
                        }
                        dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
                    }
                }
            }
            catch
            {

            }
        }
        public void tcpsever_callback(string cmd, string data, string ip, string port, tcpsever ts)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    tcpsever.Setback d = new tcpsever.Setback(tcpsever_callback);
                    Invoke(d, cmd, data,  ip,  port, ts);
                }
                else
                {
                    // device de = devices.Find(d => { return ts.device_type == d.device_type &&ip==d.IP&&port==d.myport.ToString(); });
                    device de = devices.Find(d => { return ts.device_type == d.device_type && ip == d.IP; });
                    de.myport = port;
                    if (cmd == "add")
                    {
                        if (de != null)
                        {
                            de.netstate = "连接成功";
                            kepclient kepsever = kepsevers.Find(d => { return de.kepsever_ip == d.ip && de.kepsever_port == d.port && de.id == d.id; });
                            if (de.device_type == "交通诱导灯")
                            {
                                string cmdstr = device_cmd.write10(de.id, "0001", "0001" + de.addr);
                                if (!kepsever.send(cmdstr, true))
                                {
                                      addlg("发送服务器失败",de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "投影灯")
                            {
                                string cmdstr = device_cmd.write10(de.id, "0001", "0001" + de.addr);
                                if (!kepsever.send(cmdstr, true))
                                {
                                      addlg("发送服务器失败",de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "天津五维地磁车检器")
                            {
                                string cmdstr = device_cmd.write10(de.id, "0000", "0001" + de.addr);
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
                        }
                    }

                    if (cmd == "Receive")
                    {


                    }
                    if (cmd == "Receive16")
                    {
                      
                        if (de != null)
                        {
                            string id = de.id;  
                            addlog("末端设备地址：" + de.addr + ",接收数据：" + data + "("+Class1.ZHTOAC(Class1.ZH16(data))+")\r\n",id);
                            kepclient kepsever = kepsevers.Find(d => { return de.kepsever_ip == d.ip && de.kepsever_port == d.port && de.id==d.id; });
                            if (de.device_type == "丰海科技情报板")
                            {
                                Infomation1_readdata info = device_cmd.Information1_read(data);
                                if (info != null)
                                {
                                    string bm1 = device_cmd.Infomation1_bm(info, de.addr);
                                    addlog("末端设备地址：" + de.addr + ",接收数据(回读参数)：" + bm1 + "\r\n",id);
                                    if (de.am == "" || de.am.Substring(4) != bm1)
                                    {
                                      //  addlog("不一致\r\n");
                                        de.bm_all.Add(bm1);

                                        if (de.am != "" && kepsever != null && kepsever.isrun)
                                        {
                                            for (int i = 0; i < de.bm_all.Count; i++)
                                            {
                                                string bm = de.bm_all[i].ToString();

                                                string cmdstr = device_cmd.write10(de.id, "0006", "0001" + bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                      addlg("发送服务器失败",de.id);
                                                    return;
                                                }

                                              ///  addlog("末端设备地址：" + de.addr + ",发送数据：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n");
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", id);

                                                de.bm_all.RemoveAt(0);
                                                i--;
                                            }

                                        }
                                        else
                                        {
                                            if (de.bm_all.Count > max)
                                            {
                                                de.bm_all.RemoveAt(0);
                                            }
                                        }

                                    }
                                }
                                else { addlog("末端设备地址：" + de.addr + ",接收数据错误\r\n", id); }
                            }
                            if (de.device_type == "天津五维地磁车检器")
                            {
                                car5_readdata c5 = device_cmd.car5_read(data);
                                if (c5 != null)
                                {
                                    if ( kepsever != null && kepsever.isrun)
                                    {
                                        //if (de.addr == c5.bh)   //比较数据的设备编号与配置的设备编号。20230904取消验证（现场不知道设备的编号）
                                        {
                                            int cs = -1;    //车道号基于0，0-4。
                                            if (c5.cd == de.car1)
                                            {
                                                cs = 0;
                                              
                                            }
                                            if (c5.cd == de.car2)
                                            {
                                                cs = 1;
                                               
                                            }
                                            if (c5.cd == de.car3)
                                            {
                                                cs = 2;
                                               
                                            }
                                            if (c5.cd == de.car4)
                                            {
                                                cs = 3;
                                              
                                            }
                                            if (c5.cd == de.car5)
                                            {
                                                cs = 4;                                              
                                            }
                                            if (de.am == "" || de.am.Substring(4,16) != c5.bh)
                                            {   //de.id是从站地址
                                                string cmdstr2 = device_cmd.write10(de.id, "0000", "0001" + c5.bh);     //发送设备编号，服务端要求string。已验证
                                                if (!kepsever.send(cmdstr2, true))
                                                {
                                                    addlg("发送服务器失败", de.id);
                                                    return;
                                                }
                                                addlg("从站地址：" + de.id + ",发送数据：" + cmdstr2 + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr2)) + ")\r\n", de.id);
                                            }
                                            //if (cs > 0)   //htj：旧代码忽略1道（0）
                                            if (cs >= 0)    //htj：改为>=0
                                            {
                                                if (de.am == "" || de.am.Substring(10 * 2 + cs * 16 * 2, 8) != c5.cd)
                                                {
                                                    //string cmdstr1 = device_cmd.write10(de.id, Class1.ZHTO16(5 + 8 * cs, 2), c5.cd);    //发送车道，服务端要求string。(发送的是"313031"字符串，应该发送"101"？）

                                                    //htj：16进制ASCII码转普通字符串
                                                    //string cmdstr1 = device_cmd.write10(de.id, Class1.ZHTO16(5 + 8 * cs, 2), Class1.getASCIItoStr(c5.cd));    //发送车道，服务端要求string。(发送的是"313031"字符串，应该发送"101"？）
                                                    string cmdstr1 = device_cmd.write10(de.id, Class1.ZHTO16(5 + 8 * cs, 2), c5.cd+"00");    //发送车道，服务端要求string。(发送的是"313031"字符串，应该发送"101"？）

                                                    if (!kepsever.send(cmdstr1, true))
                                                    {
                                                        addlg("发送服务器失败", de.id);
                                                        return;
                                                    }
                                                    addlg("从站地址：" + de.id + ",发送数据：" + cmdstr1 + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr1)) + ")\r\n", de.id);
                                                }
                                                if (c5.speed != "")  //行驶速度，已验证。
                                                {
                                                    string cmdstr = device_cmd.write10(de.id, Class1.ZHTO16(5 + 8 * cs + 2, 2), c5.speed);  //发送车速，服务端要求float
                                                    if (!kepsever.send(cmdstr, true))
                                                    {
                                                        addlg("发送服务器失败", de.id);
                                                        return;
                                                    }
                                                    addlg("从站地址：" + de.id + ",发送数据：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                                }
                                                if (c5.ctsj != "")  //车头时距，设备显示是该数据的10倍，已验证。
                                                {
                                                    string cmdstr = device_cmd.write10(de.id, Class1.ZHTO16(5 + 8 * cs + 4, 2), c5.ctsj);   //发送车头时距，服务端要求long
                                                    if (!kepsever.send(cmdstr, true))
                                                    {
                                                        addlg("发送服务器失败", de.id);
                                                        return;
                                                    }
                                                    addlg("从站地址：" + de.id + ",发送数据：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                                }
                                                if (c5.time != "")  //占用时间，设备显示是该数据的10倍，已验证。
                                                {
                                                    string cmdstr = device_cmd.write10(de.id, Class1.ZHTO16(5 + 8 * cs + 6, 2), c5.time);   //发送占用时间，服务端要求long
                                                    if (!kepsever.send(cmdstr, true))
                                                    {
                                                        addlg("发送服务器失败", de.id);
                                                        return;
                                                    }
                                                    addlg("从站地址：" + de.id + ",发送数据：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (cmd == "掉线")
                    {
                        
                        if (de != null)
                        {
                            de.netstate = "未连接";
                            kepclient kepsever = kepsevers.Find(d => { return de.kepsever_ip == d.ip && de.kepsever_port == d.port && de.id == d.id; });
                            if (de.device_type == "丰海科技情报板")
                            {
                                string cmdstr = device_cmd.write06(de.id, "0007", "0000");
                                if (!kepsever.send(cmdstr, true))
                                {
                                      addlg("发送服务器失败",de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "交通诱导灯")
                            {
                                string cmdstr = device_cmd.write06(de.id, "0001", "0000");
                                if (!kepsever.send(cmdstr, true))
                                {
                                      addlg("发送服务器失败",de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "投影灯")
                            {
                                string cmdstr = device_cmd.write06(de.id, "0001", "0000");
                                if (!kepsever.send(cmdstr, true))
                                {
                                      addlg("发送服务器失败",de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "天津五维地磁车检器")
                            {
                                string cmdstr = device_cmd.write06(de.id, "0000", "0000");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            dataGridView1.DataSource = device_cmd.ListToDataTable(devices);

                        }
                    }
                }
            }
            catch
            {

            }
        }

        public void udpsever_callback(string cmd, string data, string ip, string port, udpsever us)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    udpsever.Setback d = new udpsever.Setback(udpsever_callback);
                    Invoke(d, cmd, data, ip, port, us);
                }
                else
                {
                    if (cmd == "打开成功")
                    {
                        List<device> de = devices.FindAll(d => { return us.device_type == d.device_type; });
                        for (int i = 0; i < de.Count; i++)
                        {
                            de[i].netstate = "监听中";
                        }
                    }
                    if (cmd == "打开失败")
                    {
                        List<device> de = devices.FindAll(d => { return us.device_type == d.device_type; });
                        for (int i = 0; i < de.Count; i++)
                        {
                            de[i].netstate = "服务器失连";
                        }
                    }
                    if (cmd == "Receive")
                    {


                    }
                    if (cmd == "Receive16")
                    {
                        string bm1 = device_cmd.phone_read(data); 
                        if (bm1 != "")
                        {
                            device de = devices.Find(d => { return ip == d.IP && bm1.Substring(0, 4) == d.addr; });
                            if (de != null)
                            {
                                kepclient kepsever = kepsevers.Find(d => { return de.kepsever_ip == d.ip && de.kepsever_port == d.port && de.id == d.id; });
                                de.netstate = "监听中";
                                addlog("末端设备地址：" + de.addr + ",接收数据：" + data + "(" + Class1.ZHTOAC(Class1.ZH16(data)) + ")\r\n", de.id);
                                if (de.device_type == "应急电话")
                                {



                                    addlog("末端设备地址：" + de.addr + ",接收数据(回读参数)：" + bm1 + "\r\n", de.id);
                                    if (de.am == "" || de.am.Substring(4) != bm1)
                                    {
                                        de.bm_all.Add(bm1);
                                        //  addlog("不一致\r\n");
                                        if (de.am != "" && kepsever != null && kepsever.isrun)
                                        {

                                            for (int i = 0; i < de.bm_all.Count; i++)
                                            {
                                                string bm = de.bm_all[i].ToString();


                                                string cmdstr = device_cmd.write10(de.id, "0000", "0001" + bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                      addlg("发送服务器失败",de.id);
                                                    return;
                                                }
                                                // addlog("地址：" + de.addr + " 类型为：" + de.device_type + " ID:" + de.id + "的设备上传寄存器数据:" + cmdstr + "\r\n");
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                                de.bm_all.RemoveAt(0);
                                                i--;
                                            }


                                        }
                                        else
                                        {
                                            if (de.bm_all.Count > max)
                                            {
                                                de.bm_all.RemoveAt(0);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            //  addlog("末端设备地址：" + de.addr + ",接收数据错误\r\n", de.id);
                        }
                        dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
                    }
                }
            }
            catch
            {

            }
        }

        public void udpclient_callback(string cmd, string data, string ip, string port, udpclient uc)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    udpclient.Setback d = new udpclient.Setback(udpclient_callback);
                    Invoke(d, cmd, data, ip, port, uc);
                }
                else
                {
                   
                    if (cmd == "timeout")
                    {
                        dataGridView1.DataSource = device_cmd.ListToDataTable(devices);

                    }
                    if (cmd == "Receive16")
                    {
                        device de = devices.Find(d => { return ip == d.IP && port == d.myport; });

                        List<device> des = devices.FindAll(d => { return ip == d.IP && port == d.myport; });
                        if (de != null)
                        {
                         
                            if (de.device_type == "可变信息标志")
                            {
                                kepclient kepsever = kepsevers.Find(d => { return de.kepsever_ip == d.ip && de.kepsever_port == d.port && de.id == d.id; });
                            de.netstate = "在线";
                            uc.outtime = 0;
                            dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
                            addlog("末端设备地址：" + de.addr + "在线\r\n", de.id);
                            addlog("末端设备地址：" + de.addr + ",接收数据：" + data + "(" + Class1.ZHTOAC(Class1.ZH16(data)) + ")\r\n", de.id);

                                string bm1 = device_cmd.VMS_read_light(data);
                                if (bm1 != "")
                                {
                                    addlog("末端设备地址：" + de.addr + ",接收数据(回读参数)：亮度方式 " + bm1.Substring(2, 2) + "亮度 " + Convert.ToInt32(bm1.Substring(4, 4), 16) + "\r\n", de.id);
                                    if (de.am == "" || de.am.Substring(4, 12) != bm1)
                                    {
                                        de.bm_all.Add(bm1);
                                        // addlog("不一致\r\n");
                                        if (de.am != "" && kepsever != null && kepsever.isrun)
                                        {

                                            for (int i = 0; i < de.bm_all.Count; i++)
                                            {
                                                string bm = de.bm_all[i].ToString();


                                                string cmdstr = device_cmd.write10(de.id, "0004", "0001" + de.addr + bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                    addlg("从站地址：" + de.id + ",发送数据失败\r\n", de.id);
                                                    return;
                                                }
                                                // addlog("地址：" + de.addr + " 类型为：" + de.device_type + " ID:" + de.id + "的设备上传寄存器数据:" + cmdstr + "\r\n");
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                                de.bm_all.RemoveAt(0);
                                                i--;
                                            }


                                        }
                                        else
                                        {
                                            if (de.bm_all.Count > max)
                                            {
                                                de.bm_all.RemoveAt(0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    addlog("末端设备地址：" + de.addr + ",接收数据错误\r\n", de.id);
                                }
                            }
                            else
                            {
                                for (int dei = 0; dei < des.Count; dei++)
                                {
                                    if (des[dei].device_type == "北京公科飞达交通应急广播")
                                    {
                                        uc.chaoshi = 0;
                                        List<string> bm = device_cmd.broadcast_read(data); //解析出报文数据，可能多组
                                        if (bm != null)
                                        {
                                            for (int bi = 0; bi < bm.Count; bi++)
                                            {
                                                string addr = bm[bi].Substring(22, 4);      //分机类型？
                                                addr = int.Parse(bm[bi].Substring(22, 4)) + "";
                                                int bh = des[dei].bhlist.IndexOf(addr);
                                                if (bh >= 0)
                                                {
                                                    bh++;
                                                    kepclient kepsever = kepsevers.Find(d => { return des[dei].kepsever_ip == d.ip && des[dei].kepsever_port == d.port && des[dei].id == d.id; });
                                                    des[dei].netstate = "在线";

                                                    uc.outtime = 0;
                                                    dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
                                                    addlog("末端设备地址：" + bh + "在线\r\n", des[dei].addr10);
                                                    addlog("末端设备地址：" + bh + ",接收数据：" + data + "(" + Class1.ZHTOAC(Class1.ZH16(data)) + ")\r\n", des[dei].addr10);
                                                    addlog("末端设备地址：" + bh + ",接收数据(回读参数)：" + bm[bi] + "\r\n", des[dei].addr10);
                                                    string cmdstr = device_cmd.write10(des[dei].id, "0000", "0001" + Class1.ZHTO16(Class1.ZHAC(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " ")));
                                                    if (!kepsever.send(cmdstr, true))
                                                    {
                                                        addlg("发送服务器失败", de.id);
                                                        return;
                                                    }
                                                    addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                                    //20230913修改
                                                    //htj:原先的bm[bi]示例：2023-09-13 09:54:23 010023YET5                                                            K001+139                                                        09A 
                                                    //前20字符是时间戳。截取的后两个字符其实是"A "，是控制指令加一个空格，有问题。
                                                    //旧的代码，没有发送故障码
                                                    //string bm1 = Class1.ZHTO16(Class1.ZHGB(bm[bi].Substring(0, 20) + bm[bi].Substring(bm[bi].Length - 2, 2))); //此处发送的数据
                                                    //htj新的代码：//字符数：19 + 2 + 1 + 4 + 64 + 64 + 1 + n + 1 + 1 ,n之前固定长度155
                                                    //string bm1 = Class1.ZHTO16(Class1.ZHGB(bm[bi].Substring(0, 20) + bm[bi].Substring(155,bm[bi].Length - 155)));
                                                    string bm1 = Class1.ZHTO16(Class1.ZHGB(bm[bi].Substring(0, 20) + bm[bi].Substring(bm[bi].Length - 3, 2))); //包含故障码
                                                    if (kepsever != null && kepsever.isrun)
                                                    {
                                                        cmdstr = device_cmd.write10(des[dei].id, Class1.ZHTO16(bh*11,2), bm1);
                                                        if (!kepsever.send(cmdstr, true))
                                                        {
                                                            addlg("从站地址：" + des[dei].id + ",发送数据失败\r\n", des[dei].id);
                                                            return;
                                                        }
                                                        // addlog("地址：" + des[dei].addr + " 类型为：" + des[dei].device_type + " ID:" + des[dei].id + "的设备上传寄存器数据:" + cmdstr + "\r\n");
                                                        addlg("从站地址：" + des[dei].id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", des[dei].id);
                                                    }




                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
                    }
                }
            }
            catch
            {

            }
        }
        public void tcpclient_calback(string cmd, string data, tcpclient tl)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    tcpclient.Setback d = new tcpclient.Setback(tcpclient_calback);
                    Invoke(d, cmd, data, tl);
                }
                else
                {
                    device de = devices.Find(d => { return d.device_type == tl.device_type && d.addr == tl.addr;});
                 
                    if (de == null)
                    {
                        tl.Client_Close();
                        tl.timer.Dispose();
                        addlog("末端设备地址：" + tl.addr + "的设备已被删除\r\n", de.id);
                        return;
                    }
                    else
                    {   
                        kepclient kepsever = kepsevers.Find(d => { return de.kepsever_ip == d.ip && de.kepsever_port == d.port && de.id == d.id; });
                        if (cmd == "Connectting")
                        {
                            de.netstate = "在线";
                            addlog("末端设备地址：" + tl.addr + "的设备连接成功\r\n", de.id);
                            if (de.device_type == "车辆检测器")
                            {
                                string cmdstr = device_cmd.write06(tl.id, "0000", "0001");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "上海勋飞微波车检器")
                            {
                                tl.chaoshi = 0;
                                tl.timer.Change(mot_timecs / 2, mot_timecs / 2);
                                string cmdstr = device_cmd.write06(de.id, "0001", "0001");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "唯的美情报板")
                            {
                                tl.chaoshi = 0;
                                tl.timer.Change(5000, 5000);
                                string cmdstr = device_cmd.write06(tl.id, "0043", "0001");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);

                            }
                            if (de.device_type == "消防火灾")
                            {
                                tl.chaoshi = 0;
                                tl.timer.Change(5000, 5000);
                                string cmdstr = device_cmd.write06(tl.id, "0000", "0001");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);

                            }

                        }
                        if (cmd == "Connect_error")
                        {
                            addlog("末端设备地址：" + tl.addr + "的设备连接失败\r\n", de.id);
                            if (!tl.isagain)
                            {
                                de.netstate = "不在线";
                            }
                            else
                            {
                                de.netstate = "连接中...";
                                tl.Client_Connect(de.IP, int.Parse(de.myport));
                                addlog("末端设备地址：" + tl.addr + "的设备重连中\r\n", de.id);
                            }
                        }
                        if (cmd == "Receive")
                        {


                        }
                        if (cmd == "Receive16")
                        {
                            addlog("末端设备地址：" + tl.addr + "接收数据：" + data + "(" + Class1.ZHTOAC(Class1.ZH16(data)) + ")\r\n", de.id);

                            if (de.device_type == "车辆检测器")
                            {
                                string ss = device_cmd.car_read(data, de.addr);
                                if (ss != "" && tl.addr == ss.Substring(8, 4))
                                {
                                    string cmdw = "57314110" + de.addr + "01" + "0000" + Class1.ADD8_Add(Class1.ZH16("57314110" + de.addr + "01" + "0000"));
                                    tl.send(cmdw, true);
                                    addlog("末端设备地址：" + de.addr + "发送数据（回复）：" + cmdw + "(" + Class1.ZHTOAC(Class1.ZH16(cmdw)) + ")\r\n", de.id);
                                    //addlog("末端设备地址：" + de.addr + ",接收数据(回读参数)：" + ss + "\r\n", de.id);
                                    if (tl.am == "" || tl.am.Substring(4) != ss)
                                    {
                                        // addlog("不一致\r\n", de.id);
                                        tl.bm_all.Add(ss);

                                        if (tl.am != "" && kepsever != null && kepsever.isrun)
                                        {
                                            for (int i = 0; i < tl.bm_all.Count; i++)
                                            {
                                                string bm = tl.bm_all[i].ToString();
                                                string cmdstr = device_cmd.write10(tl.id, "0000", "0001" + bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                    addlg("发送服务器失败", de.id);
                                                    return;
                                                }

                                                //addlog("地址：" + de.addr + " 类型为：" + de.device_type + " ID:" + de.id + "的设备上传寄存器数据:" + cmdstr + "\r\n");
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                                tl.bm_all.RemoveAt(0);
                                                i--;
                                            }
                                        }
                                        else
                                        {
                                            if (tl.bm_all.Count > max)
                                            {
                                                tl.bm_all.RemoveAt(0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    addlog("末端设备地址：" + de.addr + ",接收数据错误\r\n", de.id);
                                }
                            }
                            if (de.device_type == "上海勋飞微波车检器")
                            {
                                tl.chaoshi = 0;
                                string ss = device_cmd.MOT_read(data);
                                if (ss != "")
                                {

                                    string cd = ss.Substring(0, 4);
                                    if (de.am == null || de.am == "")
                                    {
                                        for (int i = 0; i < 388; i++)
                                        {
                                            de.am += "0";
                                        }
                                    }
                                    if (cd == de.car1)
                                    {
                                        if (de.am.Length >= 388 && de.am.Substring(8, 76) != ss)
                                        {
                                            if (kepsever != null && kepsever.isrun)
                                            {
                                                string bm = ss;
                                                string cmdstr = device_cmd.write10(de.id, "0000", de.addr + "0001" + bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                    addlg("发送服务器失败", de.id);
                                                    return;
                                                }
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                            }
                                        }
                                    }
                                    else if (cd == de.car2)
                                    {
                                        if (de.am.Length >= 388 || de.am.Substring(84, 76) != ss)
                                        {
                                            if (kepsever != null && kepsever.isrun)
                                            {
                                                string bm = ss;
                                                string cmdstr = device_cmd.write10(de.id, "0015", bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                    addlg("发送服务器失败", de.id);
                                                    return;
                                                }
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                            }
                                        }
                                    }
                                    else if (cd == de.car3)
                                    {
                                        if (de.am.Length >= 388 || de.am.Substring(160, 76) != ss)
                                        {
                                            if (kepsever != null && kepsever.isrun)
                                            {
                                                string bm = ss;
                                                string cmdstr = device_cmd.write10(de.id, "0028", bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                    addlg("发送服务器失败", de.id);
                                                    return;
                                                }
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                            }
                                        }
                                    }
                                    else if (cd == de.car4)
                                    {
                                        if (de.am.Length >= 388 || de.am.Substring(236, 76) != ss)
                                        {
                                            if (kepsever != null && kepsever.isrun)
                                            {
                                                string bm = ss;
                                                string cmdstr = device_cmd.write10(de.id, "003B", bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                    addlg("发送服务器失败", de.id);
                                                    return;
                                                }
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                            }
                                        }
                                    }
                                    else if (cd == de.car5)
                                    {
                                        if (de.am.Length >= 388 || de.am.Substring(312, 76) != ss)
                                        {
                                            if (kepsever != null && kepsever.isrun)
                                            {
                                                string bm = ss;
                                                string cmdstr = device_cmd.write10(de.id, "004E", bm);
                                                if (!kepsever.send(cmdstr, true))
                                                {
                                                    addlg("发送服务器失败", de.id);
                                                    return;
                                                }
                                                addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    addlog("末端设备地址：" + de.addr + ",接收数据错误\r\n", de.id);
                                }
                            }
                            if (de.device_type == "唯的美情报板")
                            {
                                string ss = device_cmd.view_read(data, de.addr,de.aw.Substring(0,4)); tl.chaoshi = 0;
                                if (ss != "")
                                {

                                    addlog("末端设备地址：" + de.addr + "发送数据（回复）：" + data + "\r\n", de.id);
                                    if (de.am != ss)
                                    {
                                        string cmdstr = device_cmd.write10(tl.id, "0042", ss);
                                        if (!kepsever.send(cmdstr, true))
                                        {
                                            addlg("发送服务器失败", de.id);
                                            return;
                                        }
                                        addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                    }
                                }
                                else if (device_cmd.view_pingread(data) == "70")
                                {

                                    string cmdstr = device_cmd.write06(tl.id, "0043", "0001");
                                    if (!kepsever.send(cmdstr, true))
                                    {
                                        addlg("发送服务器失败", de.id);
                                        return;
                                    }
                                    addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);

                                }
                            }

                            if (de.device_type == "消防火灾")
                            {
                                if (data == device_cmd.WAIT())
                                {
                                    addlog("末端设备地址：" + tl.addr + "接收到WAIT\r\n", de.id);
                                    tl.send(device_cmd.ACK(), true);
                                    addlog("末端设备地址：" + tl.addr + "接收到ACK\r\n", de.id);
                                    tl.send(device_cmd.INIT(), true);
                                    addlog("末端设备地址：" + tl.addr + "接收到INIT\r\n", de.id);
                                    tl.timer.Change(1000, 1000);
                                }
                                else if (data == device_cmd.PING())
                                {
                                    addlog("末端设备地址：" + tl.addr + "接收到\r\n", de.id);
                                }
                                else if (data == device_cmd.REJEJCT())
                                {
                                    addlog("末端设备地址：" + tl.addr + "接收到REJEJCT\r\n", de.id);
                                    tl.send(device_cmd.ACK(), true);
                                    addlog("末端设备地址：" + tl.addr + "发送ACK\r\n", de.id);
                                    MessageBox.Show("设备IP未配置！");
                                }
                                else
                                {
                                    if (data.Length > 8 && data.Substring(0, 2) == "02" && data.Substring(data.Length - 2, 2) == "03" && data.Substring(2, 2) == "44")
                                    {

                                        string bm1 = device_cmd.Fire_read(data);
                                        if (bm1 != "" && bm1.Substring(4, 20) == tl.addr)
                                        {
                                            tl.timer.Change(1000, 1000);
                                            addlog("末端设备地址：" + de.addr + ",接收数据(回读参数)：" + bm1 + "\r\n", de.id);
                                            if (tl.am == "" || tl.am.Substring(4) != bm1)
                                            {
                                                // addlog("不一致\r\n");
                                                tl.bm_all.Add(device_cmd.Fire_read(data));
                                                if (tl.am != "" && kepsever != null && kepsever.isrun)
                                                {
                                                    for (int i = 0; i < tl.bm_all.Count; i++)
                                                    {
                                                        string bm = tl.bm_all[i].ToString();
                                                        string cmdstr = device_cmd.write10(tl.id, "0000", "0001" + bm);
                                                        if (!kepsever.send(cmdstr, true))
                                                        {
                                                            addlg("发送服务器失败", de.id);
                                                            return;
                                                        }

                                                        //addlog("地址：" + de.addr + " 类型为：" + de.device_type + " ID:" + de.id + "的设备上传寄存器数据:" + cmdstr + "\r\n");
                                                        addlg("从站地址：" + de.id + ",发送数据(写入回读参数)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                                        tl.bm_all.RemoveAt(0);
                                                        i--;

                                                    }
                                                }
                                                else
                                                {
                                                    if (tl.bm_all.Count > max)
                                                    {
                                                        tl.bm_all.RemoveAt(0);
                                                    }
                                                }

                                            }
                                            tl.send(device_cmd.ACK(), true);
                                            addlog("末端设备地址：" + tl.addr + "发送ACK\r\n", de.id);

                                        }
                                    }

                                }
                            }
                        }
                        if (cmd == "掉线")
                        {
                            addlog("末端设备地址：" + tl.addr + "掉线\r\n", de.id);
                            if (!tl.isagain)
                            {
                                de.netstate = "不在线";
                            }
                            else
                            {
                                de.netstate = "连接中...";
                                tl.Client_Connect(de.IP, int.Parse(de.myport));
                                addlog("末端设备地址：" + tl.addr + "重连中\r\n", de.id);
                            }
                            if (de.device_type == "上海勋飞微波车检器")
                            {
                                tl.timer.Change(Timeout.Infinite, Timeout.Infinite);
                                string cmdstr = device_cmd.write06(tl.id, "0001", "0000");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "车辆检测器")
                            {
                                string cmdstr = device_cmd.write06(tl.id, "0000", "0000");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }
                            if (de.device_type == "唯的美情报板")
                            {
                                tl.timer.Change(Timeout.Infinite, Timeout.Infinite);
                                string cmdstr = device_cmd.write06(tl.id, "0043", "0000");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);


                            }
                            if (de.device_type == "消防火灾")
                            {
                                tl.timer.Change(Timeout.Infinite, Timeout.Infinite);
                                string cmdstr = device_cmd.write06(tl.id, "0000", "0000");
                                if (!kepsever.send(cmdstr, true))
                                {
                                    addlg("发送服务器失败", de.id);
                                    return;
                                }
                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                            }

                        }
                    }
                    dataGridView1.DataSource =device_cmd.ListToDataTable(devices);
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void Connect(device de)
        {

            try
            {
                if (de.nettype == "TCP服务器")
                {
                    tcpclient tc = tcs.Find(d => { return de.device_type == d.device_type && de.addr == d.addr; });
                    if (tc == null)
                    {
                        tc = new tcpclient();

                        tc.device_type = de.device_type;
                        tc.id = de.id;
                        tc.addr = de.addr;
                        if (de.device_type == "唯的美情报板")
                        {
                            if (de.aw == "")
                            { de.aw = "0001"; }
                            tc.timer = new System.Threading.Timer(new TimerCallback((state) =>
                            {
                                if (tc.chaoshi > 5)
                                {
                                    addlog("超时:" + tc.chaoshi + "次\r\n", de.id);
                                    tc.chaoshi = 0;
                                    tc.Client_Close1();

                                    return;
                                }
                                if (tc.isrun)
                                {
                                    if (tc.device_type == "唯的美情报板")
                                    {
                                        addlog("ping:" + device_cmd.view_ping() + "\r\n", de.id);
                                        tc.send(device_cmd.view_ping(), true);
                                        //   log.Append("地址：" + addr + " 类型为：" + device_type + " ID:" + id + "的设备发送PING\r\n");
                                    }
                                    tc.chaoshi++;
                                }
                                else
                                {
                                    tc.timer.Change(-1, -1);
                                }

                            }));
                        }
                        if (de.device_type == "上海勋飞微波车检器")
                        {
                            tc.timer = new System.Threading.Timer(new TimerCallback((state) =>
                            {
                                if (tc.chaoshi > 1)
                                {
                                    addlog("超时:" + tc.chaoshi + "次\r\n", de.id);
                                    tc.chaoshi = 0;
                                    tc.Client_Close1();

                                    return;
                                }
                                if (tc.isrun)
                                {
                                   
                                    tc.chaoshi++;
                                }
                                else
                                {
                                    tc.timer.Change(-1, -1);
                                }

                            }));
                        }
                         tc.aw = de.aw;
                        if (tc.device_type == "消防火灾")
                        {
                            tc.timer = new System.Threading.Timer(new TimerCallback((state) =>
                            {
                                if (tc.isrun)
                                {
                                    if (tc.device_type == "消防火灾")
                                    {
                                        tc.send(device_cmd.PING(), true);
                                        //   log.Append("地址：" + addr + " 类型为：" + device_type + " ID:" + id + "的设备发送PING\r\n");
                                    }
                                }
                                else
                                {
                                    tc.timer.Change(-1, -1);
                                }
                            }));
                        }
                        tc.am = de.am;
                        tc.setOnSetback(new tcpclient.Setback(tcpclient_calback));
                        tc.Client_Connect(de.IP, int.Parse(de.myport));
                        tcs.Add(tc);
                    }
                }
                if (de.nettype == "TCP客户端")
                {
                    tcpsever u = tls.Find(d => { return de.Lport == d.Port.ToString(); });
                    if (de.device_type == "丰海科技情报板" && (de.aw == ""))
                    {
                        de.aw = device_cmd.Infomation1_aw(ifd);
                    }
                    if (de.device_type == "交通诱导灯" && (de.aw == ""))
                    {
                        de.aw = Class1.ZHTO16(leadset, 2);
                    }
                    if (de.device_type == "投影灯" && (de.aw == ""))
                    {
                        de.aw = Class1.ZHTO16(LAMPset, 2);
                    }

                    //天津五维地磁车检器?
                    //HTJ
                    if (de.device_type == "天津五维地磁车检器" && (de.aw == ""))
                    {
                        //de.aw = Class1.ZHTO16(LAMPset, 2);
                    }

                    if (u == null)
                    {
                        u = new tcpsever();
                        u.device_type = de.device_type;
                        u.setOnSetback(new tcpsever.Setback(tcpsever_callback));
                        u.start_sever(int.Parse(de.Lport));
                        tls.Add(u);
                        timer1.Start();
                    }
                }
                if (de.nettype == "UDP客户端")
                {
                    udpsever u = uds.Find(d => { return de.Lport == d.Port.ToString(); });
                    if (u == null)
                    {
                        u = new udpsever();
                        u.device_type = de.device_type;
                        u.setOnSetback(new udpsever.Setback(udpsever_callback));
                        u.start_sever(int.Parse(de.Lport));
                        uds.Add(u);
                    }
                }
                if (de.nettype == "UDP服务器")
                {
                    udpclient uc = ucs.Find(d => { return de.device_type == "UDP服务器" && de.IP == d.IP && de.myport == d.PORT.ToString(); });
                    if (uc == null)
                    {
                        uc = new udpclient();
                        uc.device_type = de.device_type;
                        uc.id = de.id;
                        uc.addr = de.addr;
                        uc.aw = de.aw;
                        uc.IP = de.IP;
                        uc.PORT = int.Parse(de.myport);
                        if (de.Lport==null||de.Lport == "")
                        {
                            uc.CPORT = 0;
                        }
                        else
                        {
                            uc.CPORT =int.Parse( de.Lport);
                        }
                        if (de.device_type == "可变信息标志" && (uc.aw == ""))
                        {
                            uc.aw = vmsset; 
                           
                        }
                        
                        uc.am = de.am;
                        uc.setOnSetback(new udpclient.Setback(udpclient_callback));
                        if (uc.Client_Connect())
                        {
                            if (de.device_type == "北京公科飞达交通应急广播")
                            {
                                uc.timer = new System.Threading.Timer(new TimerCallback((state) =>
                                {
                                    if (uc.chaoshi > 5)
                                    {
                                        addlog("超时:" + uc.chaoshi + "次\r\n", de.id);
                                        uc.chaoshi = 0;
                                        List<device> des = devices.FindAll(d => { return de.IP == d.IP && de.myport == d.myport; });
                                        for (int dei = 0; dei < des.Count; dei++)
                                        {
                                            if (des[dei].device_type == "北京公科飞达交通应急广播")
                                            {         
                                                des[dei].netstate = "不在线";
                                                kepclient kepsever1 = kepsevers.Find(d => { return des[dei].kepsever_ip == d.ip && des[dei].kepsever_port == d.port && des[dei].id == d.id; });
                                                string cmdstr = device_cmd.write10(des[dei].id, "0000", "0000"+ Class1.ZHTO16(Class1.ZHAC(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+" ")));
                                                if (!kepsever1.send(cmdstr, true))
                                                {
                                                    addlg("发送服务器失败", de.id);
                                                    return;
                                                }
                                                addlg("从站地址：" + de.id + ",发送数据(网络)：" + cmdstr + "(" + Class1.ZHTOAC(Class1.ZH16(cmdstr)) + ")\r\n", de.id);
                                            }
                                        }
                                        uc.timeout();
                                        return;
                                    }
                                    if (uc.isrun)
                                    {

                                        uc.chaoshi++;
                                    }
                                    else
                                    {
                                        uc.timer.Change(-1, -1);
                                    }
                                }));
                                uc.timer.Change(5000, 5000);
                            }
                            if (de.device_type == "可变信息标志" )
                            {
                                timer2.Start();
                            }
                        }
                        uc.chaoshi = 0;
                        ucs.Add(uc);
                      
                    }
                }

                kepclient kepsever = kepsevers.Find(d => { return de.kepsever_ip == d.ip && de.kepsever_port == d.port && de.id == d.id; });
                if (kepsever == null)
                {
                    kepsever = new kepclient();
                    kepsever.id = de.id;
                    kepsever.ip = de.kepsever_ip;
                    kepsever.port = de.kepsever_port;
                    kepsever.times = de.kepsever_times;
                    kepsever.timer = new System.Threading.Timer(new TimerCallback((state) =>
                    {
                        if (kepsever.isrun)
                        {
                            for (int i = 0; i < devices.Count; i++)
                            {
                                if (devices[i].kepsever_ip == kepsever.ip && devices[i].kepsever_port == kepsever.port && devices[i].id == kepsever.id)
                                {
                                    if (devices[i].device_type == "应急电话")
                                    {
                                        kepsever.send(device_cmd.phone_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.phone_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    if (devices[i].device_type == "丰海科技情报板")
                                    {
                                        kepsever.send(device_cmd.Infomation1_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.Infomation1_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    if (devices[i].device_type == "消防火灾")
                                    {
                                        kepsever.send(device_cmd.Fire_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.Fire_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    if (devices[i].device_type == "车辆检测器")
                                    {
                                        kepsever.send(device_cmd.car_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.car_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    if (devices[i].device_type == "可变信息标志")
                                    {
                                        kepsever.send(device_cmd.VMS_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.VMS_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    if (devices[i].device_type == "交通诱导灯")
                                    {
                                        kepsever.send(device_cmd.lead_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.lead_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    if (devices[i].device_type == "投影灯")
                                    {
                                        kepsever.send(device_cmd.LAMP_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.LAMP_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    if (devices[i].device_type == "唯的美情报板")
                                    {
                                        kepsever.send(device_cmd.view_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.view_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    //if (devices[i].device_type == "北京公科飞达交通应急广播")
                                    //{
                                    //    kepsever.send(device_cmd.broadcast_write03(devices[i].id), true);
                                    //    addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.broadcast_write03(devices[i].id) + "\r\n", devices[i].id);
                                    //}
                                    if (devices[i].device_type == "上海勋飞微波车检器")
                                    {
                                        kepsever.send(device_cmd.MOT_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.MOT_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    if (devices[i].device_type == "天津五维地磁车检器")
                                    {
                                        kepsever.send(device_cmd.car5_write03(devices[i].id), true);
                                        addlg("从站地址:" + devices[i].id + " 发送(获取服务器寄存器值命令)：" + device_cmd.car5_write03(devices[i].id) + "\r\n", devices[i].id);
                                    }
                                    Thread.Sleep(1);

                                }
                            }
                        }
                        else
                        {
                            kepsever.timer.Change(-1, -1);
                        }
                    }));
                    kepsever.setOnSetback(kepsever_callback);
                    kepsever.Client_Connect(de.kepsever_ip, de.kepsever_port);
                    kepsevers.Add(kepsever);
                }
            }
            catch
            { }
        }
        public void loadini()
        {
            try
            {
               
                
                try
                {
                    StreamReader sr1 = new StreamReader(Application.StartupPath + "\\devices.ini");
                    string[] ids = sr1.ReadToEnd().Split('\n');
                    sr1.Close();
                    devices.Clear();
                    for(int i=0;i<ids.Length;i++)
                    {
                        if (ids[i] != "")
                        {
                            device de = new device();
                            string[] stt = ids[i].Split(';');
                            de.device_type = stt[0];
                            de.id = stt[1];
                            de.addr = stt[2];
                            de.IP = stt[3];
                            de.myport = stt[4];
                            de.am = stt[5];
                            de.aw = stt[6];
                            de.nettype = stt[7];
                            //htj，原转换对天津地磁有错 
                            try
                            {
                                de.id10 = Convert.ToInt32(de.id, 16) + "";
                                if(de.device_type == "天津五维地磁车检器")   //htj针对性修改
                                {
                                    de.addr10 = Encoding.ASCII.GetString(Class1.ZH16(de.addr));
                                }
                                else 
                                { 
                                    de.addr10 = Convert.ToInt32(de.addr, 16) + "";   //? "3030323136303032"应转成00216002才行。
                                }
                            }
                            catch
                            {

                            }//htj
                            de.kepsever_ip = stt[8];
                            if (stt[9] != "")
                                de.kepsever_port = int.Parse(stt[9]);
                            if (stt[10] != "")
                                de.kepsever_times = int.Parse(stt[10]);
                            de.Lport = stt[11];
                            try
                            {
                                de.car1 = stt[12];
                                de.car2 = stt[13];
                                de.car3 = stt[14];
                                de.car4 = stt[15];
                                de.car5 = stt[16];
                            }
                            catch
                            { }

                            try
                            {
                                de.cardid= stt[17];
                            }
                            catch
                            { }
                            devices.Add(de);
                        }
                    }
                    dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
                }
                catch
                {

                }
                try
                {
                    StreamReader sr1 = new StreamReader(Application.StartupPath + "\\bh.ini");
                    string[] ids = sr1.ReadToEnd().Split('\n');
                    sr1.Close();
                    for(int i=0;i<ids.Length;i++)
                    {
                        string[] stt = ids[i].Split(';');
                        device de = devices.Find(d => { return stt[0] == d.id; });
                        if(de!=null)
                        {
                            de.bhlist.Clear();
                            for (int n=1;n<stt.Length;n++)
                            {
                                de.bhlist.Add(stt[n]);
                            }
                        }
                    }
                }
                catch
                { }
                try
                {
                    StreamReader sr1 = new StreamReader(Application.StartupPath + "\\Information1.ini");
                    string[] ins = sr1.ReadToEnd().Split(';');
                    sr1.Close();
                    ifd.speed_limit = ins[0];
                    ifd.typeface = ins[1];
                    ifd.red = ins[2];
                    ifd.light = ins[3];
                    ifd.light_regulate = ins[4];
                    ifd.onoff = ins[5];
                    timer1.Interval = int.Parse(ins[6]);

                }
                catch
                { }
                try
                {
                    StreamReader sr1 = new StreamReader(Application.StartupPath + "\\vms.ini");
                    string[] ins = sr1.ReadToEnd().Split(';');
                    sr1.Close();
                    vmsset = ins[0];
                    timer2.Interval = int.Parse(ins[1]);
                    vmsout= int.Parse(ins[2]);
                }
                catch
                { }
                try
                {
                    StreamReader sr1 = new StreamReader(Application.StartupPath + "\\LAMPset.ini");
                    string ins = sr1.ReadToEnd();
                    sr1.Close();
                    LAMPset = int.Parse(ins);
                   
                }
                catch
                { }
                try
                {
                    StreamReader sr1 = new StreamReader(Application.StartupPath + "\\leadset.ini");
                    string ins = sr1.ReadToEnd();
                    sr1.Close();
                    leadset = int.Parse(ins);
                }
                catch
                { }
                try
                {
                    StreamReader sr1 = new StreamReader(Application.StartupPath + "\\motset.ini");
                    string ins = sr1.ReadToEnd();
                    sr1.Close();
                    mot_timecs = int.Parse(ins);
                }
                catch
                { }
            }
            catch
            { 
            }
         }
        public void saveini()
        {
            string bh = "";
            try
            {
                string str = "";
                for (int i = 0; i < devices.Count; i++)
                {
                    str += devices[i].device_type + ";" + devices[i].id+";"+
                        devices[i].addr + ";" +
                        devices[i].IP + ";" +
                        devices[i].myport + ";"+ devices[i].am + ";" + devices[i].aw +";"+ devices[i].nettype+";"+devices[i].kepsever_ip+ ";" + devices[i].kepsever_port +";" + devices[i].kepsever_times+";"+ devices[i].Lport + ";"+ devices[i].car1+ ";" + devices[i].car2 + ";" + devices[i].car3 + ";" + devices[i].car4 + ";" + devices[i].car5 + ";" + devices[i].cardid+ "\n";
                    if (devices[i].device_type == "北京公科飞达交通应急广播" && devices[i].bhlist.Count >= 50)
                    {
                        bh += devices[i].id ;
                        for(int n=0;n< devices[i].bhlist.Count;n++)
                        {
                            bh +=";"+ devices[i].bhlist[n] ;
                        }
                        bh += "\n";
                    }
                }
                File.WriteAllText(Application.StartupPath + "\\devices.ini", str);
            }
            catch
            {

            }
            try
            {
                File.WriteAllText(Application.StartupPath + "\\bh.ini", bh);
            }
            catch
            { }
            try
            {
                string str =
                ifd.speed_limit + ";" +
                ifd.typeface + ";" +
                ifd.red + ";" +
                ifd.light + ";" +
                ifd.light_regulate + ";" +
                ifd.onoff + ";" +
                timer1.Interval;

                File.WriteAllText(Application.StartupPath + "\\Information1.ini", str);
            }
            catch
            { }
            try
            {
                string str =
                vmsset+";" +
                timer2.Interval+";"+vmsout;

                File.WriteAllText(Application.StartupPath + "\\vms.ini", str);
            }
            catch
            { }

            try
            {
                string str =
               leadset+"";

                File.WriteAllText(Application.StartupPath + "\\leadset.ini", str);
            }
            catch
            { }

            try
            {
                string str =
             LAMPset + "";

                File.WriteAllText(Application.StartupPath + "\\LAMPset.ini", str);
            }
            catch
            { }
            try
            {
                string str =
             mot_timecs + "";

                File.WriteAllText(Application.StartupPath + "\\motset.ini", str);
            }
            catch
            { }
        }

        private void keptimer1_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    if (kepsever.isrun)
            //    {
            //        for (int i = 0; i < tcs.Count; i++)
            //        {
            //            if (tcs[i].device_type == "情报版1")
            //            {
            //                kepsever.send(device_cmd.Infomation1_write03(tcs[i].id), true);
            //                kepsever.log.Append("地址：" + tcs[i].addr + " 类型为：" + tcs[i].device_type + " ID:" + tcs[i].id + " 发送获取服务器寄存器值命令" + device_cmd.Infomation1_write03(tcs[i].id) + "\r\n");
            //            }
            //            if (tcs[i].device_type == "消防火灾")
            //            {
            //                kepsever.send(device_cmd.Fire_write03(tcs[i].id), true);
            //                kepsever.log.Append("地址：" + tcs[i].addr + " 类型为：" + tcs[i].device_type + " ID:" + tcs[i].id + " 发送获取服务器寄存器值命令" + device_cmd.Fire_write03(tcs[i].id) + "\r\n");
            //            }
            //            if (tcs[i].device_type == "车辆检测器")
            //            {
            //                kepsever.send(device_cmd.car_write03(tcs[i].id), true);
            //                kepsever.log.Append("地址：" + tcs[i].addr + " 类型为：" + tcs[i].device_type + " ID:" + tcs[i].id + " 发送获取服务器寄存器值命令" + device_cmd.car_write03(tcs[i].id) + "\r\n");
            //            }

            //        }
            //        for (int i = 0; i < devices.Count; i++)
            //        {
            //            if(devices[i].device_type== "应急电话")
            //            {
            //                kepsever.send(device_cmd.phone_write03(devices[i].id), true);
         
            //                kepsever.log.Append("地址：" + tcs[i].addr + " 类型为：" + tcs[i].device_type + " ID:" + tcs[i].id + " 发送获取服务器寄存器值命令" + device_cmd.phone_write03(tcs[i].id) + "\r\n");
                          
            //            }
            //        }
            //    }
            //}
            //catch
            //{ }
        }
      
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < tls.Count; i++)
                {
                    if (tls[i].device_type == "丰海科技情报板")
                    {
                        if (tls[i].isrun)
                        {
                            for (int n = 0; n < tls[i].client_all.Count; n++)
                            {
                                Socket socket = (Socket)tls[i].client_all[n];
                                if (socket != null)
                                {
                                    device de = devices.Find(d => { return tls[i].device_type == d.device_type && socket.RemoteEndPoint.ToString() == d.IP + ":" + d.myport.ToString(); });
                                    if (de != null)
                                    {
                                        if (device_cmd.Information1_57(de.addr, socket))
                                            addlog("末端地址：" + de.addr + " 发送（获取参数命令）" + device_cmd.db(de.addr, "57", "") + "\r\n", de.id);
                                        else
                                            addlog("末端地址：" + de.addr + " 发送（获取参数命令）" + " 发送失败" + "\r\n", de.id);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            { }
        }
        public void bind()
        {
            try
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    Connect(devices[i]);
                }
                dataGridView1.DataSource = device_cmd.ListToDataTable(devices);
            }
            catch
            { }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            loadini();
            bind();
        }
        Log log = null; //服务器日志？
        Log lg = null;  //设备日志？
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "Column5")
                    {
                        device de = devices[e.RowIndex];

                        if (lg == null || lg.IsDisposed)
                        {

                        }
                        else
                        {
                            lg.Close();
                        }
                        lg = new Log(de.id);
                        lg.Text = "服务器地址：" + de.kepsever_ip + ":" + de.kepsever_port + ",从站地址" + de.id10 + "的服务器日志";
                        lg.Show();
                    }
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "Column1")
                    {
                        device de = devices[e.RowIndex];


                        if (log == null || log.IsDisposed)
                        {

                        }
                        else
                        {
                            log.Close();
                        }
                        if (de.device_type == "北京公科飞达交通应急广播")
                        {
                            log = new Log(de.addr10);
                            log.Text = de.device_type + ",设备编号" + de.addr10 + "的设备日志";
                        }
                        else
                        {
                            log = new Log(de.id);
                            log.Text = de.device_type + ",从站地址" + de.id10 + "的设备日志";
                        }
                        log.Show();


                    }
                }
            }
            catch
            { }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveini();
            try
            {
                for (int i = 0; i < kepsevers.Count; i++)
                {
                    kepsevers[i].isagain = false;
                    kepsevers[i].Client_Close();
                }
            }
            catch
            { }
            try
            {
                for (int i = 0; i < tcs.Count; i++)
                {
                    tcs[i].isagain = false;
                    tcs[i].Client_Close();
                }
            }
            catch
            { }
            try
            {
                for (int i = 0; i < tls.Count; i++)
                {

                    tls[i].stop_sever();
                }
            }
            catch
            { }
            try
            {
                for (int i = 0; i < ucs.Count; i++)
                {

                    ucs[i].close();
                }
            }
            catch
            { }
            try
            {
                for (int i = 0; i < uds.Count; i++)
                {

                    uds[i].close();
                }
            }
            catch
            { }
            Process.GetCurrentProcess().Kill();
        }
    
        //private void 设置服务器端口ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    set set = new set(this);
        //    set.Show();
        //}
        main ma;
        private void 添加设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ma==null||ma.IsDisposed)
             ma = new main(this);
            ma.Show();
        }
        Information1 in1;
        private void 情报板1默认设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (in1 == null || in1.IsDisposed)
                in1 = new Information1(this);
            in1.Show();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < ucs.Count; i++)
                {
                    if (ucs[i].device_type == "可变信息标志")
                    {
                        if (ucs[i].outtime < vmsout)
                        {
                            if (ucs[i].isrun)
                            {

                                string stt = device_cmd.VMS_write_readlight(ucs[i].addr);
                                if (ucs[i].send(stt, ucs[i].IP, ucs[i].PORT + "", true))
                                    addlog("末端设备地址：" + ucs[i].addr + "发送数据（获取亮度）" + stt + "(" + Class1.ZHTOAC(Class1.ZH16(stt)) + ")\r\n", ucs[i].id);
                                else
                                    addlog("末端设备地址：" + ucs[i].addr + "发送数据（获取亮度）失败" + stt + "(" + Class1.ZHTOAC(Class1.ZH16(stt)) + ")\r\n", ucs[i].id);
                            }

                        }
                        else
                        {
                            device de = devices.Find(d => { return ucs[i].id == d.id; });
                            if (de != null)
                            {
                                de.netstate = "不在线";
                            }
                            ucs[i].outtime = 0;

                        }

                    }
                }
            }
            catch
            { }
        }

        Information2 in2;
        lead_set ls;
        LAMP_set las;
        mot_set mo;
        private void 交通诱导灯ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ls == null || ls.IsDisposed)
                ls = new lead_set(this);
            ls.Show();
        }

        private void 投影灯默认设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (las == null || las.IsDisposed)
                las = new LAMP_set(this);
            las.Show();
        }

        private void 上海勋飞微波车检器设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mo == null || mo.IsDisposed)
                mo = new mot_set(this);
            mo.Show();
        }

        //天津五维地磁车检器
        //HTJ
        car5_set car5;
        private void 天津五维地磁车检器设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (car5 == null || car5.IsDisposed)
                car5 = new car5_set(this);
            car5.Show();
        }

        private void 可变信息板默认设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (in2 == null || in2.IsDisposed)
                in2 = new Information2(this);
            in2.Show();
        }
    }
}
