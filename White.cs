using Godot;
using System;

public partial class White : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2 B = new Vector2(485, 186);
		Tween tween = CreateTween();
		tween.TweenProperty(this, "position", B, 3f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
