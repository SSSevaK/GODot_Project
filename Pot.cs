using Godot;

public partial class Pot : Sprite2D
{
	private bool busy = false;

	private PackedScene animationScene;

	private int AnimationX = 68;
	private int AnimationY = 80;

	public override void _Ready()
	{
		animationScene = GD.Load<PackedScene>("res://animated_sprite_2d.tscn");
	}

	public override void _Process(double delta)
	{
		if (busy)
			return;

		Wheat wheat = GetTree().CurrentScene.GetNode<Wheat>("wheat");

		if (GlobalPosition.DistanceTo(wheat.GlobalPosition) < 5)
		{
			busy = true;

			AnimatedSprite2D animation = animationScene.Instantiate<AnimatedSprite2D>();

			GetTree().CurrentScene.AddChild(animation);

			animation.GlobalPosition = new Vector2(AnimationX, AnimationY);

			animation.AnimationFinished += AnimationFinished;

			animation.Play("default");
		}
	}

	private void AnimationFinished()
	{
		busy = false;
	}
}
