using Godot;
using System;

public partial class Колба : Sprite2D
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
        Rect2 zone = new Rect2(367, 519, 25, 25);
        if (zone.HasPoint(GlobalPosition) && !check)
        {
            GD.Print("Poizon in area");
            EmitSignal(SignalName.ItemInZone, 1);
            check = true;
        }
        if (!zone.HasPoint(GlobalPosition) && check)
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