using Corely.Connections;
using Corely.Connections.Proxies;
using Corely.Data.Serialization;

namespace LibPostalClient
{
    public static class LibPostalClient
    {
        public static LibPostalServiceResponse ParseAddress(string address)
        {
            HttpProxy proxy = new HttpProxy();
            proxy.Connect("https://<service-url>");
            HttpParameters parameters = new HttpParameters("address", address);
            string xmlResult = proxy.SendRequestForStringResult("/LibPostal/LibPostalService.svc/web/ParseAddress", HttpMethod.Get, null, null, parameters);
            LibPostalServiceResponse response = XmlSerializer.DeSerialize<LibPostalServiceResponse>(xmlResult);
            return response;
        }
    }
}
