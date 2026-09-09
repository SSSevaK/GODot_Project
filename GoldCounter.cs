using Godot;

public partial class GoldCounter : Godot.TextEdit
{
	public int Gold { get; private set; } = 0;

	public override void _Ready()
	{
		AddThemeColorOverride("font_color", Colors.Black);
		AddThemeColorOverride("font_readonly_color", Colors.Black);
		UpdateUI();
	}

	public void AddGoldForPotion(PotionRecipe recipe)
	{
		if (recipe == null) return;

		int reward = recipe.Price > 0 ? recipe.Price : (recipe.ID == 1 ? 100 : 75);
		Gold += reward;
		UpdateUI();

		GD.Print($"[Золото] +{reward}. Текущий баланс: {Gold}");
	}

	public void AddGold(int amount)
	{
		Gold += amount;
		UpdateUI();
	}

	// Добавляем этот метод для поддержки старых кнопок и скриптов
	public void ForSell(int id)
	{
		if (id == 1)
		{
			AddGold(100);
		}
		else if (id == 2)
		{
			AddGold(75);
		}
		else
		{
			GD.Print("[Торговля] Вызов ForSell с id = " + id);
		}
	}

	private void UpdateUI()
	{
		Text = Gold.ToString();
	}
}
