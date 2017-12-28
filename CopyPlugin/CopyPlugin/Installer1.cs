using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32; //对注册表操作一定要引用这个命名空间

namespace CopyPlugin
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        public Installer1()
        {
            InitializeComponent();
            AfterInstall += new InstallEventHandler(ProjectInstaller_AfterInstall);
        }

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            //设置允许服务与桌面交互
           SetServiceTable(Service1._serviceName);
        }
        /// <summary>
        /// 设置允许服务与桌面交互 ,修改了注册表，要重启系统才能生效
        /// </summary>
        /// <param name="ServiceName">服务程序名称</param>
        private void SetServiceTable(string ServiceName)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine;
                string key = @"SYSTEM/CurrentControlSet/Services/" + ServiceName;
                RegistryKey sub = rk.OpenSubKey(key, true);
                int value = (int)sub.GetValue("Type");
                sub.SetValue("Type", value | 256);
            }
            catch (Exception ex)
            {
                EvtLog el = new EvtLog();
                el.LogEvent(ex.Message);
            }
        }

    }
}
