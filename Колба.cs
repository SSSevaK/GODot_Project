	using Godot;
	using System;

	public partial class Колба : Sprite2D
	{
		bool input = false;
		public override void _Ready()
		{

		}

		public override void _Process(double delta)
		{
		if (input)
		{
			GlobalPosition = GetGlobalMousePosition();
		}
		}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse)
		{
			if (mouse.Pressed)
			{
				if (mouse.ButtonIndex == MouseButton.Left)
				{
					if (GetGlobalMousePosition().DistanceTo(GlobalPosition) < 50)
					{
                         input = true;
                    }
					else
					{
						 input = false;
					}
				}
			}
            else
            {
                input = false;
            }
        }
	}
}