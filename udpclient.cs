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
   public class udpclient
    {
        public string id = "";
        public string addr = "";
        public string device_type = "";
        public string IP = "192.168.1.230";
        public int PORT = 5050;
        public int CPORT = 5060;
        public ArrayList bm_all = new ArrayList();
        public string am = "";
        public string aw = "";
       // public StringBuilder log = new StringBuilder();
        Thread acceptThread;
        UdpClient uc;
        IPEndPoint RemoteIpEndPoint;
        public int outtime = 0;
        public Timer timer;
        public int chaoshi = 0;
        public bool isrun = false;
        public udpclient()
        {
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        }
        public bool Client_Connect()
        {
            try
            {
                uc = new UdpClient(CPORT);
                isrun = true;
                acceptThread = new Thread(new ThreadStart(js));
                acceptThread.Start();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public void timeout()
        {
            setback("timeout", "", "", "", this);
        }
        public void close()
        {
            isrun = false;
            try
            {
                if (timer != null)
                    timer.Change(-1, -1);
                if(acceptThread!=null)
                acceptThread.Abort();

            }
            catch
            {

            }
            try
            {
                if(uc!=null)
                uc.Close();
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

                    byte[] receiveBytes = uc.Receive(ref RemoteIpEndPoint);    //获取接受到的信息,传的时候是以Byte[]来传的,所以接受时也要用Byte
                    string stripaddress = RemoteIpEndPoint.Address.ToString();    //发送方IP地址
                    string strPort = RemoteIpEndPoint.Port.ToString();
                   // setback("Receive", Class1.ZHTOAC(receiveBytes), stripaddress, strPort, this);
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
                    uc.Send(by, by.Length, ip, int.Parse(port));
                    return true;
                }
                else
                {
                    byte[] by = Class1.ZHAC(msg);
                    uc.Send(by, by.Length, ip, int.Parse(port));
                    return true;
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
        public delegate void Setback(string cmd, string data, string ip, string port, udpclient uc);
    }
}
