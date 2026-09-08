using Godot;
using System;

public partial class TextEdit : Godot.TextEdit
{
    // 1 HealFlask
    // 2 ManaPotion

    int Gold = 0;
    int Gold_HealFlask = 100;

    private Sprite2D potionOnTable;

    public override void _Ready()
    {
        AddThemeColorOverride("font_color", Colors.Black);
        AddThemeColorOverride("font_readonly_color", Colors.Black);
        Text = Gold.ToString();
    }

    public void ForSell(int id)
    {
        MainHero hero = GetTree().CurrentScene.GetNode<MainHero>("MainHero");

        if (id == 1 && hero.NeedID == 1)
        {
            GD.Print("Получено 100");

            Gold += 100;
            Text = Gold.ToString();

            if (potionOnTable != null)
            {
                potionOnTable.QueueFree();
                potionOnTable = null;
            }
        }

        if (id == 0)
        {
            GD.Print("Торговля");
        }

        if (id == 2 && hero.NeedID == 2)
        {
            GD.Print("Получено 75");

            Gold += 75;
            Text = Gold.ToString();

            if (potionOnTable != null)
            {
                potionOnTable.QueueFree();
                potionOnTable = null;
            }
        }
    }

    public void SetPotion(Sprite2D potion)
    {
        potionOnTable = potion;
    }
}