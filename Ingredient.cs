using Godot;

public partial class Ingredient : Area2D
{
	[Export]
	public Resource ItemData { get; set; }

	private bool _isDragging = false;
	private Vector2 _offset = Vector2.Zero;
	private Slot _currentSlot = null;

	public override void _Ready()
	{
		// Переменная _area2D больше не нужна, так как сам этот класс (this) — и есть Area2D!
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
		// Проверяем пересечение мыши с САМИМ СОБОЙ (this)
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
		// Проверяем перекрытия слотов относительно самого себя (this)
		var overlappingAreas = GetOverlappingAreas();

		foreach (var area in overlappingAreas)
		{
			if (area is Slot slot && slot.IsFree())
			{
				slot.PlaceIngredient(this);
				_currentSlot = slot;
				return;
			}
		}
	}
}
