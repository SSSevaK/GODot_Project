using Godot;
using System;

public partial class Arrow : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse &&
			mouse.ButtonIndex == MouseButton.Left)
		{
			if (mouse.Pressed)
			{
				if (GlobalPosition.DistanceTo(mouse.GlobalPosition) < 30)
				{
					GetTree().ChangeSceneToFile("res://lab_scene.tscn");
				}
			}
		}
	}
}
