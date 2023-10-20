using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.add
{
    public partial class add : Form
    {
        Form1 f1;
        main ma;
        public add(Form1 F1,main Ma)
        {
            InitializeComponent();
            f1 = F1;
            ma = Ma;
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (comboBox1.SelectedItem.ToString() == "唯的美情报板")
                {
                    try
                    {
                        textBox2.Text = "";
                        textBox2.Text = textBox1.Text.Split('.')[3];
                    }
                    catch
                    { }
                }
                if(textBox4.Text==""|| textBox3.Text == "")
                {
                    MessageBox.Show("参数未设置完");
                    return;
                }
                if (comboBox1.SelectedItem.ToString() == "北京公科飞达交通应急广播")
                {
                    // textBox2.Text = "1~" + numericUpDown5.Value;

                    string Lport = numericUpDown4.Value.ToString();
                    device u1 = f1.devices.Find(d => { return  Lport == d.Lport; });
                    if (u1 != null)
                    {
                        MessageBox.Show("监听端口已被其他设备占用");
                        return;
                    }

                    device de = new device();
                    de.id = Class1.ZHTO16(int.Parse(textBox3.Text), 1);
                    de.addr = de.id;
                    de.id10 = int.Parse(textBox3.Text) + "";
                    de.addr10 = de.id10;
                    de.device_type = comboBox1.SelectedItem.ToString();
                    de.IP = textBox1.Text;
                    de.myport = numericUpDown1.Value.ToString();
                    de.kepsever_ip = textBox4.Text;
                    de.kepsever_port = Convert.ToInt32(numericUpDown3.Value);
                    de.kepsever_times = Convert.ToInt32(numericUpDown2.Value);
                    de.Lport = Lport;
                    de.nettype = "UDP服务器";
                    device u2 = f1.devices.Find(d => { return de.kepsever_ip == d.kepsever_ip && de.kepsever_port == d.kepsever_port && de.id == d.id; });
                    if (u2 != null)
                    {
                        MessageBox.Show("该服务器已有相同从站地址");
                        return;
                    }
                    ArrayList bhlist = new ArrayList();
                    for (int i = 0; i < 50; i++)
                    {
                        bhlist.Add("");
                    }
                    de.bhlist = bhlist;
                    f1.devices.Add(de);
                    ma.bind();
                    f1.bind();
                    f1.saveini();
                    bhfp bh = new bhfp(f1, f1.devices.Count - 1);
                    bh.Show();
                    bh.bind(de.bhlist);
                    this.Close();
                }
                else
                {
                    if ( textBox2.Text == "")   //设备编号
                    {
                        MessageBox.Show("参数未设置完");
                        return;
                    }
                    device de = new device();
                    de.id = Class1.ZHTO16(int.Parse(textBox3.Text), 1); //从站地址（服务器）（前端补零的16进展字符串），1Byte=>2字符

                    de.addr = Class1.ZHTO16(int.Parse(textBox2.Text), 2);//设备编号（末端设备）（前端补零的16进展字符串），2Byte=>4字符


                    de.id10 = int.Parse(textBox3.Text) + "";        //从站地址（末端设备），整型

                    de.addr10 = int.Parse(textBox2.Text) + "";      //设备编号（末端设备），整型
                    de.device_type = comboBox1.SelectedItem.ToString(); //设备类型（末端设备），字符串
                    de.IP = textBox1.Text;  //IP（末端设备）
                    de.myport = numericUpDown1.Value.ToString();    //端口号（信号机端口号？）
                    de.kepsever_ip = textBox4.Text;     //服务器IP
                    de.kepsever_port = Convert.ToInt32(numericUpDown3.Value);   //服务器端口号
                    de.kepsever_times = Convert.ToInt32(numericUpDown2.Value);  //服务器查询频率
                    if (de.device_type == "丰海科技情报板" || de.device_type == "交通诱导灯" || de.device_type == "投影灯" || de.device_type == "天津五维地磁车检器")
                    {
                        de.nettype = "TCP客户端";
                    }
                    else if (de.device_type == "消防火灾" || de.device_type == "车辆检测器" || comboBox1.SelectedItem.ToString() == "上海勋飞微波车检器" || de.device_type == "唯的美情报板")
                    {
                        de.nettype = "TCP服务器";
                    }
                    else if (de.device_type == "应急电话")
                    {
                        de.nettype = "UDP客户端";

                    }
                    else if (de.device_type == "可变信息标志")
                    {
                        de.nettype = "UDP服务器";
                    }
                    if (de.nettype == "TCP客户端" || de.nettype == "UDP客户端")
                    {
                        de.Lport = numericUpDown4.Value.ToString();     //监听端口号
                        device u = f1.devices.Find(d => { return de.Lport == d.Lport; });
                        if (u != null)
                        {
                            //if (de.device_type == "天津五维地磁车检器") //20230904，发现不能添加多个设备。20230906改回了
                            //{

                            //}
                            //else
                            //{
                                MessageBox.Show("监听端口已被其他设备占用");
                                return;
                            //}
                        }
                    }
                    List<device> ds = f1.devices.FindAll(d => { return de.kepsever_ip == d.kepsever_ip && de.kepsever_port == d.kepsever_port; });
                    if (ds.Count >= 255)
                    {
                        MessageBox.Show("该服务器端口已满255个客户端，请更换其他服务器端口");
                        return;
                    }
                    else
                    {
                        device u = f1.devices.Find(d => { return de.id == d.id; });
                        if (u != null)
                        {
                            MessageBox.Show("在该服务器IP端口下已有相同的从站地址");
                            return;
                        }
                    }
                    for (int i = 0; i < f1.devices.Count; i++)
                    {
                        if (f1.devices[i].kepsever_ip == de.kepsever_ip && f1.devices[i].kepsever_port == de.kepsever_port)
                        {
                            f1.devices[i].kepsever_times = de.kepsever_times;
                        }
                    }
                    if (comboBox1.SelectedItem.ToString() == "上海勋飞微波车检器")
                    {
                        de.car1 = Class1.ZHTO16(Convert.ToInt32(car1.Value - 1), 2);
                        de.car2 = Class1.ZHTO16(Convert.ToInt32(car2.Value - 1), 2);
                        de.car3 = Class1.ZHTO16(Convert.ToInt32(car3.Value - 1), 2);
                        de.car4 = Class1.ZHTO16(Convert.ToInt32(car4.Value - 1), 2);
                        de.car5 = Class1.ZHTO16(Convert.ToInt32(car5.Value - 1), 2);

                        decimal[] Input = new decimal[5] { car1.Value, car2.Value, car3.Value, car4.Value, car5.Value };
                        for (int i = 0; i < Input.Length; i++)
                        {
                            //int a = Input[i];
                            for (int j = i + 1; j < Input.Length; j++)
                            {
                                //int b = Input[j];
                                if (Input[i] == Input[j])
                                {
                                    MessageBox.Show("车道设置里有相同的！");
                                    return;
                                }

                            }
                        }
                    }
                    if (de.device_type == "天津五维地磁车检器")
                    {
                        if(textBox2.Text.Length<8)  //设备编号（末端设备）
                        {
                            MessageBox.Show("请输入8字节编号");
                            return;
                        }
                        de.addr = Class1.ZHTO16(Class1.ZHAC(textBox2.Text));  //设备编号（ASCII转Byte后在转前补零16进制字符串）"00216002" =>  "3030323136303032"
                        de.addr10 =textBox2.Text;   //设备编号字符串
                        if (c5_1.Text==""|| c5_2.Text == "" || c5_3.Text == "" || c5_4.Text == "" || c5_5.Text == "")
                        {
                            MessageBox.Show("请设置完车道");
                            return;
                        }
                        de.car1 = Class1.ZHTO16(Class1.ZHAC(c5_1.Text));//车道（ASCII转Byte后在转前补零16进制字符串）"101"=>"313031"
                        de.car2 = Class1.ZHTO16(Class1.ZHAC(c5_2.Text));
                        de.car3 = Class1.ZHTO16(Class1.ZHAC(c5_3.Text));
                        de.car4 = Class1.ZHTO16(Class1.ZHAC(c5_4.Text));
                        de.car5 = Class1.ZHTO16(Class1.ZHAC(c5_5.Text));

                        string[] Input = new string[5] { de.car1, de.car2, de.car3, de.car4, de.car5 };
                        for (int i = 0; i < Input.Length; i++)
                        {
                            //int a = Input[i];
                            for (int j = i + 1; j < Input.Length; j++)
                            {
                                //int b = Input[j];
                                if (Input[i] == Input[j])
                                {
                                    MessageBox.Show("车道设置里有相同的！");
                                    return;
                                }

                            }
                        }
                    }
                    if (de.device_type == "唯的美情报板")
                    {
                        de.cardid = Class1.ZHTO16(int.Parse(textBox5.Text), 1);
                    }
                    f1.devices.Add(de);
                    ma.bind();
                    f1.bind();
                    f1.saveini();
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("添加失败");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Text = "添加";
            label6.Text = "设备编号：";
            label11.Visible = false;
            numericUpDown4.Visible = false;
            label2.Visible = true;
            textBox1.Visible = true;
            label3.Visible = true;
            numericUpDown1.Visible = true;
            textBox2.Enabled = true;
            label12.Visible = false;
            numericUpDown5.Visible = false;
            label7.Text = "从站地址：";
            groupBox3.Visible = false;
            label18.Visible = false;
            textBox5.Visible = false;
            c5_1.Visible = false;
            c5_2.Visible = false;
            c5_3.Visible = false;
            c5_4.Visible = false;
            c5_5.Visible = false;
            textBox2.MaxLength = 5;
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedItem.ToString() == "交通诱导灯" || comboBox1.SelectedItem.ToString() == "投影灯" || comboBox1.SelectedItem.ToString() == "天津五维地磁车检器") //htj：天津五维地磁车检器
            {   //丰海科技情报板、交通诱导灯、投影灯
                label11.Visible = true;
                numericUpDown4.Visible = true;
            }
            if (comboBox1.SelectedIndex == 2)
            {
                //应急电话
                label11.Visible = true;
                numericUpDown4.Visible = true;
                label3.Visible = false;
                numericUpDown1.Visible = false;
                label6.Text = "电话地址：";
            }
            if (comboBox1.SelectedItem.ToString() == "丰海科技情报板" || comboBox1.SelectedItem.ToString() == "交通诱导灯" || comboBox1.SelectedItem.ToString() == "投影灯")
            {
                label5.Text = "TCP客户端";
                label3.Visible = false;
                numericUpDown1.Visible = false;
            }
            else if (comboBox1.SelectedItem.ToString() == "消防火灾" || comboBox1.SelectedItem.ToString() == "车辆检测器" || comboBox1.SelectedItem.ToString() == "上海勋飞微波车检器" || comboBox1.SelectedItem.ToString() == "唯的美情报板")
            {
                label5.Text = "TCP服务器";
                if (comboBox1.SelectedItem.ToString() == "唯的美情报板")
                {
                    textBox2.Enabled = false;
                    label18.Visible = true;
                    textBox5.Visible = true;
                }
                if (comboBox1.SelectedItem.ToString() == "上海勋飞微波车检器")
                {
                    groupBox3.Visible = true;
                }
            }
            else if (comboBox1.SelectedItem.ToString() == "应急电话")
            {
                label5.Text = "UDP客户端";

            }
            else if (comboBox1.SelectedItem.ToString() == "可变信息标志")
            {
                label5.Text = "UDP服务器";
            }
            else if (comboBox1.SelectedItem.ToString() == "北京公科飞达交通应急广播")
            {
                label5.Text = "UDP服务器";
                label11.Visible = true;
                numericUpDown4.Visible = true;
                textBox2.Enabled = false;
                //label12.Visible = true;
                //numericUpDown5.Visible = true;
                button1.Text = "下一步";

            }
            else if(comboBox1.SelectedItem.ToString() == "天津五维地磁车检器")
            {
                label5.Text = "TCP客户端";
                label3.Visible = false;
                numericUpDown1.Visible = false;
                groupBox3.Visible = true;
                c5_1.Visible = true;
                c5_2.Visible = true;
                c5_3.Visible = true;
                c5_4.Visible = true;
                c5_5.Visible = true;
                textBox2.MaxLength = 8;


            }
        }

        private void add_Load(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = "1~" + numericUpDown5.Value;
        }
    }
}
