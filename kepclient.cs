using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace WindowsFormsApp1
{
   public class kepclient
    {
        public string ip = "";
        public int port ;
        public string id = "";
        public string device_type = "";
        public ArrayList bm_all = new ArrayList();
        public string am = "";
        public string aw = "";
      //  public StringBuilder log = new StringBuilder();
        public Socket socket;
        Thread acceptThread;
        TcpClient tc;
        public Timer timer;
        public int times = 100;
        public kepclient()
        {
           
        }
        public void Client_Connect(string ip, int port)
        {
            try
            {
                tc = new TcpClient();
                tc.BeginConnect(ip, port, new AsyncCallback(isConnect), tc);

            }
            catch
            {

            }

        }

        public bool isrun = false;
        public bool isagain = true;
        public void Client_Close()
        {
            try
            {    
                timer.Change(-1, -1);
                isrun = false;
                isagain = false;
                if (acceptThread != null)
                    acceptThread.Abort();
                tc.Close();
                if (socket != null)
                    socket.Close();
            }
            catch
            {

            }
        }
        public void isConnect(IAsyncResult iar)
        {
            try
            {
                if (tc.Client != null)
                {
                    if (tc.Connected)
                    {
                        tc.EndConnect(iar);
                        isrun = true;
                        acceptThread = new Thread(new ThreadStart(yb));
                        acceptThread.Start();
                        timer.Change(100, times);
                        setback("Connectting", "", this);
                    }
                    else
                    {
                        setback("Connect_error", "", this);
                    }
                }
            }
            catch
            { }
        }
        public void yb()
        {
            socket = tc.Client;
            string myip = socket.RemoteEndPoint.ToString();
            try
            {
                while (isrun)
                {
                    if (socket.Poll(-1, SelectMode.SelectRead))
                    {
                        if (socket.Connected && socket.Available > 0)
                        {
                            byte[] message = new byte[socket.Available];
                            socket.Receive(message);
                            setback("Receive", Class1.ZHTOAC(message), this);
                            setback("Receive16", Class1.ZHTO16(message), this);
                        }
                        else
                        {

                            socket.Close();
                            tc.Close();
                            isrun = false;
                            setback("掉线", "", this);
                            return;
                        }

                    }
                    else
                    {

                        socket.Close();
                        tc.Close();
                        isrun = false;
                        setback("掉线", "", this);
                        return;
                    }

                }
            }
            catch
            {
                isrun = false;
                setback("掉线", "", this);
            }
        }
        public bool send(string msg, bool is16)
        {
            bool tf = false;

            if (socket != null && socket.Connected)
            {
                if (is16)
                    Class1.write16NO(msg, socket);
                else
                    Class1.writeACNO(msg, socket);
                tf = true;
            }
            return tf;
        }
        public Setback setback;
        public void setOnSetback(Setback mlistener)
        {

            setback = mlistener;

        }
        public delegate void Setback(string cmd, string data, kepclient tl);
    }
}
