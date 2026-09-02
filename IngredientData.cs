using Godot;

[GlobalClass]
public partial class IngredientData : Resource
{
	[Export]
	public string Name = "Неизвестный ингредиент";

	[Export]
	public Texture2D Icon;

	[Export(PropertyHint.Range, "-1.0, 1.0, 0.1")]
	public float FireIceValue = 0.0f;
}
