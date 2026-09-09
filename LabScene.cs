using Godot;

public partial class LabScene : Node2D
{
	[ExportGroup("UI Elements")]
	[Export] public Button OpenBookButton { get; set; }
	[Export] public RecipeBook BookUI { get; set; }

	[ExportGroup("Game Logic")]
	[Export] public Cauldron CauldronNode { get; set; }

	public override void _Ready()
	{
		if (BookUI != null)
		{
			BookUI.Visible = false;

			// Подписываемся на выбор рецепта
			BookUI.RecipeSelected += OnRecipeSelectedFromBook;
		}

		if (OpenBookButton != null && BookUI != null)
		{
			OpenBookButton.Pressed += () => BookUI.Visible = !BookUI.Visible;
		}
	}

	private void OnRecipeSelectedFromBook(PotionRecipe recipe)
	{
		if (CauldronNode != null)
		{
			// Присваиваем выбранный рецепт котлу!
			// (Убедитесь, что переменная TargetRecipe в Cauldron.cs публичная: public PotionRecipe TargetRecipe { get; set; })
			CauldronNode.TargetRecipe = recipe; 
			GD.Print($"[ЛАБОРАТОРИЯ] Котел переключен на рецепт: {recipe.PotionName}");
		}
		else
		{
			GD.PrintErr("[ЛАБОРАТОРИЯ] CauldronNode не назначен в Инспекторе!");
		}
	}

	public override void _ExitTree()
	{
		// Отписываемся при уничтожении сцены для предотвращения утечек памяти
		if (BookUI != null)
		{
			
		}
	}
}
