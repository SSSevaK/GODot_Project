using Godot;
using System;

public partial class ManaPotion : Sprite2D
{
    bool input = false;
    bool check = false;

    [Signal]
    public delegate void ItemInZoneEventHandler(int id);

    public override void _Ready()
    {
    }


    public override void _Process(double delta)
    {
        if (input)
        {
            GlobalPosition = GetGlobalMousePosition();
        }
        Rect2 zone = new Rect2(633, 531, 100, 10);
        Rect2 Posisonzone = new Rect2(GlobalPosition - Texture.GetSize() / 2, Texture.GetSize());
        if (zone.Intersects(Posisonzone) && !check)
        {
            EmitSignal(SignalName.ItemInZone, 1);
            check = true;
        }
        if (!zone.Intersects(Posisonzone) && check)
        {
            check = false;
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
                    if (GetGlobalMousePosition().DistanceTo(GlobalPosition) < 10)
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
        if (check == false)
        {
            GlobalPosition = new Vector2(441, 588);
        }
    }
}