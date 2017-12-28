using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;

namespace CopyPlugin
{
    class CopyFiles
    {

        private DriveInfo _diRemDevDisk = null;
        public CopyFiles()
        {

        }

        public void SetDriverInfo(DriveInfo diPara)
        {
            _diRemDevDisk = diPara;
        }

        /// <summary>
        /// 递归拷贝所有子目录。
        /// </summary>
        /// <param name="sPath">源目录</param>
        /// <param name="dPath">目的目录</param>
        private void copyDirectory(string sPath, string dPath)
        {
            try
            {
                string[] directories = System.IO.Directory.GetDirectories(sPath);
                if (!System.IO.Directory.Exists(dPath))
                    System.IO.Directory.CreateDirectory(dPath);
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sPath);
                System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
                CopyFile(dir, dPath);
                if (dirs.Length > 0)
                {
                    foreach (System.IO.DirectoryInfo temDirectoryInfo in dirs)
                    {
                        try
                        {
                            string sourceDirectoryFullName = temDirectoryInfo.FullName;
                            string destDirectoryFullName = sourceDirectoryFullName.Replace(sPath, dPath);
                            if (!System.IO.Directory.Exists(destDirectoryFullName))
                            {
                                System.IO.Directory.CreateDirectory(destDirectoryFullName);
                            }
                            CopyFile(temDirectoryInfo, destDirectoryFullName);
                            copyDirectory(sourceDirectoryFullName, destDirectoryFullName);                             
                        }
                        catch(Exception)
                        {
                            continue;
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }

        }

        /// <summary>
        /// 拷贝目录下的所有文件到目的目录。
        /// </summary>
        /// <param name="path">源路径</param>
        /// <param name="desPath">目的路径</param>
        private void CopyFile(System.IO.DirectoryInfo path, string desPath)
        {
            string sourcePath = path.FullName;
            System.IO.FileInfo[] files = path.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                try
                {
                    string sourceFileFullName = file.FullName;
                    string destFileFullName = sourceFileFullName.Replace(sourcePath, desPath);
                    file.CopyTo(destFileFullName, true);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public void StartCopy()
        {
            var targetDisk = ConfigurationManager.AppSettings["DiskName"];
            if (string.IsNullOrEmpty(targetDisk))
            {
                targetDisk = "C:\\";
            }
            string strFolder = string.Format("{0}{1}-{2}_{3}-{4}", targetDisk, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
            int iCount = 0;
            while (iCount < 100)
            {
                if (!Directory.Exists(strFolder))
                {
                    break;                    
                }
                else
                {
                    strFolder += "_" + iCount.ToString();                    
                }
            }
            strFolder += "\\";
            copyDirectory(_diRemDevDisk.Name.ToString(), strFolder);
        }

        public void StopCopy()
        {

        }
    }
}
