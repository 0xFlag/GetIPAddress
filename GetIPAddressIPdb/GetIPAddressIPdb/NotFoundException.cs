using System;

namespace GetIPAddressIPdb
{
    public class NotFoundException : Exception
    {

        public NotFoundException(string name) : base(name)
        {
        }
    }
}
