using Godot;

public partial class Slot : Area2D
{
    public Node2D CurrentIngredient { get; private set; } = null;

    public bool IsFree()
    {
        return CurrentIngredient == null;
    }

    public void PlaceIngredient(Node2D ingredient)
    {
        if (IsFree())
        {
            CurrentIngredient = ingredient;
            ingredient.GlobalPosition = GlobalPosition;
        }
        else
        {
            ingredient.GlobalPosition = GlobalPosition + new Vector2(0, 33);
        }
    }

    public void ClearSlot()
    {
        CurrentIngredient = null;
    }
}