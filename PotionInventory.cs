using Godot;

public partial class PotionInventory : Control
{
	[Export] public Button OpenButton { get; set; }
	[Export] public Control InventoryPanel { get; set; }
	[Export] public GridContainer SlotsGrid { get; set; }

	public override void _Ready()
	{
		InventoryPanel.Visible = false;
		OpenButton.Pressed += () => InventoryPanel.Visible = !InventoryPanel.Visible;
	}

	// Метод принимает скомпилированный узел зелья или создаёт его из сцены
	public bool AddPotionNode(Potion potionNode)
	{
		// Ищем первый свободный узел-слот внутри GridContainer
		foreach (Node child in SlotsGrid.GetChildren())
		{
			// Предполагаем, что слоты внутри GridContainer — это условно PotionSlot или Control
			if (child is PotionSlot slot && slot.IsEmpty())
			{
				slot.PutPotion(potionNode);
				return true;
			}
		}

		GD.PrintErr("Инвентарь зелий полон!");
		return false;
	}
}
