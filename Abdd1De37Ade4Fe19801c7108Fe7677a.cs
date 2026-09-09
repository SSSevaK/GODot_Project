using Godot;
using System;

public partial class Abdd1De37Ade4Fe19801c7108Fe7677a : Sprite2D
{
	public int ID = 1;
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
					GetTree().CurrentScene.GetNode<GoldCounter>("GoldCounter").ForSell(ID);
					GetTree().CurrentScene.GetNode<MainHero>("MainHero").Sell(ID);
					QueueFree();
				}

				if (mouse.ButtonIndex == MouseButton.Right)
				{
					GetTree().CurrentScene.GetNode<GoldCounter>("GoldCounter").ForSell(0);
				}
			}
		}
	}
}
