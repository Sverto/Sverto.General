using System.Net;

namespace Sverto.General.Net
{
    public static class NetHelper
    {

        public static IPHostEntry NSLookup(string domain)
        {
            IPHostEntry functionReturnValue = default(IPHostEntry);
            functionReturnValue = null;
            try
            {
                functionReturnValue = Dns.GetHostEntry(domain);
            }
            catch
            {
            }
            return functionReturnValue;
        }

        public static bool DomainExists(string domain)
        {
            return (NSLookup(domain) != null);
        }

    }
}
