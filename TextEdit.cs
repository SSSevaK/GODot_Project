using Godot;
using System;

public partial class TextEdit : Godot.TextEdit
{
    int Gold = 0;
    private HealingPotion potionOnTable;
    public override void _Ready()
    {
        Text = Gold.ToString();
    }

    public void ForSell (int id)
    {
        if (id == 1)
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

    }
    public void SetPotion(HealingPotion potion)
    {
        potionOnTable = potion;
    }
}