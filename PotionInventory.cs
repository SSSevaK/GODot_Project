using Godot;

public partial class PotionInventory : Control
{
	[Export] public Button OpenButton { get; set; }
	[Export] public Control InventoryPanel { get; set; }
	[Export] public GridContainer SlotsGrid { get; set; }
	[Export] public Node2D PotionsHolder { get; set; }
	[Export] public PackedScene PotionScene { get; set; }
	[Export] public PackedScene SlotScene { get; set; }
	[Export] public int TotalSlots { get; set; } = 9;

	public override void _Ready()
	{
		SetInventoryVisible(false);

		if (OpenButton != null)
		{
			OpenButton.Pressed += () => SetInventoryVisible(!InventoryPanel.Visible);
		}

		InitSlots();

		if (SceneManager.Instance != null)
		{
			SceneManager.Instance.SceneActivated += OnSceneActivated;
		}

		Callable.From(RestorePotionsFromGameManager).CallDeferred();
	}

	private void OnSceneActivated(string scenePath)
	{
		if (!string.IsNullOrEmpty(scenePath) && scenePath.ToLower().Contains("lab"))
		{
			Callable.From(RestorePotionsFromGameManager).CallDeferred();
		}
	}

	private void InitSlots()
	{
		if (SlotsGrid == null) return;

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

		if (PotionsHolder != null)
		{
			foreach (Node child in PotionsHolder.GetChildren())
			{
				PotionsHolder.RemoveChild(child);
				child.QueueFree();
			}
		}

		foreach (Node child in SlotsGrid.GetChildren())
		{
			if (child is PotionSlot slot)
			{
				slot.ClearSlot();
			}
		}

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
