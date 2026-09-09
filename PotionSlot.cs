using Godot;

public partial class PotionSlot : Panel
{
	// Храним ссылку на зелье, которое сидит в этом слоте
	public Potion CurrentPotion { get; private set; } = null;

	public bool IsEmpty() => CurrentPotion == null;

	// Точный центр слота в глобальных координатах экрана
	public Vector2 GetCenterGlobalPosition()
	{
		return GlobalPosition + (Size / 2.0f);
	}

	public void PlacePotion(Potion potion)
	{
		CurrentPotion = potion;

		// Не перепривязываем Node к UI-панели!
		// Вместо этого ставим глобальную позицию зелья ровно в центр слота
		potion.GlobalPosition = GetCenterGlobalPosition();
	}

	public void ClearSlot()
	{
		if (CurrentPotion != null)
		{
			CurrentPotion = null;
		}
	}
}
