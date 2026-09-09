using Godot;

public partial class Wheat : StaticBody2D
{
	private bool dragging = false;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse &&
			mouse.ButtonIndex == MouseButton.Left)
		{
			if (mouse.Pressed)
			{
				if (GlobalPosition.DistanceTo(mouse.GlobalPosition) < 30)
				{
					dragging = true;
				}
			}
			else
			{
				dragging = false;
				GlobalPosition = new Vector2(558, 73);
			}
		}

		if (@event is InputEventMouseMotion motion && dragging)
		{
			GlobalPosition = motion.GlobalPosition;
		}
	}
}
