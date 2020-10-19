using System;
using System.Linq;

namespace Common
{
    public class Network
    {
        public static string getHostName()
        {
            var HostAddresses = System.Net.Dns.GetHostName().ToString();
            return HostAddresses;
        }
        public static string getHostIp()
        {
            var HostAddresses = getHostName();
            var IdAddresses = System.Net.Dns.GetHostEntry(HostAddresses).AddressList;
            var IdAddressesB = IdAddresses.Select(o => o.ToString() + "; ").ToList();
            var IP = String.Concat(IdAddressesB);
            return IP;
        }
    }
}
