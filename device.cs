using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class device
    {
        public device()
        {
            netstate = "不在线";
            kepsever_state= "不在线";
            kepsever_times = 100;
        }
        public string kepsever_ip { get; set; }
        public int kepsever_port { get; set; }
        public string kepsever_state { get; set; }
        public int kepsever_times { get; set; }
        public string device_type { get; set; } 
        public string id { get; set; }
        public string addr { get; set; }

        public string id10 { get; set; }
        public string addr10 { get; set; }
        public string IP { get; set; }
        public string myport { get; set; }
        public string nettype { get; set; }
        public string netstate { get; set; }
        public string am { get; set; }
        public string aw = "";
        public string cardid = "";
        public string Lport { get; set; }

        public string car1 = "";
        public string car2 = "";
        public string car3 = "";
        public string car4 = "";
        public string car5 = "";
        public ArrayList bm_all = new ArrayList();
        public ArrayList bhlist = new ArrayList();

    }
}
