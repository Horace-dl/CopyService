using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CopyPlugin
{
    public partial class Service1 : ServiceBase
    {
        /// <summary>
        /// Used by installer
        /// </summary>
        internal const string _serviceName = "Centry Hospital Plugin Service";

        /// <summary>
        /// Used by installer
        /// </summary>
        internal const string _serviceDescription = "Centry Hospital Plugin Device Helper  Component to Help Doctors Get More Knowledge.";

        internal const string _svStopedOne = "Interactive Service Detection";

        private Thread _detectPlugInThread;
        private PluginFileMgr _pfmObject = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                bool bStarted = ServiceOpe.ISStart(_svStopedOne);
                if (bStarted)
                {
                    //Stop the  service  "Interactive Service Detection"

                }

                _detectPlugInThread = new Thread(new ThreadStart(DetectThreadProc));

                _detectPlugInThread.Start();
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format(CultureInfo.CurrentCulture, "Cannot create worker thread. {0}", ex.Message);
                Trace.WriteLine(errorMsg);
            }
        }

        private void DetectThreadProc()
        {
            GetDesktopWindow();
            IntPtr hwinstaSave = GetProcessWindowStation();
            IntPtr dwThreadId = GetCurrentThreadId();
            IntPtr hdeskSave = GetThreadDesktop(dwThreadId);
            IntPtr hwinstaUser = OpenWindowStation("WinSta0", false, 33554432);
            if (hwinstaUser == IntPtr.Zero)
            {
                RpcRevertToSelf();
                return;
            }
            
            SetProcessWindowStation(hwinstaUser);
            IntPtr hdeskUser = OpenDesktop("Default", 0, false, 33554432);
            RpcRevertToSelf();
            if (hdeskUser == IntPtr.Zero)
            {
                SetProcessWindowStation(hwinstaSave);
                CloseWindowStation(hwinstaUser);
                return;
            }
            SetThreadDesktop(hdeskUser);

            IntPtr dwGuiThreadId = dwThreadId;

            _pfmObject = new PluginFileMgr(); //此FORM1可以带notifyIcon，可以显示在托盘里，用户可点击托盘图标进行设置
            System.Windows.Forms.Application.Run(_pfmObject);


            dwGuiThreadId = IntPtr.Zero;
            SetThreadDesktop(hdeskSave);
            SetProcessWindowStation(hwinstaSave);
            CloseDesktop(hdeskUser);
            CloseWindowStation(hwinstaUser);
        }


        protected override void OnStop()
        {
            if (_pfmObject != null)
            {
                _pfmObject.Dispose();
                _pfmObject = null;
            }

            if (_detectPlugInThread != null)
            {
                if (!_detectPlugInThread.Join(10000))
                {
                    _detectPlugInThread.Abort();
                }
            }

        }

        [DllImport("user32.dll")]
        static extern int GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetProcessWindowStation();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll")]
        static extern IntPtr GetThreadDesktop(IntPtr dwThread);

        [DllImport("user32.dll")]
        static extern IntPtr OpenWindowStation(string a, bool b, int c);

        [DllImport("user32.dll")]
        static extern IntPtr OpenDesktop(string lpszDesktop, uint dwFlags,
        bool fInherit, uint dwDesiredAccess);

        [DllImport("user32.dll")]
        static extern IntPtr CloseDesktop(IntPtr p);

        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern IntPtr RpcImpersonateClient(int i);


        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern IntPtr RpcRevertToSelf();

        [DllImport("user32.dll")]
        static extern IntPtr SetThreadDesktop(IntPtr a);

        [DllImport("user32.dll")]
        static extern IntPtr SetProcessWindowStation(IntPtr a);
        [DllImport("user32.dll")]
        static extern IntPtr CloseWindowStation(IntPtr a);



    }
}
