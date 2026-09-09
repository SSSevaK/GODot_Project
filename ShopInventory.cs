using Godot;

public partial class ShopInventory : Node2D
{
	[Export] public PackedScene PotionScene { get; set; } 
	[Export] public Node2D ShopSlotsParent { get; set; }  

	public override void _Ready()
	{
		// Откладываем загрузку на 1 кадр, чтобы слоты успели встать на места
		Callable.From(LoadPotionsFromGameManager).CallDeferred();
	}

	public void LoadPotionsFromGameManager()
	{
		if (GameManager.Instance == null || PotionScene == null || ShopSlotsParent == null) return;

		// Очищаем прошлые 2D зелья на столе, если они были
		foreach (Node child in GetChildren())
		{
			if (child is Potion)
			{
				child.QueueFree();
			}
		}

		var savedRecipes = GameManager.Instance.PotionsInInventory;
		int recipeIndex = 0;

		foreach (Node child in ShopSlotsParent.GetChildren())
		{
			if (child is ShopSlot slot && slot.IsFree() && recipeIndex < savedRecipes.Count)
			{
				Potion newPotion = PotionScene.Instantiate<Potion>();
				newPotion.Visible = true;
				newPotion.Setup(savedRecipes[recipeIndex]);

				AddChild(newPotion);
				slot.PlaceItem(newPotion);

				recipeIndex++;
			}
		}
	}
}
