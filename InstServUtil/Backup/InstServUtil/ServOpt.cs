using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using System.Threading;

namespace InstServUtil
{
    class ServiceOpe
    {             
        public ServiceOpe()
        {

        }
        /// <summary>  

        /// 判断是否安装了某个服务  

        /// </summary>  

        /// <param name="serviceName"></param>  

        /// <returns></returns>  

        public static bool ISWindowsServiceInstalled(string serviceName)
        {

            try
            {

                ServiceController[] services = ServiceController.GetServices();

                foreach (ServiceController service in services)
                {

                    if (service.ServiceName == serviceName)
                    {

                        return true;

                    }

                }





                return false;

            }

            catch

            { return false; }

        }


        /// <summary>  

        /// 启动某个服务  

        /// </summary>  

        /// <param name="serviceName"></param>  

        public static void StartService(string serviceName)
        {

            try
            {

                ServiceController[] services = ServiceController.GetServices();





                foreach (ServiceController service in services)
                {

                    if (service.ServiceName == serviceName)
                    {

                        service.Start();





                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                        service.Refresh();
                    }

                }

            }

            catch { }

        }


        /// <summary>  

        /// 停止某个服务  

        /// </summary>  

        /// <param name="serviceName"></param>  

        public static void StopService(string serviceName)
        {

            try
            {

                ServiceController[] services = ServiceController.GetServices();





                foreach (ServiceController service in services)
                {

                    if (service.ServiceName == serviceName)
                    {

                        service.Stop();





                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                        service.Refresh();
                    }

                }

            }

            catch (Exception ex)
            {

            }

        }

        /// <summary>  

        /// 判断某个服务是否启动  

        /// </summary>  

        /// <param name="serviceName"></param>  

        public static bool ISStart(string serviceName)
        {

            bool result = true;





            try
            {

                ServiceController[] services = ServiceController.GetServices();





                foreach (ServiceController service in services)
                {

                    if (service.ServiceName == serviceName)
                    {

                        if ((service.Status == ServiceControllerStatus.Stopped)

                            || (service.Status == ServiceControllerStatus.StopPending))
                        {

                            result = false;

                        }

                    }

                }

            }

            catch { }





            return result;

        }
    }




}
