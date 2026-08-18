using Godot;
using System;


public partial class Burner : Area2D
{
	// Ссылки на дочерний узел с анимацией огня
	private AnimatedSprite2D _flame;

	public override void _Ready()
	{
		// Находим узел Flame
		_flame = GetNode<AnimatedSprite2D>("Flame");

		// Подключаем сигналы входа/выхода предметов в зону горелки
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		// По умолчанию ставим средний огонь
		SetFlameState("medium");
	}

	private void OnAreaEntered(Area2D area)
{
	// Вся логика должна быть ВНУТРИ фигруных скобок if!
	if (area is Ingredient ingredient)
	{
		if (ingredient.ItemData is IngredientData itemData)
		{
			float val = itemData.FireIceValue; // Значение от 0.0 до 1.0

			if (val < 0.4f)
			{
				// Предмет ближе к Льду -> слабый огонь
				SetFlameState("weak");
			}
			else if (val > 0.6f)
			{
				// Предмет ближе к Огню -> сильный огонь
				SetFlameState("strong");
			}
			else
			{
				// Нейтральный предмет -> средний огонь
				SetFlameState("medium");
			}
		}
	}
}

private void OnAreaExited(Area2D area)
{
	if (area is Ingredient)
	{
		// Когда предмет убирают, пламя возвращается к среднему уровню
		SetFlameState("medium");
	}
}

	private void SetFlameState(string animName)
	{
		if (_flame != null)
		{
			_flame.Play(animName);
		}
	}
}
