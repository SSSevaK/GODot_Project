using Godot;

public partial class GridContainerScript : GridContainer
{
	[Export] public PackedScene SlotScene { get; set; } // Ссылка на PotionSlot.tscn
	[Export] public int TotalSlots { get; set; } = 9;   // Количество слотов (3x3)

	public override void _Ready()
	{
		// Если слоты не расставлены вручную в редакторе, спавним их из кода
		if (GetChildCount() == 0 && SlotScene != null)
		{
			for (int i = 0; i < TotalSlots; i++)
			{
				Node slotInstance = SlotScene.Instantiate();
				AddChild(slotInstance);
			}
		}
	}
}
