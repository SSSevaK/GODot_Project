using Godot;

public partial class MainHero : StaticBody2D
{
	[Export] public Sprite2D Box { get; set; }
	[Export] public Godot.TextEdit Text { get; set; }
	[Export] public GoldCounter GoldCounter { get; set; }

	

	public int NeedID = 1;
	private bool check = false;
	private Tween _activeTween;

	private Vector2 StartPosition = new Vector2(172, 152);
	private Vector2 ShopPosition = new Vector2(440, 160);
	private Vector2 ExitPosition = new Vector2(1015, 442);

	public override void _Ready()
	{
	// 1. Безопасно ищем узлы через родителя или CurrentScene (если они не назначены в Инспекторе)
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

	// 2. Настраиваем текст диалога
	if (Text != null)
	{
		Text.AddThemeColorOverride("font_color", Colors.Black);
		Text.AddThemeColorOverride("font_readonly_color", Colors.Black);
		Text.Text = "";
	}

	// 3. Подписываемся на SceneManager
	if (SceneManager.Instance != null)
	{
		SceneManager.Instance.SceneActivated += OnSceneActivated;
	}

	GlobalPosition = StartPosition;
	NewOffer();
	}

	// Вызывается автоматически через SceneManager при каждом возврате в сцену
	private void OnSceneActivated(string scenePath)
	{
	if (string.IsNullOrEmpty(scenePath)) return;

	if (scenePath.ToLower().Contains("true_lavka"))
	{
		// Если покупатель уже стоит у стойки и ждёт зелье — не отправляем его на старт
		if (check)
		{
			// Восстанавливаем позицию диалогового окна на всякий случай
			ShowOffer();
			return;
		}

		// Если его еще нет у стойки — запускаем движение
		NewOffer();
	}
	}

	private void NewOffer()
	{
		check = false;
		Generate();

		

		_activeTween?.Kill();
		_activeTween = CreateTween();
		_activeTween.TweenProperty(this, "global_position", ShopPosition, 3f);
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
		_activeTween.TweenProperty(this, "global_position", ExitPosition, 1f);
		_activeTween.Finished += NewOffer;
	}
}
