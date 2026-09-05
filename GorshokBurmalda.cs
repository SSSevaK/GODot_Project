using Godot;
using System;

public partial class GorshokBurmalda : Area2D
{
	// Поля для текстур ростка и урожая (настраиваются в инспекторе)
	[Export] public Texture2D SproutTexture; 
	[Export] public Texture2D ReadyTexture;  

	private Sprite2D _plantSprite; // ИСПРАВЛЕНО НА Sprite2D
	private Timer _growthTimer;
	private Label _timeLabel;
	private Random _random = new Random();

	private int _minTime = 5;  
	private int _maxTime = 15; 
	private bool _isGrowing = false;

	public override void _Ready()
	{
		// ИСПРАВЛЕНО НА Sprite2D и имя строго с маленькой буквы
		_plantSprite = GetNode<Sprite2D>("sprite_with_rastenie");
		_growthTimer = GetNode<Timer>("Timer");
		_timeLabel = GetNode<Label>("vremya_rosta");

		_timeLabel.Text = "";
		_plantSprite.Visible = false;
		_plantSprite.Texture = null; 

		_growthTimer.Timeout += OnGrowthTimeout;
		this.InputEvent += OnInputEvent;
	}

	public override void _Process(double delta)
	{
		if (_isGrowing && !_growthTimer.IsStopped())
		{
			float timeLeft = (float)_growthTimer.TimeLeft;
			_timeLabel.Text = $"{timeLeft:F1} сек";
		}
	}

	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			var inventoryNode = GetNode<seed_scr_burmalda>("/root/SeedScrBurmalda");

			if (!_isGrowing)
			{
				string activeSeed = inventoryNode.SelectedSeedType;

				if (string.IsNullOrEmpty(activeSeed))
				{
					GD.Print("Сначала выберите семена в инвентаре!");
					_timeLabel.Text = "Выбери слот!";
					return;
				}

				if (inventoryNode.Inventory.ContainsKey(activeSeed) && inventoryNode.Inventory[activeSeed] > 0)
				{
					inventoryNode.ChangeIngredient(activeSeed, -1);
					StartGrowth();
				}
				else
				{
					GD.Print($"Семена {activeSeed} закончились!");
					_timeLabel.Text = "0 шт!";
				}
			}
			else if (_growthTimer.IsStopped())
			{
				Harvest();
			}
		}
	}

	private void StartGrowth()
	{
		_isGrowing = true;
		_timeLabel.Text = ""; 
		
		int randomSeconds = _random.Next(_minTime, _maxTime + 1);
		GD.Print($"Посажено семя! Время роста: {randomSeconds} сек.");

		if (SproutTexture != null)
		{
			_plantSprite.Texture = SproutTexture;
		}
		_plantSprite.Visible = true; 

		_growthTimer.WaitTime = randomSeconds;
		_growthTimer.Start();
	}

	private void OnGrowthTimeout()
	{
		_timeLabel.Text = "Готово!";
		GD.Print("Созрело! Кликните для сбора.");

		if (ReadyTexture != null)
		{
			_plantSprite.Texture = ReadyTexture;
		}
	}

	private void Harvest()
	{
		GD.Print("Урожай успешно собран!");
		
		_isGrowing = false; 
		_plantSprite.Visible = false;
		_plantSprite.Texture = null; 
		_timeLabel.Text = "";
	}
}
