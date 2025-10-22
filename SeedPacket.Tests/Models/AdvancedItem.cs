namespace SeedPacket.Tests.Model;

public class AdvancedItem
{
    public Item Item { get; set; } = new Item();

	public override string ToString() => $"Item.ItemName: {Item.ItemName}";
}
