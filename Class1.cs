using HslCommunication.Serial;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Class1
    {
        public static byte[] ZHAC(string str)   //把Ascii转化为Byte
        {
            byte[] dz = Encoding.ASCII.GetBytes(str);
            return dz;
        }
        public static byte[] ZHGB(string str)   //字符串转GB2312
        {
            byte[] dz = Encoding.GetEncoding("GB2312").GetBytes(str);
            return dz;
        }
        public static byte[] ZH16(string DZ) //将字符串转成十六进制字字节数组？
        {
            DZ = DZ.Replace(" ", ""); //先去掉空格
            int len1 = DZ.Length / 2;
            byte[] ret = new byte[len1];
            for (int i = 0; i < len1; i++)
            {   //十六进制字符串（去除空格）后，（两个字符转一个字节）转成字节数组
                ret[i] = Convert.ToByte(DZ.Substring(i * 2, 2), 16);
            }
            return ret;
        }


        public static void writeACNO(string str, Socket sp) //转ASCII编码发送
        {
            byte[] by = ZHAC(str);
            sp.Send(by);

        }
        public static string tz(string ret)  //倒序，没两字符是补零16进制字符
        {
            byte[] by = ZH16(ret);
            byte[] by1 = new byte[by.Length];
            for (int i = 0; i < by.Length; i++)
            {
                by1[i] = by[by.Length - 1 - i];  //?
            }
            return ZHTO16(by1);
        }
        public static void write16NO(string str, Socket sp)  //发送字节数组？
        {
            byte[] by = ZH16(str);
            sp.Send(by);
        }

        public static string ZHTOAC(byte[] str) //转成ASCII串？
        {
            string dz = Encoding.ASCII.GetString(str).Replace("\0", "\\0");
            return dz;
        }
        public static string ZHTOGB(byte[] str) //转成GB2312串？
        {
            string dz = Encoding.GetEncoding("GB2312").GetString(str);
            return dz;
        }
        public static string ZHTO16(byte[] sz)  //？每个字节转成两个十六进制字符（前补零）
        {
            string ret = "";
            foreach (byte b in sz)
            {
                if (b.ToString("X").Length > 1)
                {
                    ret += b.ToString("X");
                }
                else
                {
                    ret += "0" + b.ToString("X");
                }
            }
            return ret;
        }
        public static string ZHTO161(byte[] sz) //空格隔开的十六进制字符？
        {
            string ret = "";
            foreach (byte b in sz)
            {
                if (b.ToString("X").Length > 1)
                {
                    ret += b.ToString("X") + " ";
                }
                else
                {
                    ret += "0" + b.ToString("X") + " ";
                }
            }
            return ret;
        }
        public static string ZHTO16(int sz, int bytes)  //（前端补零的16进展字符串），1Byte=>2字符
        {   //将整数sz转换成bytes字节的字符串（1Byte=>2字符），不足则前边补0
            string ret = sz.ToString("X");
            for (int i = ret.Length; i < bytes * 2; i++)
            {

                ret = "0" + ret;

            }
            return ret;
        }
        public static string yh(string ret) //异或？
        {
            byte[] by = ZH16(ret);
            int xay = 0;
            for (int i = 0; i < by.Length; i++)
            {
                xay ^= by[i];
            }
            if (xay.ToString("X").Length > 1)
            {
                return xay.ToString("X");
            }
            else
            {
                return "0" + xay.ToString("X");
            }
        }

        public static void set_font_color(RichTextBox log, string str, Color color, Font ft)
        {
            int len = log.Text.Length;
            log.AppendText(str);
            log.Select(len, str.Length);
            log.SelectionColor = color;
            log.SelectionFont = ft;
            log.Focus();

        }
        public static byte[] ADD16(byte[] memorySpage)
        {
            int num = 0;
            for (int i = 0; i < memorySpage.Length; i++)
            {
                num = (num + memorySpage[i]) % 0xffff;
            }
            //实际上num 这里已经是结果了，如果只是取int 可以直接返回了
            memorySpage = BitConverter.GetBytes(num);
            //返回累加校验和
            byte[] by = new byte[] { memorySpage[0], memorySpage[1] };
            return by;
        }
        /// <summary>
        /// 累加校验和
        /// </summary>
        /// <param name="memorySpage">需要校验的数据</param>
        /// <returns>返回校验和结果</returns>
        public static int ADD8_Add(byte[] memorySpage)
        {
            int sum = 0;
            for (int i = 0; i < memorySpage.Length; i++)
            {
                sum += memorySpage[i];
            }
            sum = sum & 0xff;
            //返回累加校验和
            return sum;
        }
        public static void up_font_color(RichTextBox log, string str, Color color, Font ft)
        {
            int len = log.Text.Length;

            log.Select(len - str.Length - 1, str.Length);
            log.SelectionColor = color;
            log.SelectionFont = ft;
            log.Focus();

        }
        public static byte[] crc16(byte[] data)
        {
            return SoftCRC16.CRC16(data);
        }

        //htj: ASCII字符串转成字符串
        public static string getASCIItoStr(string str)
        {
            byte[] bb = Hex2Bytes(str, false);
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            string strCharacter = asciiEncoding.GetString(bb);
            Console.WriteLine(strCharacter);
            return strCharacter;
        }
        //先将16进制ASCII码转字节数组
        public static byte[] Hex2Bytes(string sHex, bool isExchange)
        {
            if (sHex == null || sHex.Length == 0)
                return null;
            sHex = sHex.Length % 2 == 0 ? sHex : "0" + sHex;
            byte[] bRtns = new byte[sHex.Length / 2];
            for (int i = 0; i < bRtns.Length; i++)
            {
                if (isExchange)
                    bRtns[bRtns.Length - 1 - i] = Convert.ToByte(sHex.Substring(i * 2, 2), 16);
                else
                    bRtns[i] = Convert.ToByte(sHex.Substring(i * 2, 2), 16);
            }
            return bRtns;
        }
        //普通字符串转16进制ASCII码        
        public static string toASCII(string code)
        {
            char[] cs = code.ToCharArray();//先转字节数组
            string Hstr = null;
            for (int l = 0; l < cs.Length; l++)
            {
                Hstr += ((int)cs[l]).ToString("X");
            }
            //System.Console.WriteLine(Hstr);
            return Hstr;
        }
        //针对天津五维地磁车检器的转换（s[0]<==>s[2]，s[1]<==>s[3]，s[4]<==>s[6]，s[5]<==>s[7]）
        public static string w5_02134657(string s)
        {
            string ret = "";
            if (s.Length >= 8)
            {
                ret += s[2];
                ret += s[3];
                ret += s[0];
                ret += s[1];
                ret += s[6];
                ret += s[7];
                ret += s[4];
                ret += s[5];
            }

            return ret;
        }
        public static string w5_06172435(string s)
        {
            string ret = "";
            if (s.Length >= 8)
            {
                ret += s[6];
                ret += s[7];
                ret += s[4];
                ret += s[5];
                ret += s[2];
                ret += s[3];
                ret += s[0];
                ret += s[1];
            }

            return ret;
        }
    }
}
