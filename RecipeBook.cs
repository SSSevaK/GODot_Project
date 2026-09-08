using Godot;
using System;

public partial class RecipeBook : Control
{
	public event Action<PotionRecipe> RecipeSelected;

	[ExportGroup("UI Elements")]
	[Export] public Label PotionNameLabel { get; set; }
	[Export] public TextureRect PotionIconRect { get; set; }
	[Export] public Label IngredientsLabel { get; set; }
	[Export] public Button ChoseButton { get; set; } // Кнопка ChoseButton
	[Export] public Button PrevButton { get; set; }
	[Export] public Button NextButton { get; set; }
	[Export] public Button CloseButton { get; set; }

	[ExportGroup("Data")]
	[Export] public Godot.Collections.Array<PotionRecipe> Recipes { get; set; }

	private int _currentIndex = 0;

	public override void _Ready()
	{
		if (NextButton != null) NextButton.Pressed += TurnPageForward;
		if (PrevButton != null) PrevButton.Pressed += TurnPageBack;
		if (CloseButton != null) CloseButton.Pressed += () => Visible = false;
		
		if (ChoseButton != null) ChoseButton.Pressed += OnChoseButtonPressed;

		UpdatePage();
	}

	private void TurnPageForward()
	{
		if (Recipes != null && _currentIndex < Recipes.Count - 1)
		{
			_currentIndex++;
			UpdatePage();
		}
	}

	private void TurnPageBack()
	{
		if (_currentIndex > 0)
		{
			_currentIndex--;
			UpdatePage();
		}
	}

	private void UpdatePage()
	{
		if (Recipes == null || Recipes.Count == 0) return;

		PotionRecipe currentRecipe = Recipes[_currentIndex];
		
		// 1. Заполняем левую страницу
		PotionNameLabel.Text = currentRecipe.PotionName;
		PotionIconRect.Texture = currentRecipe.Icon; // Раскомментируй, когда добавишь иконку в рецепт

		// 2. Заполняем правую страницу (ингредиенты)
		// IngredientsLabel.Text = currentRecipe.GetIngredientsDescription(); 

		// Управляем видимостью стрелок по краям
		PrevButton.Visible = _currentIndex > 0;
		NextButton.Visible = _currentIndex < Recipes.Count - 1;
	}

	private void OnChoseButtonPressed()
	{
		if (Recipes != null && Recipes.Count > _currentIndex)
		{
			PotionRecipe selectedRecipe = Recipes[_currentIndex];
			
			GD.Print($"[БЛОКНОТ] Передаем рецепт: {selectedRecipe.PotionName}");
			
			// Вызываем событие и передаем выбранный рецепт наружу
			RecipeSelected?.Invoke(selectedRecipe);
		}

		Visible = false; // Закрываем блокнот
	}
}
