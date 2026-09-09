using Godot;

public partial class MainHero : StaticBody2D
{
	[Export] public Sprite2D Box { get; set; }
	[Export] public Godot.TextEdit Text { get; set; }
	[Export] public GoldCounter GoldCounter { get; set; }

	[ExportGroup("Позиции движения")]
	[Export] public Vector2 StartPosition { get; set; } = new Vector2(-150, 160);
	[Export] public Vector2 ShopPosition { get; set; } = new Vector2(440, 160);
	[Export] public Vector2 ExitPosition { get; set; } = new Vector2(-150, 160);

	[ExportGroup("Скорость движения")]
	[Export] public float WalkToShopDuration { get; set; } = 3.0f;
	[Export] public float ExitDuration { get; set; } = 1.5f;

	// СТАТИЧЕСКИЕ ПЕРЕМЕННЫЕ: не сбрасываются при смене сцен!
	private static bool _hasActiveOffer = false;
	private static int _savedNeedID = 1;

	public int NeedID = 1;
	private bool check = false;
	private Tween _activeTween;

	public override void _Ready()
	{
		InitNodes();

		if (SceneManager.Instance != null)
		{
			SceneManager.Instance.SceneActivated += OnSceneActivated;
		}

		// Если заказ УЖЕ был создан ранее — сразу восстанавливаем покупателя у стойки
		if (_hasActiveOffer)
		{
			RestoreExistingOffer();
		}
		else
		{
			NewOffer();
		}
	}

	private void OnSceneActivated(string scenePath)
	{
		if (string.IsNullOrEmpty(scenePath)) return;

		if (scenePath.ToLower().Contains("true_lavka"))
		{
			// При возврате в лавку проверяем: есть ли уже активный заказ
			if (_hasActiveOffer)
			{
				RestoreExistingOffer();
			}
			else
			{
				NewOffer();
			}
		}
	}

	/// <summary>
	/// Мгновенно восстанавливает покупателя у стойки без повторного движения
	/// </summary>
	private void RestoreExistingOffer()
	{
		_activeTween?.Kill();
		NeedID = _savedNeedID;
		GlobalPosition = ShopPosition; // Сразу ставим к прилавку
		ShowOffer();
	}

	private void NewOffer()
	{
		check = false;
		Generate();

		// Запоминаем текущий заказ в статические переменные
		_savedNeedID = NeedID;
		_hasActiveOffer = true;

		GlobalPosition = StartPosition;

		_activeTween?.Kill();
		_activeTween = CreateTween();
		_activeTween.TweenProperty(this, "global_position", ShopPosition, WalkToShopDuration);
		_activeTween.Finished += ShowOffer;
	}

	private void ShowOffer()
	{
		if (Box != null) Box.GlobalPosition = new Vector2(551, 34);

		if (Text != null)
		{
			if (NeedID == 1)
			{
				Text.Text = "I need Heal Potion (ID: 1)";
			}
			else if (NeedID == 2)
			{
				Text.Text = "I need Mana Potion (ID: 2)";
			}
		}

		check = true;
	}

	private void Generate()
	{
		NeedID = GD.RandRange(1, 2);
		GD.Print($"[Покупатель] Пришел новый покупатель! Ему нужно зелье с ID = {NeedID}");
	}

	public bool TrySellPotion(Potion potion)
	{
		if (!check || potion == null || potion.RecipeData == null)
		{
			GD.Print("[Продажа] Покупатель ещё не дошёл или зелье пустое!");
			return false;
		}

		if (potion.RecipeData.ID == NeedID)
		{
			GD.Print($"[Продажа] Успешно продано зелье: {potion.RecipeData.ResourceName}");

			if (GoldCounter != null)
			{
				GoldCounter.AddGoldForPotion(potion.RecipeData);
			}

			if (GameManager.Instance != null)
			{
				GameManager.Instance.RemovePotion(potion.RecipeData);
			}

			potion.QueueFree();
			CompleteTransaction();
			return true;
		}

		GD.Print($"[Продажа] Отклонено! Нужен ID={NeedID}, а вы даете ID={potion.RecipeData.ID}");
		return false;
	}

	public void Sell(int id)
	{
		if (id == NeedID && check)
		{
			if (GoldCounter != null)
			{
				GoldCounter.AddGold(id == 1 ? 100 : 75);
			}
			CompleteTransaction();
		}
	}

	private void CompleteTransaction()
	{
		if (Box != null) Box.GlobalPosition = new Vector2(-1000, -1000);
		if (Text != null) Text.Text = "";
		check = false;

		_activeTween?.Kill();
		_activeTween = CreateTween();
		_activeTween.TweenProperty(this, "global_position", ExitPosition, ExitDuration);
		
		// Покупатель ушел за экран — сбрасываем активный заказ и зовем следующего
		_activeTween.Finished += () =>
		{
			_hasActiveOffer = false;
			NewOffer();
		};
	}

	private void InitNodes()
	{
		var parent = GetParent();
		var currentScene = GetTree()?.CurrentScene;

		if (Box == null)
		{
			Box = parent?.GetNodeOrNull<Sprite2D>("DialogBox") 
				  ?? currentScene?.GetNodeOrNull<Sprite2D>("DialogBox");
		}

		if (Text == null)
		{
			Text = parent?.GetNodeOrNull<Godot.TextEdit>("TextEdit2") 
				   ?? currentScene?.GetNodeOrNull<Godot.TextEdit>("TextEdit2");
		}

		if (GoldCounter == null)
		{
			GoldCounter = parent?.GetNodeOrNull<GoldCounter>("TextEdit") 
						  ?? currentScene?.GetNodeOrNull<GoldCounter>("TextEdit");
		}

		if (Text != null)
		{
			Text.AddThemeColorOverride("font_color", Colors.Black);
			Text.AddThemeColorOverride("font_readonly_color", Colors.Black);
			Text.Text = "";
		}
	}
}
