using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

//HTJ：
//0.84:
//  0.修改了天津五维地磁车检器的数据接收转换问题
//  1.修改了服务端日志、设备日志弹窗异常的问题
//0.85：20230906
//  0.修改Program.cs，添加静态变量appName，每一次新版本时要修改项目属性的输出与appName变量
//  1.全面修改天津五维微波车检器为天津五维地磁车检器，添加设备界面中监听端口控件显示，并保存校验不允许端口重复
//  2.按粟工要求修改了Form1.cs的唯的美情报板部分line265-275开始，支持中文；修改device_cmd.cs中第467行，扩大字体到04
//0.86：20230906
//  1.根据粟工新要求修改了device_cmd.cs两处
//  2.根据粟工新要求修改了Form1.cs两处
//0.87：20230911
//  1.根据粟工新要求修改了device_cmd.cs 2处
//  1.根据粟工新要求修改了Form1.cs1处
//0.88：20230913
//  1.应急电话发送问题修改了device_cmd.cs1处
//  2.应急电话发送问题修改了Form1.cs1处

namespace WindowsFormsApp1
{
    internal static class Program
    {
        public static string appName = "调度中心测试版软件V0.88";    //HTJ增加
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!OneInstance.IsFirst(appName))
            //if (!OneInstance.IsFirst("调度中心测试版软件V0.85"))
            {
                return;
            }
            //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Application.Restart();
        }
    }
   
    public static class OneInstance
    {
        ///<summary>
        ///判断程序是否正在运行 
        ///</summary>
        ///<param name="appId">程序名称</param>
        ///<returns>如果程序是第一次运行返回True,否则返回False</returns>
        public static bool IsFirst(string appId)
        {
            bool ret = false;
            if (OpenMutex(0x1F0001, 0, appId) == IntPtr.Zero)
            {
                CreateMutex(IntPtr.Zero, 0, appId);
                ret = true;
            }
            return ret;
        }
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr OpenMutex(
            uint dwDesiredAccess, // access 
            int bInheritHandle,    // inheritance option 
            string lpName          // object name 
            );
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateMutex(
            IntPtr lpMutexAttributes, // SD 
            int bInitialOwner,                       // initial owner 
            string lpName                            // object name 
            );
    }
}
