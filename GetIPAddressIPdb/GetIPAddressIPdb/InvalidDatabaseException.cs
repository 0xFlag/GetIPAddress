using System.IO;

namespace GetIPAddressIPdb
{
    public class InvalidDatabaseException : IOException
    {
        public InvalidDatabaseException(string message) : base(message)
        {
        }
    }
}