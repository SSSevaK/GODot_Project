using Godot;

public partial class Gorshok11 : StaticBody2D
{
    private bool busy = false;
    private bool canPlant = true;

    private PackedScene animationScene;
    private PackedScene animationScene2;

    AnimatedSprite2D animation1;
    AnimatedSprite2D animation2;

    private int AnimationX = 412;
    private int AnimationY = 266;

    public bool grewWheat = false;
    public bool grewSeed = false;

    public override void _Ready()
    {
        animationScene = GD.Load<PackedScene>("res://animated_sprite_2d.tscn");
        animationScene2 = GD.Load<PackedScene>("res://animated_sprite_2d_2.tscn");

        animation1 = animationScene.Instantiate<AnimatedSprite2D>();
        animation2 = animationScene2.Instantiate<AnimatedSprite2D>();

        animation1.SpriteFrames.SetAnimationLoop("default", false);
        animation2.SpriteFrames.SetAnimationLoop("default", false);
    }

    public override void _Process(double delta)
    {
        Wheat wheat = GetTree().CurrentScene.GetNode<Wheat>("wheat");
        Seed seed = GetTree().CurrentScene.GetNode<Seed>("Seed");
        Serp serp = GetTree().CurrentScene.GetNode<Serp>("Serp");

        if (!busy && canPlant &&
            GlobalPosition.DistanceTo(wheat.GlobalPosition) < 15)
        {
            busy = true;
            canPlant = false;

            grewWheat = true;
            grewSeed = false;

            GetTree().CurrentScene.AddChild(animation1);

            animation1.GlobalPosition = new Vector2(AnimationX, AnimationY);

            animation1.AnimationFinished -= AnimationFinished;
            animation1.AnimationFinished += AnimationFinished;

            animation1.Play("default");
        }

        else if (!busy && canPlant &&
                 GlobalPosition.DistanceTo(seed.GlobalPosition) < 15)
        {
            busy = true;
            canPlant = false;

            grewSeed = true;
            grewWheat = false;

            GetTree().CurrentScene.AddChild(animation2);

            animation2.GlobalPosition = new Vector2(AnimationX, AnimationY);

            animation2.AnimationFinished -= AnimationFinished;
            animation2.AnimationFinished += AnimationFinished;

            animation2.Play("default");
        }

        if (!busy && GlobalPosition.DistanceTo(serp.GlobalPosition) < 15)
        {
            if (grewWheat || grewSeed)
            {
                animation1.QueueFree();
                animation2.QueueFree();

                animation1 = animationScene.Instantiate<AnimatedSprite2D>();
                animation2 = animationScene2.Instantiate<AnimatedSprite2D>();

                animation1.SpriteFrames.SetAnimationLoop("default", false);
                animation2.SpriteFrames.SetAnimationLoop("default", false);

                canPlant = true;
            }
        }
    }

    private void AnimationFinished()
    {
        busy = false;
    }
}