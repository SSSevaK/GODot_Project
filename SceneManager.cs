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
            FindAllGorshoks();
        }

        EmitSignal(SignalName.SceneActivated, scenePath);
    }

    private void FindAllGorshoks()
    {
        foreach (Node scene in _cachedScenes.Values)
        {
            if (scene == null || !GodotObject.IsInstanceValid(scene))
                continue;

            FindAllGorshoksInNode(scene);
        }
    }

    private void FindAllGorshoksInNode(Node node)
    {
        if (node is Gorshok gorshok)
        {
            CreateIngredients(gorshok.grewWheat, gorshok.grewSeed);

            gorshok.grewWheat = false;
            gorshok.grewSeed = false;
        }

        if (node is Gorshok2 gorshok2)
        {
            CreateIngredients(gorshok2.grewWheat, gorshok2.grewSeed);

            gorshok2.grewWheat = false;
            gorshok2.grewSeed = false;
        }

        if (node is Gorshok3 gorshok3)
        {
            CreateIngredients(gorshok3.grewWheat, gorshok3.grewSeed);

            gorshok3.grewWheat = false;
            gorshok3.grewSeed = false;
        }

        if (node is Gorshok4 gorshok4)
        {
            CreateIngredients(gorshok4.grewWheat, gorshok4.grewSeed);

            gorshok4.grewWheat = false;
            gorshok4.grewSeed = false;
        }

        if (node is Gorshok5 gorshok5)
        {
            CreateIngredients(gorshok5.grewWheat, gorshok5.grewSeed);

            gorshok5.grewWheat = false;
            gorshok5.grewSeed = false;
        }

        if (node is Gorshok6 gorshok6)
        {
            CreateIngredients(gorshok6.grewWheat, gorshok6.grewSeed);

            gorshok6.grewWheat = false;
            gorshok6.grewSeed = false;
        }

        if (node is Gorshok7 gorshok7)
        {
            CreateIngredients(gorshok7.grewWheat, gorshok7.grewSeed);

            gorshok7.grewWheat = false;
            gorshok7.grewSeed = false;
        }

        if (node is Gorshok8 gorshok8)
        {
            CreateIngredients(gorshok8.grewWheat, gorshok8.grewSeed);

            gorshok8.grewWheat = false;
            gorshok8.grewSeed = false;
        }

        if (node is Gorshok9 gorshok9)
        {
            CreateIngredients(gorshok9.grewWheat, gorshok9.grewSeed);

            gorshok9.grewWheat = false;
            gorshok9.grewSeed = false;
        }

        if (node is Gorshok10 gorshok10)
        {
            CreateIngredients(gorshok10.grewWheat, gorshok10.grewSeed);

            gorshok10.grewWheat = false;
            gorshok10.grewSeed = false;
        }

        if (node is Gorshok11 gorshok11)
        {
            CreateIngredients(gorshok11.grewWheat, gorshok11.grewSeed);

            gorshok11.grewWheat = false;
            gorshok11.grewSeed = false;
        }

        if (node is Gorshok12 gorshok12)
        {
            CreateIngredients(gorshok12.grewWheat, gorshok12.grewSeed);

            gorshok12.grewWheat = false;
            gorshok12.grewSeed = false;
        }

        foreach (Node child in node.GetChildren())
        {
            FindAllGorshoksInNode(child);
        }
    }

    private void CreateIngredients(bool grewWheat, bool grewSeed)
    {
        if (grewWheat)
        {
            Ingredient ingredient = ingredientScene.Instantiate<Ingredient>();

            ingredient.ItemData = wheatData;

            GetTree().CurrentScene.AddChild(ingredient);
            ingredient.GlobalPosition = new Vector2(101, 268);
        }

        if (grewSeed)
        {
            Ingredient ingredient = ingredientScene.Instantiate<Ingredient>();

            ingredient.ItemData = seedData;

            GetTree().CurrentScene.AddChild(ingredient);
            ingredient.GlobalPosition = new Vector2(101, 268);
        }
    }
}