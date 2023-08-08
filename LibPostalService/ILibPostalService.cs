using System.ServiceModel;
using System.ServiceModel.Web;

namespace LibPostalService
{
    [ServiceContract]
    public interface ILibPostalService
    {

        /// <summary>
        /// Parse address for address parts
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        LibPostalServiceResponse ParseAddress(string address);

    }
}
