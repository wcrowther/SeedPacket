using SeedPacket.Interfaces;

namespace SeedPacket.Generators
{
    public class BasicGenerator : Generator, IGenerator 
    {
        // BasicGenerator never populates the datasource so it is always null

        public BasicGenerator () : base()
        {
            Rules.AddBasicRules();
        }
    }
} 
