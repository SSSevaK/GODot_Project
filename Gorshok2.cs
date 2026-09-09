using Godot;

public partial class Gorshok2 : StaticBody2D
{
    private bool busy = false;

    private PackedScene animationScene;
    private PackedScene animationScene2;
    AnimatedSprite2D animation1;
    AnimatedSprite2D animation2;

    private int AnimationX = 149;
    private int AnimationY = 30;

    public override void _Ready()
    {
        animationScene = GD.Load<PackedScene>("res://animated_sprite_2d.tscn");
        animationScene2 = GD.Load<PackedScene>("res://animated_sprite_2d_2.tscn");

        animation1 = animationScene.Instantiate<AnimatedSprite2D>();
        animation2 = animationScene2.Instantiate<AnimatedSprite2D>();
    }

    public override void _Process(double delta)
    {
        Wheat wheat = GetTree().CurrentScene.GetNode<Wheat>("wheat");
        Seed seed = GetTree().CurrentScene.GetNode<Seed>("Seed");
        Serp serp = GetTree().CurrentScene.GetNode<Serp>("Serp");

        if (!busy && GlobalPosition.DistanceTo(wheat.GlobalPosition) < 15)
        {
            busy = true;
            GetTree().CurrentScene.AddChild(animation1);
            animation1.GlobalPosition = new Vector2(AnimationX, AnimationY);
            animation1.AnimationFinished += AnimationFinished;
            animation1.Play("default");
        }

        if (!busy && GlobalPosition.DistanceTo(seed.GlobalPosition) < 15)
        {
            busy = true;
            GetTree().CurrentScene.AddChild(animation2);
            animation2.GlobalPosition = new Vector2(AnimationX, AnimationY);
            animation2.AnimationFinished += AnimationFinished;
            animation2.Play("default");
        }

        if (!busy && GlobalPosition.DistanceTo(serp.GlobalPosition) < 15)
        {

            animation1.QueueFree();
            animation2.QueueFree();
            animation1 = animationScene.Instantiate<AnimatedSprite2D>();
            animation2 = animationScene2.Instantiate<AnimatedSprite2D>();
        }
    }

    private void AnimationFinished()
    {
        busy = false;
    }
}
