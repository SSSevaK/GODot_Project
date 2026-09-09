using Godot;

public partial class Gorshok7 : StaticBody2D
{
    private bool busy = false;

    private PackedScene animationScene;
    private PackedScene animationScene2;

    private int AnimationX = 410;
    private int AnimationY = 141;

    public override void _Ready()
    {
        animationScene = GD.Load<PackedScene>("res://animated_sprite_2d.tscn");
        animationScene2 = GD.Load<PackedScene>("res://animated_sprite_2d_2.tscn");
    }

    public override void _Process(double delta)
    {
        Wheat wheat = GetTree().CurrentScene.GetNode<Wheat>("wheat");
        Seed seed = GetTree().CurrentScene.GetNode<Seed>("Seed");

        if (!busy && GlobalPosition.DistanceTo(wheat.GlobalPosition) < 15)
        {
            busy = true;
            AnimatedSprite2D animation = animationScene.Instantiate<AnimatedSprite2D>();
            GetTree().CurrentScene.AddChild(animation);
            animation.GlobalPosition = new Vector2(AnimationX, AnimationY);
            animation.AnimationFinished += AnimationFinished;
            animation.Play("default");
        }

        if (!busy && GlobalPosition.DistanceTo(seed.GlobalPosition) < 15)
        {
            busy = true;
            AnimatedSprite2D animation = animationScene2.Instantiate<AnimatedSprite2D>();
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
