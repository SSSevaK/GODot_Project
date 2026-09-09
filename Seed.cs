using Godot;

public partial class Seed : StaticBody2D
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
                GlobalPosition = new Vector2(478 , 74);
            }
        }

        if (@event is InputEventMouseMotion motion && dragging)
        {
            GlobalPosition = motion.GlobalPosition;
        }
    }
}
