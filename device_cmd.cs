using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.parameter;

namespace WindowsFormsApp1
{
    public class device_cmd
    {
        /// <summary>
        /// C# List转换成DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static System.Data.DataTable ListToDataTable(IList list)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    //获取类型
                    Type colType = pi.PropertyType;
                    //当类型为Nullable<>时
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(pi.Name, colType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        public static string db(string addr16, string type, string data)
        {
            string str16 = addr16 + Class1.ZHTO16(Class1.ZHAC(type)) + Class1.ZHTO16(Class1.ZHAC(data));
            string mycrc = Class1.ZHTO16(Class1.crc16(Class1.ZH16("02" + str16)));
            string str = str16 + mycrc;
            string str1 = "";
            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str.Substring(i * 2, 2) == "02")
                {
                    str1 += "1BE7";
                }
                else if (str.Substring(i * 2, 2) == "03")
                {
                    str1 += "1BE8";
                }
                else if (str.Substring(i * 2, 2) == "1B")
                {
                    str1 += "1B00";
                }
                else
                {
                    str1 += str.Substring(i * 2, 2);
                }
            }
            return "02" + str1 + "03";
        }
        private static string cl(string data)
        {
            string str = data.Substring(2, data.Length - 2);
            string str1 = "";
            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str.Substring(i * 2, 4) == "1BE7")
                {
                    str1 += "02";
                    i++;
                }
                else if (str.Substring(i * 2, 4) == "1BE8")
                {
                    str1 += "03";
                    i++;
                }
                else if (str.Substring(i * 2, 4) == "1B00")
                {
                    str1 += "1B";
                    i++;
                }
                else
                {
                    str1 += str.Substring(i * 2, 2);
                }
            }
            return "02" + str1 + "03";
        }
        public static string write06(string id, string jcq, string value)
        {
            return "22F100000006" + id + "06" + jcq + value;
        }
        public static string write10(string id, string jcq, string value)
        {
            return "2C6D0000" + Class1.ZHTO16(value.Length / 2 + 7, 2) + id + "10" + jcq + Class1.ZHTO16(value.Length / 4, 2) + Class1.ZHTO16(value.Length / 2, 1) + value;
        }
        #region Information1

        public static bool Information1_write(Infomation1_data info, Socket socket)
        {
            try
            {
                byte[] by = Class1.ZH16(db(info.addr16, "24", info.speed_limit + info.typeface + info.red + "0" + info.light_regulate + info.light));
                socket.Send(by);
                byte[] by1 = Class1.ZH16(db(info.addr16, "55", info.onoff));
                socket.Send(by1);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool Information1_57(string addr16, Socket socket)
        {
            try
            {
                byte[] by = Class1.ZH16(db(addr16, "57", ""));
                socket.Send(by);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string Infomation1_write03(string id)
        {
            return "123600000006" + id + "03" + "0000000F";
        }
        public static Infomation1_readdata Information1_read(string data)
        {
            Infomation1_readdata info = null;
            try
            {
                if (data.Substring(0, 2) == "02" && data.Substring(data.Length - 2, 2) == "03")
                {

                    data = cl(data);
                    string mytype = Class1.ZHTOAC(Class1.ZH16(data.Substring(6, 4)));
                    if (mytype == "57")
                    {
                        string mydata = Class1.ZHTOAC(Class1.ZH16(data.Substring(10, 28)));
                        info = new Infomation1_readdata();
                        info.addr16 = data.Substring(2, 4);
                        info.speed_limit = mydata.Substring(0, 3);
                        info.typeface = mydata.Substring(3, 1);
                        info.red = mydata.Substring(4, 1);
                        info.light_regulate = mydata.Substring(6, 1);
                        info.fault = mydata.Substring(9, 2);
                        info.temp = data.Substring(22, 2);
                        info.light = mydata.Substring(12, 2);
                        return info;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {

            }
            return null;
        }

        public static Infomation1_data Information1_readaw(string aw, string addr)
        {
            Infomation1_data info = null;
            try
            {
                info = new Infomation1_data();
                info.addr16 = addr;
                info.speed_limit = Convert.ToInt32(aw.Substring(0, 4), 16) + "";
                info.typeface = Convert.ToInt32(aw.Substring(4, 4), 16) + "";
                info.red = Convert.ToInt32(aw.Substring(8, 4), 16) + "";
                info.light_regulate = Convert.ToInt32(aw.Substring(12, 4), 16) + "";
                info.light = Convert.ToInt32(aw.Substring(16, 4), 16) + "";
                info.onoff = Convert.ToInt32(aw.Substring(20, 4), 16) + "";
                return info;


            }
            catch
            {

            }
            return null;
        }
        public static string Infomation1_aw(Infomation1_data info)
        {
            return Class1.ZHTO16(int.Parse(info.speed_limit), 2) + Class1.ZHTO16(int.Parse(info.typeface), 2) + Class1.ZHTO16(int.Parse(info.red), 2) + Class1.ZHTO16(int.Parse(info.light_regulate), 2) + Class1.ZHTO16(int.Parse(info.light), 2) + Class1.ZHTO16(int.Parse(info.onoff), 2);
        }
        public static string Infomation1_bm(Infomation1_readdata info, string addr)
        {
            return addr + Class1.ZHTO16(int.Parse(info.speed_limit), 2) + Class1.ZHTO16(int.Parse(info.typeface), 2) + Class1.ZHTO16(int.Parse(info.red), 2) + Class1.ZHTO16(int.Parse(info.light_regulate), 2) + Class1.ZHTO16(int.Parse(info.fault), 2) + "00" + info.temp + Class1.ZHTO16(int.Parse(info.light), 2);
        }
        #endregion

        #region Fire
        public static string ACK()
        {
            return "0241343103";
        }
        public static string NAK()
        {
            return "024E345103";
        }
        public static string INIT()
        {
            return "0249343903";
        }
        public static string PING()
        {
            return "0250353003";
        }
        public static string WAIT()
        {
            return "0257353703";
        }
        public static string REJEJCT()
        {
            return "0258353803";
        }

        public static string Fire_write03(string id)
        {
            return "123600000006" + id + "03" + "0000000F";
        }
        public static string Fire_read(string data)
        {
            try
            {

                if (data.Substring(0, 2) == "02" && data.Substring(data.Length - 2, 2) == "03" && data.Substring(2, 2) == "44")
                {
                    string ss = "0000";
                    if (Class1.ZHTOAC(Class1.ZH16(data.Substring(66, 2))) == "N")
                    {
                        ss = "0001";
                    }

                    return data.Substring(4, 20) + Class1.ZHTO16(int.Parse(Class1.ZHTOAC(Class1.ZH16(data.Substring(34, 4)))), 2) + data.Substring(38, 28) + ss;
                }
            }
            catch
            {

            }
            return "";
        }

        #endregion

        #region phone
        public static string phone_write03(string id)
        {

            return "123600000006" + id + "03" + "00000005";
        }
        public static string phone_read(string data)
        {
            try
            {
                if (data.Substring(0, 4) == "BBBB" && data.Length >= (Convert.ToInt32(data.Substring(10, 2), 16) * 2 + 14))
                {

                    string mydata = data.Substring(14);
                    for (int i = mydata.Length; i < 4; i++)
                    {
                        mydata = "00" + mydata;
                    }
                    return data.Substring(6, 4) + "00" + data.Substring(10, 2) + "00" + data.Substring(12, 2) + mydata;
                }
            }
            catch
            {

            }
            return "";
        }
        #endregion

        #region car
        public static string car_write03(string id)
        {
            return "123600000006" + id + "03" + "00000049";
        }
        public static string car_read(string data, string addr)
        {

            try
            {
                if (data.Substring(0, 4) == "5731" && addr == data.Substring(8, 4))
                {
                    int carlen = Convert.ToInt32(data.Substring(30, 2), 16);
                    string mydata = addr + Class1.ZHTO16(carlen, 2);
                    for (int i = 0; i < carlen; i++)
                    {
                        int len1 = Convert.ToInt32(data.Substring(i * 24 + 30 + 20, 2), 16);
                        int len2 = Convert.ToInt32(data.Substring(i * 24 + 30 + 22, 2), 16);
                        mydata += data.Substring(i * 24 + 30, 20) + Class1.ZHTO16(len1, 2) + Class1.ZHTO16(len2, 2);
                    }
                    for (int i = carlen; i < 10; i++)
                    {
                        mydata += "00 00 00 00 00 00 00 00 00 00 00 00 00 00".Replace(" ", "");
                    }
                    return mydata;
                }
            }
            catch
            {

            }
            return "";
        }
        #endregion

        #region VMS

        public static string VMS_write03(string id)
        {
            return "123600000006" + id + "03" + "0000000A";
        }

        public static string VMS_read_light(string data)
        {
            try
            {
                if (data.Substring(0, 2) == "02" && data.Substring(6, 4) == "3036")
                {
                    string lighttype = data.Substring(10, 2);
                    if (lighttype == "01")
                    {
                        lighttype = "31";
                    }
                    if (lighttype == "00")
                    {
                        lighttype = "30";
                    }
                    return Class1.ZHTO16(int.Parse(Class1.ZHTOAC(Class1.ZH16(lighttype))), 2) + Class1.ZHTO16(int.Parse(Class1.ZHTOAC(Class1.ZH16(data.Substring(12, 4)))), 2);
                }
            }
            catch
            {

            }
            return "";
        }

        public static string VMS_write_bmp(string data, string addr)
        {
            data = data.Replace(" ", "");
            data = Class1.ZHTO16(Class1.ZHAC(data));
            string ss = "02" + addr + "3130706c61792e6c73742b000000005b506c61796c6973745d0d0a4974656d5f4e6f3d310d0a4974656d303d313030302c312c312c5c433030303030305c42" + data + "0d0a2b8903".ToUpper();
            return ss;
        }
        public static string VMS_write_readlight(string addr)
        {
            return "02" + addr + "3036303003";
        }

        public static string VMS_write_setlighttype(int data, string addr)
        {
            return "02" + addr + "3034" + Class1.ZHTO16(data, 1) + "303003";
        }
        public static string VMS_write_setlight(int data, string addr)
        {
            string ld = Class1.ZHTO16(Class1.ZHAC(data.ToString("00")));
            return "02" + addr + "3035" + ld + "30303030303003";
        }
        #endregion
        #region 交通诱导灯
        public static string lead_write03(string id)
        {
            return "123600000006" + id + "03" + "00000003";
        }

        public static string lead_write_setlight(int data)
        {
            if (data == 1)
            {
                return "FFFF070101010006";
            }
            if (data == 2)
            {
                return "FFFF070101020005";
            }
            if (data == 3)
            {
                return "FFFF070101030004";
            }
            if (data == 4)
            {
                return "FFFF070101040003";
            }
            return "";
        }
        #endregion

        #region 交通诱导灯
        public static string LAMP_write03(string id)
        {
            return "123600000006" + id + "03" + "00000003";
        }

        public static string LAMP_write_setlight(int data)
        {
            if (data == 1)
            {
                return "FFFF000001010000";
            }
            if (data == 2)
            {
                return "FFFF000001020003";
            }
            if (data == 3)
            {
                return "FFFF000001030002";
            }
            if (data == 4)
            {
                return "FFFF000001040005";
            }
            if (data == 5)
            {
                return "FFFF000001050004";
            }
            if (data == 0)
            {
                return "FFFF000001060007";
            }
            return "";
        }
        #endregion

        public static string view_write_set(string data, string cardid)
        {
            string str = "6832" + cardid + "7B 01 04 00 00 00" + "08 00 01 " + data;
            str = str.Replace(" ", "");
            str = "FFFFFFFF" + "0F00" + "0000" + str + Class1.ZHTO16(Class1.ADD16(Class1.ZH16(str)));
            return str;
        }
        public static string view_write_set_gb(string data, string cardid)
        {
            //20230906根据粟工要求修改03 02 FF FF FF改为03 04 FF FF FF
            string str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 04 FF FF FF" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            //原正确代码
            //string str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 02 FF FF FF" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            str = str.Replace(" ", "");
            str = "FFFFFFFF" + Class1.tz(Class1.ZHTO16((Class1.ZH16(str).Length + 2), 2)) + "0000" + str + Class1.ZHTO16(Class1.ADD16(Class1.ZH16(str)));
            return str;
        }
        public static string view_write03(string id)
        {
            //20230906修改
            //return "123600000006" + id + "03" + "00000045";
            //20230911修改
            return "123600000006" + id + "03" + "00000047";
        }
        public static string view_read(string data, string addr, string aw)
        {
            data = data.ToUpper();
            try
            {
                if (data.Substring(16, 4) == "E832" && "7B" == data.Substring(22, 2) && "00" == data.Substring(24, 2))
                {
                    // string num = data.Substring(40, 2);
                    return addr + "0001" + aw;

                }
            }
            catch
            {

            }
            return "";
        }
        public static string view_ping()
        {
            return "ff ff ff ff 08 00 00 00 68 32 ff 70 01 01 0b 02".Replace(" ", ""); ;
        }
        public static string view_pingread(string data)
        {
            data = data.ToUpper();
            try
            {
                if (data.Substring(16, 4) == "E832")
                {

                    return data.Substring(22, 2);

                }
            }
            catch
            {

            }
            return "";
        }


        public static string broadcast_write03(string id)
        {
            return "123600000006" + id + "03" + "0000004F";
        }
        public static List<string> broadcast_read(string data)
        {   //北京公科飞达交通应急广播
            List<string> back = Class1.ZHTOGB(Class1.ZH16(data)).Split('#').ToList(); //#是发送报文尾或回复报文间隔
            for (int l = 0; l < back.Count; l++)
            {
                string[] member = back[l].Split(',');   //各数据段由半角逗号隔开
                if (member.Length >= 9) //完整的报文数据段至少9个（包括#）
                {
                    //if (member[0] == "ET" && member[2] == "S")
                    //{
                    if (member[0] == "ET")  //字段[0]标识符，2字符
                    {
                        int b = Class1.ZHGB(member[5]).Length;  //字段[5]分机名称？大于0字符
                        for (int i = b; i < 64; i++)
                        {
                            member[5] = member[5] + " ";    //不足64长度则后边补空格
                        }
                        int a = Class1.ZHGB(member[6]).Length;  //字段[6]分机桩号?大于0字符
                        for (int i = a; i < 64; i++)
                        {
                            member[6] = member[6] + " ";    //不足64长度则后边补空格
                        }
                        for (int i = member[4].Length; i < 4; i++)  //字段[4]分机类型？1字符，“1”----电话；“2”----广播。“3”----FM
                        {
                            member[4] = "0" + member[4]; //长度不足4则前端补0
                        }
                        int n = member[5].Length;
                        int n1 = member[6].Length;
                        if (member[7] == "")    //字段[7]报文数据，无数据则补“0”
                        {
                            member[7] = "0";
                        }
                        else
                        {
                            member[7] = member[7].Substring(0, 1); //字段[7]报文数据，有数据取第一个？
                        }

                        //htj:20230912，控制命令在《监控系统联动协议》的2009版与2020版中是不一致的
                        //2009版：A=告警，故障代码；H=挂机，通话时长[（分钟），十进制]，C=0,L=0
                        //2020版：S=状态，状态码。M=采播，0或1，启动或停止终端设备采播；W=声卡播放，0或1，启动或停止声卡设备采播
                        //F=录播,录音文件编号,大于“1”的是录音文件编号，表示开始录播,“0”表示停止录播；默认循环播放。
                        //C=呼叫,0
                        //字段[3]分机类型，1字符：1=电话，2=广播，3=FM
                        //字段[1]时间戳，19字符
                        //字段[2]控制命令，1字符。从设备接收的控制指令：告警“A”，呼叫“C”，摘机“L”，挂机“H”。状态“S”。发送到设备的控制指令：采播“M”，声卡播放“W”，录播“F”，呼叫“C”
                        //字段[4]分机编号，4字符
                        //字段[5]分机名称，64字符
                        //字段[6]分机桩号，64字符
                        //字段[7]报文数据，1字符，可能不是1个字符。如故障码可以串。
                        //控制命令为告警“A”时的报文数据：
                        //“0”----正常；
                        //“1”----话路故障；
                        //“2”----电池欠压；
                        //“3”----开门报警；
                        //“4”----通讯故障；
                        //“5”----麦克风故障；
                        //“6”----扬声器故障；
                        //“7”----功放故障；
                        //“9”----其它故障。
                        //故障可以叠加，如：“23”表示欠压且开门。
                        //控制命令为呼叫“C”时的报文数据：“0”
                        //控制命令为摘机“L”时的报文数据：“0”
                        //控制命令为挂机“H”时的报文数据：以分钟为单位，如：“18”表示该分机此次通话时长为18分钟。
                        //控制命令为状态“S”时的报文数据：
                        //“0”----正常；
                        //“1”----通话中 / 广播中
                        //“2”----开门报警 / 功放故障
                        //“3”----监听
                        //“9”----中断、离线
                        //控制命令为采播“M”时的报文数据：“0”或“1”。“1”表示启动终端设备采播；“0”表示停止终端设备采播；
                        //控制命令为声卡播放“W”时的报文数据：“0”或“1”。“1”表示启动声卡设备采播；“0”表示停止终端设备采播；
                        //控制命令为录播“F”时的报文数据：大于“1”的是录音文件编号，表示开始录播；“0”表示停止录播；默认循环播放。

                        //旧的代码
                        //if (member[2] == "A")   //字段[2]控制命令，1字符。
                        //{   //字符数：19 + 2 + 1 + 4 + 64 + 64 + 1 + n + 1 + 1
                        //    back[l] = member[1] + " 0" + member[3] + member[4] + member[5] + member[6] + "0" + member[7] + member[2] + " ";
                        //}
                        //else
                        //{   //字符数：19 + 2 + 1 + 4 + 64 + 64 + 1 + n + 1 + 1 + 1 + 1
                        //    back[l] = member[1] + " 0" + member[3] + member[4] + member[5] + member[6] + "0" + member[7] + member[2] + " " + member[2] + " ";
                        //}
                        //htj://20230913修改，新的代码，改成传一样的内容？
                        if (member[2] == "A")   //字段[2]控制命令，1字符。
                        {   //字符数：19 + 2 + 1 + 4 + 64 + 64 + 1 + n + 1 + 1
                            back[l] = member[1] + " 0" + member[3] + member[4] + member[5] + member[6] + "0" + member[7] + member[2] + " ";
                        }
                        else
                        {   //字符数：19 + 2 + 1 + 4 + 64 + 64 + 1 + n + 1 + 1
                            back[l] = member[1] + " 0" + member[3] + member[4] + member[5] + member[6] + "0" + member[7] + member[2] + " " + member[2] + " ";
                        }
                    }
                    else
                    {
                        back.RemoveAt(l);
                        l--;
                    }
                }
                else
                {
                    back.RemoveAt(l);
                    l--;
                }
            }
            return back;
        }
        public static string MOT_write03(string id)
        {
            return "123600000006" + id + "03" + "00000061";
        }
        /// <summary>
        /// 16进制字符串转10进制float(4字节)
        /// </summary>
        /// <param name="floatStr16">例如:41A40000</param>
        /// <returns></returns>
        public static float HexString2FloatDec(string floatStr16)
        {
            UInt32 a = Convert.ToUInt32(floatStr16, 16);
            return BitConverter.ToSingle(BitConverter.GetBytes(a), 0);
        }

        /// <summary>
        /// 10进制float(4字节)转16进制字符串
        /// </summary>
        /// <param name="decF">例如:20.5f</param>
        /// <returns></returns>
        public static string FloatDec2HexString(float decF)
        {


            byte[] bytes = BitConverter.GetBytes(decF);

            string hex = BitConverter.ToString(bytes).Replace("-", "");
            string str = Class1.tz(hex);
            return str.Substring(4, 4) + str.Substring(0, 4);
        }
        public static string MOT_read(string data)
        {
            if (data.Substring(0, 4) == "FFF9" && data.Length >= 64)
            {
                float jj = Convert.ToInt32(data.Substring(52, 4), 16) * 0.1f;
                float cc = Convert.ToInt32(data.Substring(56, 4), 16) * 0.1f;
                float yzl = Convert.ToInt32(data.Substring(60, 4), 16) * 0.1f;
                return "00" + data.Substring(14, 2) + data.Substring(16, 24) + "00" + data.Substring(40, 2) + "00" + data.Substring(42, 2) + "00" + data.Substring(44, 2) + "00" + data.Substring(46, 2) + "00" + data.Substring(48, 2) + "00" + data.Substring(50, 2) + FloatDec2HexString(jj) + FloatDec2HexString(cc) + FloatDec2HexString(yzl);
            }
            else
            {
                return "";
            }

        }
        public static string MOT_PING()
        {
            return "";
        }

        public static string car5_write03(string id)
        {
            return "123600000006" + id + "03" + "0000002D";
        }
        public static car5_readdata car5_read(string data)
        {
            // data字符串：每两个字符对应数据的一个Byte，前16Byte比较固定，依次：
            //名称        英文名称    长度(BYTE)    说明
            //起始字       STX         1            0x88
            //业务类型     TYPE        1            与发送包类型相同
            //路段代码     HSC         8            与发送包相同
            //数据长度     DL          2            1
            //传送组号     CN          2            与发送包相同
            //结束字       ETX         1            0x99
            //校验字       CRC         1            从STX到ETX的XOR校验和DATA的XOR校验。不包括STX和ETX。
            
            //数据         DATA        1            0x99—OK，0x00失败

            //数据DATA的第1个字节是数据命令码（DATA_TYPE）,16进制：字符串（32,2)
            //数据DATA的第2个字节是输出顺序码（1-32，其它无效）（十六进制范围是01-20，十进制范围是01-32）,16进制：字符串（34,2)
            //数据DATA的第3-5字节是车道代码，10进制？：字符串（36,6)
            //数据DATA的第6-13字节是地磁检测器代码，10进制？：字符串（42,16)
            //后续字节根据命令码确定具体数据内容。复合发送时，要必须按照序号顺序发送
            //车头时距，（58，8）。数据命令码是0x01时。数据类型是32位int型。
            //占有时间，（58，8）。数据命令码是0x02时。数据类型是32位int型
            //速度（则前面有占有时间）。（66，8）。数据命令码是0x05时。数据类型是32位float型
            try
            {
                string tou = data.Substring(0, 4);              //起始字STX 1Byte、业务类型TYPE（命令码） 1Byte，8881
                int len = Convert.ToInt32(Class1.tz(data.Substring(20, 4)), 16);    //数据长度DL 2Byte。数据起始位置是第16Byte（32字符），基于0
                if (data.Substring(0, 4) == "8881" && data.Length >= len)   //？头部与长度符合要求，数据长度DL不是整个包的长度？
                {
                    car5_readdata c5 = new car5_readdata();
                    //htj：旧的两句转换了，比较失败
                    //c5.bh = Class1.ZHTOAC(Class1.ZH16(data.Substring(4, 16)));  //路段代码HSC（设备编号？） 8Byte，如："00216002"
                    //c5.cd = Class1.ZHTOAC(Class1.ZH16(data.Substring(36, 6)));  //车道代码，3Byte，二进制串的ASCII？如31 30 31表示101？，如："101"
                    //htj：shiyong1bujiexi1
                    c5.bh = data.Substring(4, 16);  //路段代码HSC（设备编号？） 8Byte，如："3030323136303032"
                    c5.cd = data.Substring(36, 6);  //车道代码，3Byte，二进制串的ASCII？如313031表示101？，如："313031"
                    byte nTYPE = Convert.ToByte(data.Substring(32, 2), 16);     //数据命令码，1Byte（数据起始位置是第16Byte）
                    if ((nTYPE & 0x01) == 0x01)     //车头时距，0x01，数据Long型4Byte
                    {
                        //有车处理
                        //c5.ctsj = Class1.tz(data.Substring(50, 8));     //？
                        //c5.ctsj = Class1.tz(data.Substring(58, 8));     //如："901A0000",tz后成为"00001A90"
                        c5.ctsj = Class1.w5_02134657(data.Substring(58, 8));    //"901A0000"转为"1A900000"

                        //原始串
                        string orgstr = data.Substring(58, 8);
                        //long orglong = long.Parse(orgstr, System.Globalization.NumberStyles.AllowHexSpecifier);
                        //long c5ctsj = long.Parse(c5.ctsj, System.Globalization.NumberStyles.AllowHexSpecifier);

                        //原始串字节逆序
                        string reorgstr = Class1.w5_06172435(orgstr);
                        //原始串字节逆序转换成long
                        long reorglong = long.Parse(reorgstr, System.Globalization.NumberStyles.AllowHexSpecifier);
                        reorglong *= 10;    //10倍（设备显示是10倍，所以乘以10）
                        string x10str = Class1.ZHTO16((int)reorglong, 4); //*10转换字符串
                        string x10orgstr = Class1.w5_06172435(x10str);    //将*10转换字符串字节倒序，算成是*10原始字符串
                        c5.ctsj = Class1.w5_02134657(x10orgstr);    //再w5_02134657作为ctsj

                        long x10 = long.Parse(x10str, System.Globalization.NumberStyles.AllowHexSpecifier);

                    }
                    if ((nTYPE & 0x02) == 0x02)     //占有时间，0x02，数据Long型4Byte
                    {
                        //c5.time = Class1.tz(data.Substring(50, 8));     //？
                        //无车处理
                        //c5.time = Class1.tz(data.Substring(58, 8));     //"46000000",tz后成为"00000046"
                        c5.time = Class1.w5_02134657(data.Substring(58, 8));    //"46000000"转为"00460000"

                        //原始串
                        string orgstr = data.Substring(58, 8);
                        //原始串字节逆序
                        string reorgstr = Class1.w5_06172435(orgstr);
                        //原始串字节逆序转换成long
                        long reorglong = long.Parse(reorgstr, System.Globalization.NumberStyles.AllowHexSpecifier);
                        reorglong *= 10;    //10倍（设备显示是10倍，所以乘以10）
                        string x10str = Class1.ZHTO16((int)reorglong, 4); //*10转换字符串
                        string x10orgstr = Class1.w5_06172435(x10str);    //将*10转换字符串字节倒序，算成是*10原始字符串
                        c5.time = Class1.w5_02134657(x10orgstr);    //再w5_02134657作为ctsj
                        
                        long x10 = long.Parse(x10str, System.Globalization.NumberStyles.AllowHexSpecifier);
                    }
                    if ((nTYPE & 0x04) == 0x04)     //车速，0x04，数据Float型，4Byte
                    {
                        // c5.speed = data.Substring(58, 8);               //？
                        //本包带有速度数据
                        //c5.speed = data.Substring(66, 8);               //如：BAE80242
                        c5.speed = Class1.w5_02134657(data.Substring(66, 8));   //"3A83B542"转成833A42B5。
                    }
                    //if ((nTYPE & 0x08) == 0x08)     //排队长度，0x08，到停车线距离。4Byte
                    //{

                    //}
                    //if ((nTYPE & 0x10) == 0x10)     //闯红灯触发，0x10，4Byte
                    //{

                    //}
                    //if ((nTYPE & 0x20) == 0x20)    //满车触发，0x20，4Byte
                    //{

                    //}
                    //if ((nTYPE & 0x40) == 0x40)    //故障类型，0x40，4Byte
                    //{

                    //}
                    return c5;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        //20230906根据粟工要求增加
        //带颜色显示
        public static string view_write_set_gb_RGY(string data, string cardid, string a)
        {
            string str = "0000    ";
            if (a == "0") //白色
            {
                str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 04 FF FF FF" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            }
            if (a == "1") //红色
            {
                str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 04 FF 00 00" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            }
            if (a == "2") //绿色
            {
                str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 04 00 FF 00" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            }

            if (a == "3") //黄色
            {
                str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 04 FF FF 00" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            }

            str = str.Replace(" ", "");
            str = "FFFFFFFF" + Class1.tz(Class1.ZHTO16((Class1.ZH16(str).Length + 2), 2)) + "0000" + str + Class1.ZHTO16(Class1.ADD16(Class1.ZH16(str)));
            return str;
        }
        //20230911根据粟工要求增加
        public static string view_write_set_gb_RGY(string data, string cardid, string a, string hSiz)
        {
            //string hSize_t = "000   ";
            string str = "0000    ";




            if (a == "0") //白色
            {
                str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 0" + hSiz + "FF FF FF" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            }
            if (a == "1") //红色
            {
                str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 0" + hSiz + "FF 00 00" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            }
            if (a == "2") //绿色
            {
                str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 0" + hSiz + "00 FF 00" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            }

            if (a == "3") //黄色
            {
                str = "6832" + cardid + "7B 01 " + Class1.tz(Class1.ZHTO16((Class1.ZHGB(data).Length + 12), 2)) + " 00 00" + "12 00 00 00 01 00 03 0" + hSiz + "FF FF 00" + Class1.ZHTO16(Class1.ZHGB(data)) + "00";
            }

            str = str.Replace(" ", "");
            str = "FFFFFFFF" + Class1.tz(Class1.ZHTO16((Class1.ZH16(str).Length + 2), 2)) + "0000" + str + Class1.ZHTO16(Class1.ADD16(Class1.ZH16(str)));
            return str;
        }

    }
}
