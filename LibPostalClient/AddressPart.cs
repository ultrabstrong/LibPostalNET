using System;

namespace LibPostalClient
{
    [Serializable]
    public class AddressPart
    {
        public AddressPart() { }

        public AddressPart(string _name, string _value)
        {
            Name = _name;
            Value = _value;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
