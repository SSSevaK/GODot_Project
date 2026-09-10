using Godot;
using System.Collections.Generic;

public partial class Cauldron : Area2D
{
	private List<IngredientData> _ingredientsInCauldron = new List<IngredientData>();
	private float _currentFireIceValue = 0.0f;

	[Export] public Label WarningLabel { get; set; }
	[Export] public Button BrewButton { get; set; }

	[ExportGroup("Ресурсы и Сцены")]
	[Export] public PotionRecipe TargetRecipe { get; set; }
	[Export] public PackedScene PotionScene { get; set; }
	[Export] public PotionInventory Inventory { get; set; }

	public override void _Ready()
	{
		if (WarningLabel != null)
		{
			WarningLabel.Modulate = new Color(1, 1, 1, 0);
			WarningLabel.HorizontalAlignment = HorizontalAlignment.Center;
			WarningLabel.VerticalAlignment = VerticalAlignment.Center;
		}

		if (BrewButton != null)
			BrewButton.Pressed += OnBrewButtonPressed;
	}

	public void AddIngredient(Ingredient ingredient)
	{
		if (ingredient.ItemData is IngredientData data)
		{
			_ingredientsInCauldron.Add(data);
			RecalculateStats();
			ingredient.QueueFree();
		}
	}

	private void RecalculateStats()
	{
		_currentFireIceValue = 0.0f;
		foreach (var data in _ingredientsInCauldron)
		{
			_currentFireIceValue += data.FireIceValue;
		}
	}

	private void OnBrewButtonPressed()
	{
		if (TargetRecipe == null)
		{
			ShowWarning("Рецепт не выбран!");
			return;
		}

		if (_ingredientsInCauldron.Count == 0)
		{
			ShowWarning("Котёл пуст!");
			return;
		}

		BrewPotion();
	}

	private void BrewPotion()
	{
		float difference = Mathf.Abs(_currentFireIceValue - TargetRecipe.TargetFireIceValue);

		if (difference <= TargetRecipe.Tolerance)
		{
			ShowWarning($"Сварено: {TargetRecipe.PotionName}!");
			GameManager.Instance.AddPotion(TargetRecipe);
			if (PotionScene != null)
			{
				Potion newPotion = PotionScene.Instantiate<Potion>();
				newPotion.Setup(TargetRecipe);

				if (Inventory != null)
				{
					Inventory.AddPotionNode(newPotion);
				}
			}
		}
		else
		{
			GD.Print($"Варка не удалась! {_currentFireIceValue} vs {TargetRecipe.TargetFireIceValue}");
		}

		ClearCauldron();
	}

	private void ShowWarning(string text)
	{
		if (WarningLabel == null) return;
		WarningLabel.Text = text;
		Tween tween = CreateTween();
		tween.TweenProperty(WarningLabel, "modulate:a", 1.0f, 0.15f);
		tween.TweenInterval(1.0f);
		tween.TweenProperty(WarningLabel, "modulate:a", 0.0f, 0.4f);
	}

	private void ClearCauldron()
	{
		_ingredientsInCauldron.Clear();
		_currentFireIceValue = 0.0f;
	}
}
