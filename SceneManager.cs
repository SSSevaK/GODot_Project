using Godot;
using System.Collections.Generic;

public partial class SceneManager : Node
{
    public static SceneManager Instance { get; private set; }

    [Signal]
    public delegate void SceneActivatedEventHandler(string scenePath);

    private readonly Dictionary<string, Node> _cachedScenes = new Dictionary<string, Node>();

    private PackedScene ingredientScene;

    private IngredientData wheatData;
    private IngredientData seedData;

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
            return;
        }

        var currentScene = GetTree().CurrentScene;

        if (currentScene != null && !string.IsNullOrEmpty(currentScene.SceneFilePath))
        {
            _cachedScenes[currentScene.SceneFilePath] = currentScene;
        }

        ingredientScene = GD.Load<PackedScene>("res://ingredient.tscn");

        wheatData = GD.Load<IngredientData>("res://IceWheat.tres");
        seedData = GD.Load<IngredientData>("res://NeilFlower.tres");
    }

    public void SwitchScene(string scenePath)
    {
        Callable.From(() => SwitchSceneInternal(scenePath)).CallDeferred();
    }

    private void SwitchSceneInternal(string scenePath)
    {
        var root = GetTree().Root;
        var currentScene = GetTree().CurrentScene;

        if (currentScene != null)
        {
            if (!string.IsNullOrEmpty(currentScene.SceneFilePath))
            {
                _cachedScenes[currentScene.SceneFilePath] = currentScene;
            }

            root.RemoveChild(currentScene);
        }

        Node targetScene;

        if (_cachedScenes.TryGetValue(scenePath, out var cachedNode) &&
            GodotObject.IsInstanceValid(cachedNode))
        {
            targetScene = cachedNode;
        }
        else
        {
            PackedScene packedScene = GD.Load<PackedScene>(scenePath);

            if (packedScene == null)
            {
                return;
            }

            targetScene = packedScene.Instantiate();
            _cachedScenes[scenePath] = targetScene;
        }

        root.AddChild(targetScene);
        GetTree().CurrentScene = targetScene;

        if (GetTree().CurrentScene.SceneFilePath == "res://lab_scene.tscn")
        {
            Gorshok gorshok = FindGorshok();

            if (gorshok != null)
            {
                if (gorshok.grewWheat)
                {
                    Ingredient ingredient = ingredientScene.Instantiate<Ingredient>();

                    ingredient.ItemData = wheatData;

                    targetScene.AddChild(ingredient);
                    ingredient.GlobalPosition = new Vector2(101, 268);

                    gorshok.grewWheat = false;
                }

                if (gorshok.grewSeed)
                {
                    Ingredient ingredient = ingredientScene.Instantiate<Ingredient>();

                    ingredient.ItemData = seedData;

                    targetScene.AddChild(ingredient);
                    ingredient.GlobalPosition = new Vector2(101, 268);

                    gorshok.grewSeed = false;
                }
            }
        }

        EmitSignal(SignalName.SceneActivated, scenePath);
    }

    private Gorshok FindGorshok()
    {
        foreach (Node scene in _cachedScenes.Values)
        {
            if (scene == null || !GodotObject.IsInstanceValid(scene))
                continue;

            Gorshok gorshok = FindGorshokInNode(scene);

            if (gorshok != null)
                return gorshok;
        }

        return null;
    }

    private Gorshok FindGorshokInNode(Node node)
    {
        if (node is Gorshok gorshok)
            return gorshok;

        foreach (Node child in node.GetChildren())
        {
            Gorshok result = FindGorshokInNode(child);

            if (result != null)
                return result;
        }

        return null;
    }
}