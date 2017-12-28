using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace InstServUtil
{
    public partial class Form1 : Form
    {
        //List<string> strOutput = null;
        /// <summary>
        /// Used by installer
        /// </summary>
        internal const string _serviceName = "Centry Hospital Plugin Service";
        internal const string _serviceInteractive = "UI0Detect";
        string strInsPath = string.Empty;

        public delegate void AppendTextCallback(string text);

        

        public Form1()
        {
            InitializeComponent();
            strInsPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            strInsPath += "InstallUtil.exe";
            //   strOutput = new List<string>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> lstCmd = new List<string>();
            lstCmd.Add(strInsPath + " CopyPlugin.exe");
            lstCmd.Add("CloseInteractive.bat");
            ExeCommand(lstCmd);           
        }

        private void StartService()
        {
            if (ServiceOpe.ISWindowsServiceInstalled(_serviceName))
            {
                ServiceOpe.StartService(_serviceName);
            }
        }

        private void CloseDetectService()
        {
            if (ServiceOpe.ISWindowsServiceInstalled(_serviceInteractive))
            {
                ServiceOpe.StopService(_serviceInteractive);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        public void ExeCommand(List<string> strCmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;//true表示不显示黑框，false表示显示dos界面
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            
            try
            {
                p.Start();
                // 异步获取命令行内容  
                p.BeginOutputReadLine();
                foreach (string cmd in strCmd)
                {
                    p.StandardInput.WriteLine(cmd);
                }
                
                p.Close();
            }
            catch (Exception ex)
            {

            }

        }      

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {          
            if (String.IsNullOrEmpty(e.Data) == false)
            {
                this.AppendText(e.Data);
            }
        }

        public void AppendText(string text)
        {
            if (this.listBox1.InvokeRequired)
            {
                AppendTextCallback d = new AppendTextCallback(AppendText);
                this.listBox1.Invoke(d, text);
            }
            else
            {
                listBox1.Items.Add(text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> lstCmd = new List<string>();
            lstCmd.Add(strInsPath + " CopyPlugin.exe /u");
            ExeCommand(lstCmd);
            
        }

        private void CloseService()
        {
            Thread _closeService = new Thread(new ThreadStart(Work.DoWork));
            _closeService.Start();
            _closeService.Join();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AppendText("Starting service...");
            
            StartService();
            AppendText("Service started!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AppendText("Stoping service...");
            CloseService();
            AppendText("Service stopped!");
        }
                
    }

    class Work
    {
        internal const string _serviceName = "Centry Hospital Plugin Service";
        Work() { }

        public static void DoWork()
        {
            if (ServiceOpe.ISWindowsServiceInstalled(_serviceName))
            {
                ServiceOpe.StopService(_serviceName);
            }
        }
    }
}
