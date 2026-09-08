using Godot;
using System;

public partial class TextEdit : Godot.TextEdit
{
    // 1 HealFlask
    int Gold = 0;
    int Gold_HealFlask = 100;
    private HealingPotion potionOnTable;
    public override void _Ready()
    {

        Text = Gold.ToString();
    }

    public void ForSell (int id)
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

        if (id == 2)
        {
            GD.Print("Торговля");

        }

    }
    public void SetPotion(HealingPotion potion)
    {
        potionOnTable = potion;
    }
}