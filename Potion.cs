using Godot;

public partial class Potion : Area2D
{
	[Export] public PotionRecipe RecipeData { get; set; }

	private Sprite2D _sprite;
	private bool _isDragging = false;
	private Vector2 _offset = Vector2.Zero;
	private ShopSlot _currentSlot = null;

	public override void _Ready()
	{
		InitSprite();
	}

	public void Setup(PotionRecipe recipe)
	{
		RecipeData = recipe;
		InitSprite();
	}

	public void SetSlot(ShopSlot slot)
	{
		_currentSlot = slot;
	}

	private void InitSprite()
	{
		if (_sprite == null) _sprite = GetNodeOrNull<Sprite2D>("Sprite2D");

		if (_sprite != null && RecipeData != null)
		{
			if (RecipeData.Icon != null)
			{
				_sprite.Texture = RecipeData.Icon;
			}
			else
			{
				GD.PrintErr($"[Potion] У ресурса '{RecipeData.PotionName}' не назначена Иконка (Icon) в Инспекторе!");
			}
		}
	}

	public override void _Process(double delta)
	{
		if (_isDragging)
		{
			GlobalPosition = GetGlobalMousePosition() + _offset;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			if (mouseEvent.Pressed)
			{
				if (IsMouseOver())
				{
					_isDragging = true;
					_offset = GlobalPosition - GetGlobalMousePosition();

					if (_currentSlot != null)
					{
						_currentSlot.ClearSlot();
						_currentSlot = null;
					}

					GetViewport().SetInputAsHandled();
				}
			}
			else if (_isDragging)
			{
				_isDragging = false;
				CheckDropZone();
			}
		}
	}

	private bool IsMouseOver()
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		var query = new PhysicsPointQueryParameters2D
		{
			Position = GetGlobalMousePosition(),
			CollideWithAreas = true,
			CollideWithBodies = false
		};

		var result = spaceState.IntersectPoint(query);
		foreach (var hit in result)
		{
			if ((GodotObject)hit["collider"] == this)
				return true;
		}

		return false;
	}

	private void CheckDropZone()
	{
		var overlappingBodies = GetOverlappingBodies();
		foreach (var body in overlappingBodies)
		{
			if (body is MainHero hero)
			{
				if (hero.TrySellPotion(this))
				{
					return;
				}
			}
		}

		var overlappingAreas = GetOverlappingAreas();
		foreach (var area in overlappingAreas)
		{
			if (area is ShopSlot slot && slot.IsFree())
			{
				slot.PlaceItem(this);
				return;
			}
		}

		// Возвращаем в прежний слот, если отпустили мимо
		if (_currentSlot != null)
		{
			_currentSlot.PlaceItem(this);
		}
	}
}
