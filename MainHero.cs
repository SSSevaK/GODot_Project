using Godot;
using System;

public partial class MainHero : StaticBody2D
{
    Sprite2D Box;
    Godot.TextEdit Text;

    public int NeedID = 1;

    bool check = false;

    Vector2 StartPosition = new Vector2(100, 442);
    Vector2 ShopPosition = new Vector2(660, 442);
    Vector2 ExitPosition = new Vector2(1015, 442);

    public override void _Ready()
    {
        Box = GetTree().CurrentScene.GetNode<Sprite2D>("DialogBox");
        Text = GetTree().CurrentScene.GetNode<Godot.TextEdit>("TextEdit2");

        Text.AddThemeColorOverride("font_color", Colors.Black);
        Text.AddThemeColorOverride("font_readonly_color", Colors.Black);

        Text.Text = "";

        NewOffer();
    }
    private void NewOffer()
    {
        check = false;
        Generate();
        GlobalPosition = StartPosition;
        Tween tween = CreateTween();
        tween.TweenProperty(this, "position", ShopPosition, 3f);
        tween.Finished += ShowOffer;
    }

    private void ShowOffer()
    {
        Box.GlobalPosition = new Vector2(525, 400);

        if (NeedID == 1)
        {
            Text.Text = "I need Water";
        }
        else if (NeedID == 2)
        {
            Text.Text = "I need Potion";
        }

        check = true;
    }
    private void Generate()
    {
        NeedID = GD.RandRange(1, 2);

        GD.Print("Новое предложение: NeedID = ", NeedID);
    }

    public void Sell(int id)
    {
        if (id == NeedID && check)
        {
            GD.Print("Продажа успешна!");

            Box.GlobalPosition = new Vector2(10, 10);
            Text.Text = "";

            check = false;

            Tween tween = CreateTween();
            tween.TweenProperty(this, "position", ExitPosition, 1f);

            tween.Finished += NewOffer;
        }
    }
}