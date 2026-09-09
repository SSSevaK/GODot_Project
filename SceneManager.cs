using Godot;
using System.Collections.Generic;

public partial class SceneManager : Node
{
	public static SceneManager Instance { get; private set; }

	// Сигнал оповещения об активации сцены
	[Signal]
	public delegate void SceneActivatedEventHandler(string scenePath);

	private readonly Dictionary<string, Node> _cachedScenes = new Dictionary<string, Node>();

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
		if (_cachedScenes.TryGetValue(scenePath, out var cachedNode) && GodotObject.IsInstanceValid(cachedNode))
		{
			targetScene = cachedNode;
		}
		else
		{
			PackedScene packedScene = GD.Load<PackedScene>(scenePath);
			if (packedScene == null)
			{
				GD.PrintErr($"[SceneManager] Ошибка: Не удалось загрузить сцену по пути: {scenePath}");
				return;
			}

			targetScene = packedScene.Instantiate();
			_cachedScenes[scenePath] = targetScene;
		}

		root.AddChild(targetScene);
		GetTree().CurrentScene = targetScene;

		// Автоматически посылаем сигнал всем подпискам!
		EmitSignal(SignalName.SceneActivated, scenePath);
	}
}
