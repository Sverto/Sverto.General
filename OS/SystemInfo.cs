using System;
using System.Management;
using System.Net;
using System.Text;

namespace Sverto.General.OS
{
    public class SystemInfo
    {

        static internal string GetSystemInfo()
        {
            StringBuilder sbInfo = new StringBuilder();
            sbInfo.AppendLine("Machine: " + Environment.MachineName);
            sbInfo.AppendLine("Main Board: " + GetMainBoardModel());
            sbInfo.AppendLine("CPU: " + GetCpuName());
            sbInfo.AppendLine("CPU ID: " + GetCpuId().ToString());
            sbInfo.AppendLine("OS: " + Environment.OSVersion.ToString());
            sbInfo.AppendLine("Version: " + Environment.Version.ToString());
            sbInfo.AppendLine("Domain: " + Environment.UserDomainName);
            sbInfo.AppendLine("Username: " + Environment.UserName);
            sbInfo.AppendLine("Working Directory: " + Environment.CurrentDirectory);
            //sbInfo.AppendLine("IP Adresses: " & GetIpAdressess().ToString())
            sbInfo.AppendLine("IP Adresses: " + GetIpAdressess());
            return sbInfo.ToString();
        }

        static internal string GetCpuName()
        {
            SelectQuery query = new SelectQuery("Win32_Processor");
            ManagementObjectSearcher search = new ManagementObjectSearcher(query);
            string cpuName = "Unknown";
            try
            {
                foreach (ManagementObject info in search.Get())
                {
                    cpuName = info.GetPropertyValue("Name").ToString();
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            catch
            {
            }
            return cpuName;
        }

        static internal string GetCpuId()
        {
            SelectQuery query = new SelectQuery("Win32_Processor");
            ManagementObjectSearcher search = new ManagementObjectSearcher(query);
            string cpuId = "Unknown";
            try
            {
                foreach (ManagementObject info in search.Get())
                {
                    cpuId = info.GetPropertyValue("ProcessorID").ToString();
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            catch
            {
            }
            return cpuId;
        }

        static internal string GetMainBoardModel()
        {
            SelectQuery query = new SelectQuery("Win32_BaseBoard");
            ManagementObjectSearcher search = new ManagementObjectSearcher(query);
            string name = "Unknown";
            try
            {
                foreach (ManagementObject info in search.Get())
                {
                    name = info.GetPropertyValue("Product").ToString();
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            catch
            {
            }
            return name;
        }

        static internal string GetIpAdressess()
        {
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            StringBuilder sbIps = new StringBuilder();
            foreach (IPAddress ip in ips)
            {
                sbIps.Append(ip.ToString() + ", ");
            }
            return sbIps.ToString();
        }

    }
}