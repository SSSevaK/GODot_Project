using Godot;
using System;

public partial class TextEdit : Godot.TextEdit
{
    // blue is 1
    int Gold = 0;
    public override void _Ready()
	{
        GetNode<Колба>("../flask/Колба").ItemInZone += CheckItem;
        Text = Gold.ToString();

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
    private void CheckItem(int id)
    {
        if (id == 1)
        {
            GD.Print("Получено 100");
            Gold += 100;
            Text = Gold.ToString();
        }
    }
}
