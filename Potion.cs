using Godot;

public partial class Potion : Control
{
	[Export] public TextureRect IconTexture { get; set; }
	[Export] public Label NameLabel { get; set; }

	public PotionRecipe Data { get; private set; }

	// Метод для инициализации зелья при спавне
	public void Setup(PotionRecipe recipe)
	{
		Data = recipe;
		
		if (IconTexture != null && recipe.Icon != null)
			IconTexture.Texture = recipe.Icon;

		if (NameLabel != null)
			NameLabel.Text = recipe.PotionName;

		TooltipText = $"{recipe.PotionName}\nСтат: {recipe.TargetFireIceValue}";
	}
}
