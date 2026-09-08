using Godot;
using System;

public partial class HealingPotion : Sprite2D
{
    private Node2D bag;
    private PackedScene gold_bag;
    bool input = false;
    bool check = false;
        
    [Signal]
    public delegate void ItemInZoneEventHandler(int id);

    public override void _Ready()
    {
       gold_bag = GD.Load<PackedScene>("res://gold_bag.tscn");
    }


    public override void _Process(double delta)
    {
        if (input)
        {
            GlobalPosition = GetGlobalMousePosition();
        }
        Rect2 zone = new Rect2(633, 531, 100, 10    );
        Rect2 Poisonzone = new Rect2(GlobalPosition - Texture.GetSize()/2,  Texture.GetSize());
        if (zone.Intersects(Poisonzone) && !check)
        {
            EmitSignal(SignalName.ItemInZone, 1);
            check = true;
            GD.Print("yes");
        }
        if (!zone.Intersects(Poisonzone) && check)
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
                    if (GetGlobalMousePosition().DistanceTo(GlobalPosition) < 20)
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
                if (check)
                {
                    if (bag != null)
                    {
                        bag.QueueFree();
                    }
                    bag = gold_bag.Instantiate<Node2D>();
                    GetTree().CurrentScene.AddChild(bag);
                    bag.GlobalPosition = new Vector2(590, 535);
                    GetTree().CurrentScene.GetNode<TextEdit>("TextEdit").SetPotion(this);
                }
                if (!check)
                {
                    if (bag != null && !mouse.Pressed)
                    {
                        bag.QueueFree();
                        bag = null;
                    }
                }
            }
        }
        if (check == false)
        {
            GlobalPosition = new Vector2(391, 584);
        }
    }
}