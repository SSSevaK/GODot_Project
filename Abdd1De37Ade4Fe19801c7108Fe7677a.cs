using Godot;
using System;

public partial class Abdd1De37Ade4Fe19801c7108Fe7677a : Sprite2D
{

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouse)
        {
            if (mouse.Pressed)
            {
                if (GetGlobalMousePosition().DistanceTo(GlobalPosition) > 30)
                {
                    return;
                }
                if (mouse.ButtonIndex == MouseButton.Left)
                {
                    GetTree().CurrentScene.GetNode<TextEdit>("TextEdit").ForSell(1);
                    GetTree().CurrentScene.GetNode<MainHero>("MainHero").Sell(1);
                    QueueFree();
                }

                if (mouse.ButtonIndex == MouseButton.Right)
                {
                    GetTree().CurrentScene.GetNode<TextEdit>("TextEdit").ForSell(0);
                }
            }
        }
    }
}