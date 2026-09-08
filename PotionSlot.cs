using Godot;

public partial class PotionSlot : Panel
{
	public override void _Ready()
	{
		CustomMinimumSize = new Vector2(32, 32);
	}

	public bool IsEmpty() => GetChildCount() == 0;

	public void PutPotion(Potion potion)
	{
		if (potion.GetParent() != null)
		{
			potion.GetParent().RemoveChild(potion);
		}

		AddChild(potion);
		potion.SetAnchorsPreset(LayoutPreset.FullRect);

		// Принудительно делаем зелье видимым и выводим отладку
		potion.Visible = true;
		GD.Print($"[DEBUG] Зелье {potion.Name} добавлено. Детей в слоте: {GetChildCount()}");
	}
}
