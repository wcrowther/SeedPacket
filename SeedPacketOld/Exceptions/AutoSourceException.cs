using System;

namespace SeedPacket.Exceptions
{
    public class AutoSourceException : Exception
    {
        public AutoSourceException(string fileExtension) 
            : base($"AutoSource not able to identify file sourceType for extension {fileExtension}." +
                   $"Please set the DataSource file DateInputType explicitly.")
        {
        }
    }
}
