using SeedPacket.Generators;

namespace SeedPacket
{
    public class CustomGenerator : MultiGenerator 
    {
        public CustomGenerator( string xmlfilepath = null, string xmlstring = null)
                                : base(xmlfilepath, xmlstring)
        {
           // Do Stuff Here  
        }
    }
} 

 
