using Corely.Services;
using System.Collections.Generic;

namespace LibPostalService
{
    public class LibPostalServiceResponse : ServiceResponseBase
    {
        public List<AddressPart> ParsedAddress { get; set; }
    }
}