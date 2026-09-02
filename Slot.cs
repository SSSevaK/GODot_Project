using Godot;

public partial class Slot : Area2D
{
	// Хранит ссылку на ингредиент, если слот занят
	public Node2D CurrentIngredient { get; private set; } = null;

	// Проверка: свободна ли ячейка
	public bool IsFree()
	{
		return CurrentIngredient == null;
	}

	// Помещаем ингредиент в центр ячейки
	public void PlaceIngredient(Node2D ingredient)
	{
		CurrentIngredient = ingredient;
		ingredient.GlobalPosition = GlobalPosition;
	}

	// Метод для освобождения ячейки (когда игрок забирает предмет)
	public void ClearSlot()
	{
		CurrentIngredient = null;
	}
}
