using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
   public class udpsever
    {
        UdpClient receivingUdpClient;
        public string id = "";
        public string addr = "";
        public string device_type = "";
        Thread acceptThread;
        public ArrayList client_all = new ArrayList();
        public int Port = 0;
      //  public StringBuilder log = new StringBuilder();
        public bool isrun = false;
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        public bool start_sever(int port)
        {
            try
            {
                Port = port;
                receivingUdpClient = new UdpClient(Port);
                acceptThread = new Thread(new ThreadStart(js));
                acceptThread.Start();
                isrun = true;
               // log.Append("类型：" + device_type + "的UDP服务器已打开监听\r\n");
                setback("打开成功","","", "", this);
                return true;
            }
            catch
            {
               // log.Append("类型：" + device_type + "的UDP服务器打开失败\r\n");
                setback("打开失败", "", "", "", this);
            }
            return false;
        }
        public void close()
        {
            isrun = false;
            try
            {
                
            acceptThread.Abort();
          
            }
            catch
            {

            }
            try
            {
                receivingUdpClient.Close();
            }
            catch
            { }
        }
        public void js()
        {

            while (isrun)         //循环扫描
            {
                try
                {

                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);    //获取接受到的信息,传的时候是以Byte[]来传的,所以接受时也要用Byte
                    string stripaddress = RemoteIpEndPoint.Address.ToString();    //发送方IP地址
                    string strPort = RemoteIpEndPoint.Port.ToString();
                 //   setback("Receive", Class1.ZHTOAC(receiveBytes), stripaddress, strPort, this);
                    setback("Receive16", Class1.ZHTO16(receiveBytes), stripaddress, strPort, this);
                   
                }
                catch
                { }
            }

        }
        public bool send(string msg, string ip, string port, bool is16)
        {
            bool tf = false;
            try
            {
                if (is16)
                {

                    byte[] by = Class1.ZH16(msg);
                    receivingUdpClient.Send(by, by.Length, ip, int.Parse(port));
                }
                else
                {
                    byte[] by = Class1.ZHAC(msg);
                    receivingUdpClient.Send(by, by.Length, ip, int.Parse(port));
                }
            }
            catch
            { }
            return tf;
        }
        public Setback setback;
        public void setOnSetback(Setback mlistener)
        {

            setback = mlistener;

        }
        public delegate void Setback(string cmd, string data,string ip,string port, udpsever us);
    }
}
