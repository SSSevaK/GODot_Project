using Godot;

public partial class ShopSlot : Area2D
{
	public Potion CurrentPotion { get; private set; }

	public bool IsFree()
	{
		return !GodotObject.IsInstanceValid(CurrentPotion);
	}

	public void PlaceItem(Potion potion)
	{
		CurrentPotion = potion;
		potion.GlobalPosition = GlobalPosition;
		potion.SetSlot(this); // Оповещаем зелье о его текущем слоте
	}

	public void ClearSlot()
	{
		CurrentPotion = null;
	}
}
