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
   public class tcpsever
    {
        public string id = "";
        public string addr= "";
        public string device_type = "";
        TcpListener mytcpsever;
        Thread tcpseverThread;
        public ArrayList client_all = new ArrayList();
        public int Port=0;
       // public StringBuilder log = new StringBuilder();
        public bool start_sever(int port)
        {
            try
            {
                Port = port;
                mytcpsever = new TcpListener(System.Net.IPAddress.Any, port);
                mytcpsever.Start();
                isrun = true;
                tcpseverThread = new Thread(() => tcpsever_run());
                tcpseverThread.IsBackground = true;
                tcpseverThread.Start();           
                return true;
            }
            catch
            {

            }
            return false;
        }
       public bool isrun = false;
        public void stop_sever()
        {
            try
            {
                isrun = false;            
                mytcpsever.Stop();
                tcpseverThread.Abort();
                for (int i = 0; i < client_all.Count; i++)
                {
                    Socket socket = (Socket)client_all[i];
                    socket.Close();
                }
                client_all.Clear();
            }
            catch
            {

            }
        }
        /// <summary>
        /// 检测客户端连接请求线程
        /// </summary>
        public void tcpsever_run()
        {
            while (isrun) //循环侦听
            {
                try
                {
                    //treeView1.Nodes.Clear();
                    Socket socket = mytcpsever.AcceptSocket();
                    client_all.Add(socket);
                    setback("add", socket.RemoteEndPoint.ToString(), socket.RemoteEndPoint.ToString().Split(':')[0], socket.RemoteEndPoint.ToString().Split(':')[1], this);
                    Thread acceptThread1 = new Thread(() => tcpsever_Receive(socket));
                    acceptThread1.IsBackground = true;
                    acceptThread1.Start();
                }
                catch
                { }
            }

        }
        public Socket getclient(string ip)
        {
            for(int i=0;i< client_all.Count;i++)
            {
                try
                {
                    Socket so = (Socket)client_all[i];
                    if (so.RemoteEndPoint.ToString().Split(':')[0]==ip)
                    {
                        return so;
                    }
                }
                catch
                { }
             
            }
            return null;
        }
        public void removeclient(string ip)
        {
            for (int i = 0; i < client_all.Count; i++)
            {
                try
                {
                    Socket so = (Socket)client_all[i];
                    if (so.RemoteEndPoint.ToString().Split(':')[0] == ip)
                    {
                        client_all.RemoveAt(i);
                        i--;
                    }
                }
                catch
                { }

            }

        }
        /// <summary>
        /// 接收客户端数据线程
        /// </summary>
        /// <param name="socket">客户端socket</param>
        public void tcpsever_Receive(Socket socket)
        {
            
            string myip = socket.RemoteEndPoint.ToString();
            try
            {
                while (isrun) //循环侦听
                {
                    if (socket.Poll(-1, SelectMode.SelectRead))
                    {
                        try
                        {
                            byte[] by = new byte[0];
                            socket.Receive(by);
                        }
                        catch
                        {
                            //掉线事件
                            for (int i = 0; i < client_all.Count; i++)
                            {
                                Socket mysocket = (Socket)client_all[i];
                                if (mysocket == null || !mysocket.Connected || myip == ((Socket)client_all[i]).RemoteEndPoint.ToString())
                                {
                                  
                                    client_all.RemoveAt(i);
                                    i--;
                                    try
                                    {

                                        StopServerConnet(mysocket);


                                    }
                                    catch
                                    { }
                                }
                            }
                
                            setback("掉线", myip, myip.Split(':')[0], myip.Split(':')[1], this);

                            return;
                        }


                        if (socket.Available > 0)
                        {
                            try
                            {
                                byte[] by1 = new byte[socket.Available];
                                socket.Receive(by1);
                                //setback("Receive", Class1.ZHTOAC(by1),myip.Split(':')[0], myip.Split(':')[1], this);
                                setback("Receive16", Class1.ZHTO16(by1), myip.Split(':')[0], myip.Split(':')[1], this);
                            }
                            catch
                            { }
                        }
                        else
                        {
                            //掉线事件
                            for (int i = 0; i < client_all.Count; i++)
                            {
                                Socket mysocket = (Socket)client_all[i];
                                if (mysocket == null || !mysocket.Connected || myip == ((Socket)client_all[i]).RemoteEndPoint.ToString())
                                {
                                   
                                    client_all.RemoveAt(i);
                                    i--;
                                    try
                                    {

                                        StopServerConnet(mysocket);

                                    }
                                    catch
                                    { }
                                }
                            }

                            setback("掉线", myip, myip.Split(':')[0], myip.Split(':')[1], this);
                  
                            return;
                        }


                    }
                    else
                    {
                        //掉线事件
                        for (int i = 0; i < client_all.Count; i++)
                        {
                            Socket mysocket = (Socket)client_all[i];
                            if (mysocket == null || !mysocket.Connected || myip == ((Socket)client_all[i]).RemoteEndPoint.ToString())
                            {
                              
                                client_all.RemoveAt(i);
                                i--;
                                try
                                {

                                    StopServerConnet(mysocket);

                                }
                                catch
                                { }
                            }
                        }
                        
                            setback("掉线", myip, myip.Split(':')[0], myip.Split(':')[1], this);
                      
                        return;
                    }
                }
            }
            catch
            {
                for (int i = 0; i < client_all.Count; i++)
                {
                    Socket mysocket = (Socket)client_all[i];
                    if (mysocket == null || !mysocket.Connected || myip == ((Socket)client_all[i]).RemoteEndPoint.ToString())
                    {
                       
                        client_all.RemoveAt(i);
                        i--;
                        try
                        {

                            StopServerConnet(mysocket);

                        }
                        catch
                        { }
                    }
                }
                if (client_all.Count == 0)
                {
                    setback("掉线", "", myip.Split(':')[0], myip.Split(':')[1], this);
                }
                return;
                //setback("网络出错", myip);
            }
        }
        public void StopServerConnet(Socket proxsocket)//关闭连接
        {
            if (proxsocket != null)
            {
                try {
                    proxsocket.Shutdown(SocketShutdown.Both);
                }
                catch(Exception ex)
                {
                }
                try {
                    proxsocket.Close(100);
                }
                catch (Exception ex)
                {
                }
                try {
                    proxsocket.Dispose();
                }
                catch (Exception ex)
                {
                }
            }

        }
       
        public bool send(string msg,bool is16)
        {
            bool tf = false;
            foreach (Socket sck in client_all)
            {
                if (sck.Connected)
                {
                    if (is16)
                        Class1.write16NO(msg, sck);
                    else
                        Class1.writeACNO(msg, sck);
                    tf = true;
                }
            }
            return tf;
        }
        public Setback setback;
        public void setOnSetback(Setback mlistener)
        {

            setback = mlistener;

        }
        public delegate void Setback(string cmd, string data, string ip, string port, tcpsever ts);
    }
}
