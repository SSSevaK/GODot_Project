using Godot;
using System;
using System.Collections.Generic;

public partial class seed_scr_burmalda : Node
{
	[Signal]
	public delegate void IngredientChangedEventHandler(string type, int amount);

	// Хранит текущее выбранное семечко ("seed_1", "seed_2", "seed_3" и т.д.)
	public string SelectedSeedType = ""; 

	// Расширенный словарь для всех 8 слотов инвентаря
	public Dictionary<string, int> Inventory = new Dictionary<string, int>()
	{
		{ "seed_1", 10 },
		{ "seed_2", 5 },
		{ "seed_3", 15 }, // Количество для 3-го слота
		{ "seed_4", 8 },  // Количество для 4-го слота
		{ "seed_5", 12 }, // Количество для 5-го слота
		{ "seed_6", 4 },  // Количество для 6-го слота
		{ "seed_7", 20 }, // Количество для 7-го слота
		{ "seed_8", 7 }   // Количество для 8-го слота
	};

	public void ChangeIngredient(string type, int amount)
	{
		if (Inventory.ContainsKey(type))
		{
			Inventory[type] += amount;
			EmitSignal(SignalName.IngredientChanged, type, Inventory[type]);
		}
	}
}
