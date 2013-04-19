
public class myLib
    {
        public static string GetWanIP(string url)
        {
            //http://www.ip138.com/ip2city.asp
            Uri uri = new Uri(url);
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = 0;
            req.CookieContainer = new System.Net.CookieContainer();
            req.GetRequestStream().Write(new byte[0], 0, 0);
            System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)(req.GetResponse());
            StreamReader rs = new StreamReader(res.GetResponseStream(), System.Text.Encoding.GetEncoding("GB18030"));
            string s = rs.ReadToEnd();
            rs.Close();
            req.Abort();
            res.Close(); //s = "116.10.175.142";
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(s, @"(\d+)\.(\d+)\.(\d+)\.(\d+)");
            if (m.Success)
                return m.Value;
            return string.Empty;
        }


        public static string GetSoftList()
        {
            StringBuilder sb = new StringBuilder();
            string temp = null, tempType = null;
            object displayName = null, uninstallString = null, releaseType = null;
            RegistryKey currentKey = null;
            int softNum = 0;
            RegistryKey pregkey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");//获取指定路径下的键
            try
            {

                sb.Append("<table width='95%' border='1' ><tr><td>序号</td><td>程序名称</td></tr>");
                //  <tr>
                //    <td>&nbsp;</td>
                //    <td>&nbsp;</td>
                //  </tr>
                //</table>");
                int n = 1;
                foreach (string item in pregkey.GetSubKeyNames())               //循环所有子键
                {
                    currentKey = pregkey.OpenSubKey(item);
                    displayName = currentKey.GetValue("DisplayName");           //获取显示名称
                    uninstallString = currentKey.GetValue("UninstallString");   //获取卸载字符串路径
                    releaseType = currentKey.GetValue("ReleaseType");           //发行类型,值是Security Update为安全更新,Update为更新
                    bool isSecurityUpdate = false;
                    if (releaseType != null)
                    {
                        tempType = releaseType.ToString();
                        if (tempType == "Security Update" || tempType == "Update")
                            isSecurityUpdate = true;
                    }
                    if (!isSecurityUpdate && displayName != null && uninstallString != null)
                    {
                        softNum++;
                        //temp += displayName.ToString()+"<br>" + Environment.NewLine;
                        sb.Append("<tr><td>" + n + "</td><td>&nbsp;" + displayName.ToString() + "</td></tr>");
                        n++;
                    }

                }
                sb.Append("</table");
            }
            catch (Exception E)
            {
                return null;
                //MessageBox.Show(E.Message.ToString());
            }

            //this.textBox1.Text = "共有软件" + softNum.ToString() + "个" + Environment.NewLine + temp;
            pregkey.Close();
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName()); ;
            IPAddress ipaddress = ipHost.AddressList[0];
            string pcip = ipaddress.ToString();
            string pcname = ipHost.HostName;
            return "共有软件" + softNum.ToString() + "个(电脑信息：" + pcname + ">" + pcip + ")<br>" + sb.ToString();
        }

        public static string GetSoftList2()
        {
            StringBuilder sb = new StringBuilder("[");
            string[] names = null;//返回数组
            string temp = null, tempType = null;
            object displayName = null, uninstallString = null, releaseType = null;
            RegistryKey currentKey = null;
            int softNum = 0;
            RegistryKey pregkey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");//获取指定路径下的键
            int n = 0;
            try
            {
                foreach (string item in pregkey.GetSubKeyNames())               //循环所有子键
                {
                    currentKey = pregkey.OpenSubKey(item);
                    displayName = currentKey.GetValue("DisplayName");           //获取显示名称
                    uninstallString = currentKey.GetValue("UninstallString");   //获取卸载字符串路径
                    releaseType = currentKey.GetValue("ReleaseType");           //发行类型,值是Security Update为安全更新,Update为更新
                    bool isSecurityUpdate = false;
                    if (releaseType != null)
                    {
                        tempType = releaseType.ToString();
                        if (tempType == "Security Update" || tempType == "Update")
                            isSecurityUpdate = true;
                    }
                    if (!isSecurityUpdate && displayName != null && uninstallString != null)
                    {
                        softNum++;
                        //temp += displayName.ToString()+"<br>" + Environment.NewLine;
                        if (n == 0)
                        {
                            sb.Append(displayName.ToString());
                            //names[n] = displayName.ToString();
                        }
                        else
                        {
                            sb.Append("," + displayName.ToString());
                            //names[n] = displayName.ToString();
                        }
                        n++;
                    }

                }

            }
            catch (Exception E)
            {
                return null;
                //MessageBox.Show(E.Message.ToString());
            }

            //this.textBox1.Text = "共有软件" + softNum.ToString() + "个" + Environment.NewLine + temp;
            pregkey.Close();
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName()); ;
            IPAddress ipaddress = ipHost.AddressList[0];
            string pcip = ipaddress.ToString();
            string pcname = ipHost.HostName;
            return sb.ToString() + "]";
        }

        public static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }

                }
            }
            return result;
        }


        public static string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// 获取CPUID，此为举例
        /// </summary>
        /// <returns></returns>
        private static string cpuId()
        {
            string retVal = identifier("Win32_Processor", "UniqueId");  //CPUID   
            retVal += identifier("Win32_Processor", "ProcessorId");
            retVal += identifier("Win32_Processor", "Name");  //处理器名称
            retVal += identifier("Win32_Processor", "Manufacturer");  //处理器制造商
            retVal += identifier("Win32_Processor", "MaxClockSpeed");  //最大时钟频率
            return retVal;
        }
        //BIOS信息
        /// <summary>
        /// 获取BIOS信息，此为举例
        /// </summary>
        /// <returns></returns>
        public static string biosId()
        {
            return identifier("Win32_BIOS", "Manufacturer")          //BIOS制造商名称
                    + identifier("Win32_BIOS", "SMBIOSBIOSVersion")  //
                    + identifier("Win32_BIOS", "IdentificationCode") //
                    + identifier("Win32_BIOS", "SerialNumber")       //BIOS序列号
                    + identifier("Win32_BIOS", "ReleaseDate")        //出厂日期
                    + identifier("Win32_BIOS", "Version");           //版本号
        }
        /// <summary>
        /// 获取硬盘信息，此为举例
        /// </summary>
        /// <returns></returns>
        private static string diskId()
        {
            return identifier("Win32_DiskDrive", "Model")           //模式
                    + identifier("Win32_DiskDrive", "Manufacturer") //制造商
                    + identifier("Win32_DiskDrive", "Signature")    //签名
                    + identifier("Win32_DiskDrive", "TotalHeads");  //扇区头
        }
        /// <summary>
        /// 获取主板信息，此为举例
        /// </summary>
        /// <returns></returns>
        public static string baseId()
        {
            return identifier("Win32_BaseBoard", "Model")
                    + identifier("Win32_BaseBoard", "Manufacturer")
                    + identifier("Win32_BaseBoard", "Name")
                    + identifier("Win32_BaseBoard", "SerialNumber");
        }
        /// <summary>
        /// 获取显卡信息，此为举例
        /// </summary>
        /// <returns></returns>
        private static string videoId()
        {
            return identifier("Win32_VideoController", "DriverVersion")
                    + identifier("Win32_VideoController", "Name");
        }
        /// <summary>
        /// 获取MAC信息，此为举例
        /// </summary>
        /// <returns></returns>
        public static string macId()
        {
            return identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
        }
    }

    /// <summary>
    /// 网络设置类，设置网络的各种参数（DNS、网关、子网掩码、IP）
    /// </summary>
    public class NetworkSetting
    {
        public NetworkSetting()
        {
            // 构造函数逻辑            
        }

        /// <summary>
        /// 设置DNS
        /// </summary>
        /// <param name="dns"></param>
        public static void SetDNS(string[] dns)
        {
            SetIPAddress(null, null, null, dns);
        }
        /// <summary>
        /// 设置网关
        /// </summary>
        /// <param name="getway"></param>
        public static void SetGetWay(string getway)
        {
            SetIPAddress(null, null, new string[] { getway }, null);
        }
        /// <summary>
        /// 设置网关
        /// </summary>
        /// <param name="getway"></param>
        public static void SetGetWay(string[] getway)
        {
            SetIPAddress(null, null, getway, null);
        }
        /// <summary>
        /// 设置IP地址和掩码
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        public static void SetIPAddress(string ip, string submask)
        {
            SetIPAddress(new string[] { ip }, new string[] { submask }, null, null);
        }
        /// <summary>
        /// 设置IP地址，掩码和网关
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        /// <param name="getway"></param>
        public static void SetIPAddress(string ip, string submask, string getway)
        {
            SetIPAddress(new string[] { ip }, new string[] { submask }, new string[] { getway }, null);
        }
        /// <summary>
        /// 设置IP地址，掩码，网关和DNS
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        /// <param name="getway"></param>
        /// <param name="dns"></param>
        public static void SetIPAddress(string[] ip, string[] submask, string[] getway, string[] dns)
        {
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            foreach (ManagementObject mo in moc)
            {
                //如果没有启用IP设置的网络设备则跳过
                if (!(bool)mo["IPEnabled"])
                    continue;

                //设置IP地址和掩码
                if (ip != null && submask != null)
                {
                    inPar = mo.GetMethodParameters("EnableStatic");
                    inPar["IPAddress"] = ip;
                    inPar["SubnetMask"] = submask;
                    outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                }

                //设置网关地址
                if (getway != null)
                {
                    inPar = mo.GetMethodParameters("SetGateways");
                    inPar["DefaultIPGateway"] = getway;
                    outPar = mo.InvokeMethod("SetGateways", inPar, null);
                }

                //设置DNS地址
                if (dns != null)
                {
                    inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                    inPar["DNSServerSearchOrder"] = dns;
                    outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                }
            }
        }

        /// <summary>
        /// 启用DHCP服务器
        /// </summary>
        public static void EnableDHCP()
        {
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                //如果没有启用IP设置的网络设备则跳过
                if (!(bool)mo["IPEnabled"])
                    continue;
                //重置DNS为空
                mo.InvokeMethod("SetDNSServerSearchOrder", null);
                //开启DHCP
                mo.InvokeMethod("EnableDHCP", null);
            }
        }

        /// <summary>
        /// 判断是否符合IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string ip)
        {
            //将完整的IP以“.”为界限分组
            string[] arr = ip.Split('.');


            //判断IP是否为四组数组成
            if (arr.Length != 4)
                return false;


            //正则表达式，1~3位整数
            string pattern = @"\d{1,3}";
            for (int i = 0; i < arr.Length; i++)
            {
                string d = arr[i];


                //判断IP开头是否为0
                if (i == 0 && d == "0")
                    return false;


                //判断IP是否是由1~3位数组成
                if (!Regex.IsMatch(d, pattern))
                    return false;

                if (d != "0")
                {
                    //判断IP的每组数是否全为0
                    d = d.TrimStart('0');
                    if (d == "")
                        return false;

                    //判断IP每组数是否大于255
                    if (int.Parse(d) > 255)
                        return false;
                }
            } return true;
        }
    }
