using Godot;

public partial class PotionInventory : Control
{
	[Export] public Button OpenButton { get; set; }
	[Export] public Control InventoryPanel { get; set; }
	[Export] public GridContainer SlotsGrid { get; set; }
	[Export] public Node2D PotionsHolder { get; set; }
	[Export] public PackedScene PotionScene { get; set; }
	[Export] public PackedScene SlotScene { get; set; } // Ссылка на PotionSlot.tscn
	[Export] public int TotalSlots { get; set; } = 9;

	public override void _Ready()
	{
		SetInventoryVisible(false);

		if (OpenButton != null)
		{
			OpenButton.Pressed += () => SetInventoryVisible(!InventoryPanel.Visible);
		}

		// Сначала инициализируем слоты, затем восстанавливаем зелья
		InitSlots();
		Callable.From(RestorePotionsFromGameManager).CallDeferred();
	}

	private void InitSlots()
	{
		if (SlotsGrid == null) return;

		// Если слотов еще нет в сетке — спавним их
		if (SlotsGrid.GetChildCount() == 0 && SlotScene != null)
		{
			for (int i = 0; i < TotalSlots; i++)
			{
				Node slotInstance = SlotScene.Instantiate();
				SlotsGrid.AddChild(slotInstance);
			}
		}
	}

	private void RestorePotionsFromGameManager()
	{
		if (GameManager.Instance == null || PotionScene == null || SlotsGrid == null) return;

		// 1. Очищаем старые 2D-узлы зелий на сцене
		if (PotionsHolder != null)
		{
			foreach (Node child in PotionsHolder.GetChildren())
			{
				child.QueueFree();
			}
		}

		// 2. Сбрасываем ссылки у всех слотов
		foreach (Node child in SlotsGrid.GetChildren())
		{
			if (child is PotionSlot slot)
			{
				slot.ClearSlot();
			}
		}

		// 3. Раскладываем ТЕКУЩИЕ зелья из GameManager
		var savedRecipes = GameManager.Instance.PotionsInInventory;

		for (int i = 0; i < savedRecipes.Count; i++)
		{
			Potion potionInstance = PotionScene.Instantiate<Potion>();
			potionInstance.Setup(savedRecipes[i]);

			PutPotionIntoFreeSlot(potionInstance);
		}
	}

	private void SetInventoryVisible(bool visible)
	{
		if (InventoryPanel != null) InventoryPanel.Visible = visible;
		if (PotionsHolder != null) PotionsHolder.Visible = visible;

		if (visible && SlotsGrid != null)
		{
			SlotsGrid.QueueSort();
			Callable.From(UpdateAllPotionPositions).CallDeferred();
		}
	}

	// Вызывается при варке нового зелья котлом
	public bool AddPotionNode(Potion potionNode)
	{
		return PutPotionIntoFreeSlot(potionNode);
	}

	private bool PutPotionIntoFreeSlot(Potion potionNode)
	{
		if (SlotsGrid == null) return false;

		SlotsGrid.QueueSort();

		foreach (Node child in SlotsGrid.GetChildren())
		{
			if (child is PotionSlot slot && slot.IsEmpty())
			{
				if (PotionsHolder != null)
				{
					PotionsHolder.AddChild(potionNode);
				}
				else
				{
					GetTree().CurrentScene.AddChild(potionNode);
				}

				Callable.From(() => slot.PlacePotion(potionNode)).CallDeferred();
				return true;
			}
		}

		GD.PrintErr("Инвентарь зелий полон!");
		return false;
	}

	private void UpdateAllPotionPositions()
	{
		if (SlotsGrid == null) return;

		foreach (Node child in SlotsGrid.GetChildren())
		{
			if (child is PotionSlot slot && !slot.IsEmpty())
			{
				slot.PlacePotion(slot.CurrentPotion);
			}
		}
	}
}
