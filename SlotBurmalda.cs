using Godot;
using System;

[Tool]
public partial class SlotBurmalda : Control
{
	[Export] public string ItemId = "seed_1"; 
	[Export] public Texture2D ItemTexture; 

	private Label _label;
	private TextureRect _icon; 
	private seed_scr_burmalda _globalData;

	public override void _Ready()
	{
		_label = FindChild("Label", true, false) as Label;
		
		if (_icon == null) { _icon = FindChild("Icon", true, false) as TextureRect; }
		if (_icon == null) { _icon = FindChild("*", true, false) as TextureRect; }
		
		if (_icon != null && ItemTexture != null)
		{
			_icon.Texture = ItemTexture;
		}

		if (Engine.IsEditorHint()) return;

		_globalData = GetNode<seed_scr_burmalda>("/root/SeedScrBurmalda");
		_globalData.IngredientChanged += OnIngredientChanged;
		
		if (_label != null && _globalData.Inventory.ContainsKey(ItemId))
		{
			_label.Text = _globalData.Inventory[ItemId].ToString();
		}
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (!Engine.IsEditorHint() && @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			if (_globalData != null)
			{
				_globalData.SelectedSeedType = ItemId;
				GD.Print($"[Инвентарь] Выбрано семя для посадки: {_globalData.SelectedSeedType}");
			}
		}
	}

	private void OnIngredientChanged(string type, int amount)
	{
		if (type == ItemId && _label != null)
		{
			_label.Text = amount.ToString();
		}
	}

	public override void _ExitTree()
	{
		if (_globalData != null)
		{
			_globalData.IngredientChanged -= OnIngredientChanged;
		}
	}
}
