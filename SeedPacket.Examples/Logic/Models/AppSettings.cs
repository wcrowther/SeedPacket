namespace SeedPacket.Examples.Logic.Models
{
    public class AppSettings
    {
        public static AppSettings Current;

        public AppSettings()
        {
            Current = this;
        }
        
        public string CustomXmlSourceFile { get; set; }

    }
}
