using Corely.Services;
using System;
using System.Collections.Generic;

namespace LibPostalClient
{
    [Serializable]
    public class LibPostalServiceResponse : ServiceResponseBase
    {
        public List<AddressPart> ParsedAddress { get; set; }
    }
}
