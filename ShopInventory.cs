using Godot;

public partial class ShopInventory : Node2D
{
	[Export] public PackedScene PotionScene { get; set; } 
	[Export] public Node2D ShopSlotsParent { get; set; }  


	public override void _Ready()
{
	// Подписываемся на события смены сцен
	if (SceneManager.Instance != null)
	{
		SceneManager.Instance.SceneActivated += OnSceneActivated;
	}

	// Первоначальная загрузка при самом первом запуске
	LoadPotionsFromGameManager();
}

private void OnSceneActivated(string scenePath)
{
	// 1. Проверяем, что перешли ИМЕННО В ЛАВКУ
		GD.Print("[ShopInventory] Возврат в лавку: запрашиваем обновление зелий из GameManager...");
		LoadPotionsFromGameManager();
}

	public void LoadPotionsFromGameManager()
	{
		if (GameManager.Instance == null || PotionScene == null || ShopSlotsParent == null)
		{
			GD.PrintErr("[ShopInventory] Ошибка: Не назначены PotionScene или ShopSlotsParent в Инспекторе!");
			return;
		}
		GD.Print("[ShopInventory] ВОЗВРАТ в лавку: обновляем зелья...");
		// 1. Мгновенно убираем старые зелья
		foreach (Node child in GetChildren())
		{
			if (child is Potion potion)
			{
				RemoveChild(potion);
				potion.QueueFree();
			}
		}

		// 2. Сбрасываем слоты
		foreach (Node child in ShopSlotsParent.GetChildren())
		{
			if (child is ShopSlot slot)
			{
				slot.ClearSlot();
			}
		}

		var savedRecipes = GameManager.Instance.PotionsInInventory;
		GD.Print($"[ShopInventory] Загрузка... В GameManager зелий: {savedRecipes.Count}");

		if (savedRecipes == null || savedRecipes.Count == 0) return;

		int recipeIndex = 0;

		// 3. Создаем 2D зелья заново
		foreach (Node child in ShopSlotsParent.GetChildren())
		{
			if (child is ShopSlot slot && slot.IsFree() && recipeIndex < savedRecipes.Count)
			{
				var currentRecipe = savedRecipes[recipeIndex];

				Potion newPotion = PotionScene.Instantiate<Potion>();
				newPotion.Setup(currentRecipe); // Сначала настраиваем данные
				AddChild(newPotion);            // Затем добавляем на сцену
				
				slot.PlaceItem(newPotion);      // Ставим в слот

				GD.Print($"[ShopInventory] Создано зелье '{currentRecipe.PotionName}' в {slot.Name}");
				recipeIndex++;
			}
		}
	}
}
