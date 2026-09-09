using Godot;
using System.Collections.Generic;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	// Список данных сваренных зелий (храним только ресурсы PotionRecipe, а не узлы Control)
	public List<PotionRecipe> PotionsInInventory { get; private set; } = new List<PotionRecipe>();

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			QueueFree();
		}
	}

	// Метод добавления зелья
	public void AddPotion(PotionRecipe recipe)
	{
		PotionsInInventory.Add(recipe);
		GD.Print($"[GameManager] Зелье {recipe.ResourceName} сохранено в глобальный инвентарь.");
	}

	// Метод удаления зелья (когда напарник продаст его в лавке)
	public bool RemovePotion(PotionRecipe recipe)
	{
		return PotionsInInventory.Remove(recipe);
	}
}
