using Godot;

public partial class ShopSlot : Area2D
{
	public Potion CurrentPotion { get; private set; }

	public bool IsFree() => CurrentPotion == null;

	public void PlaceItem(Potion potion)
	{
		CurrentPotion = potion;
		potion.GlobalPosition = GlobalPosition;
	}

	public void ClearSlot()
	{
		CurrentPotion = null;
	}
}
