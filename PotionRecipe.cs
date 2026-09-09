using Godot;

[GlobalClass]
public partial class PotionRecipe : Resource
{
	[Export] 
	public string PotionName { get; set; } = "Зелье";
	
	[Export] public int ID { get; set; }       // 1 — Хил, 2 — Мана и т.д.
	[Export] public int Price { get; set; } = 100; // Цена зелья
	[Export] public Texture2D Icon { get; set; }

	// Требуемое значение Лёд-Огонь (-0.8 для Зелья Исцеления)
	[Export(PropertyHint.Range, "-5.0, 5.0, 0.1")] 
	public float TargetFireIceValue { get; set; } = 0.0f;

	// Допустимая погрешность (например, 0.2 значит, что от -0.6 до -1.0 зелье сварится)
	[Export] 
	public float Tolerance { get; set; } = 0.2f;
}
