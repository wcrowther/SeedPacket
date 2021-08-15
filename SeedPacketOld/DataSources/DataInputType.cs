
namespace SeedPacket
{
    /// <summary>The data source input type enum. Can be either a string or a file in eitherXML or JSON format.<br/>
    /// Defaults to Auto which will try to use whatever is supplied. Default uses embedded XML default.</summary>
    public enum DataInputType
    {
        Auto,
        XmlString,
        XmlFile,
        JsonString,
        JsonFile,
        CsvString,
        CsvFile,
        Default
    };
}
