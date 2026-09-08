using Godot;

[Tool]
public partial class Ingredient : Area2D
{
	private Resource _itemData;

	[Export]
	public Resource ItemData
	{
		get => _itemData;
		set
		{
			_itemData = value;
			UpdateSpriteFromData();
		}
	}

	private Sprite2D _sprite;
	private bool _isDragging = false;
	private Vector2 _offset = Vector2.Zero;
	private Slot _currentSlot = null;

	public override void _Ready()
	{
		_sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
		UpdateSpriteFromData();
	}

	private void UpdateSpriteFromData()
	{
		if (_sprite == null)
		{
			_sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
		}

		if (_sprite != null && _itemData is IngredientData data && data.Icon != null)
		{
			_sprite.Texture = data.Icon;
		}
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint()) return;

		if (_isDragging)
		{
			GlobalPosition = GetGlobalMousePosition() + _offset;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Engine.IsEditorHint()) return;

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
			{
				return true;
			}
		}

		return false;
	}

	private void CheckDropZone()
	{
		var overlappingAreas = GetOverlappingAreas();

		foreach (var area in overlappingAreas)
		{
			if (area is Cauldron cauldron)
			{
				cauldron.AddIngredient(this);
				return;
			}

			if (area is Slot slot && slot.IsFree())
			{
				slot.PlaceIngredient(this);
				_currentSlot = slot;
				return;
			}
		}
	}
}
