using Godot;

public partial class SceneTransition : Button
{
	// Путь к сцене, куда переходим (задается в инспекторе)
	[Export(PropertyHint.File, "*.tscn")] 
	public string TargetScenePath { get; set; }

	public override void _Ready()
	{
		Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		if (!string.IsNullOrEmpty(TargetScenePath))
		{
			GetTree().ChangeSceneToFile(TargetScenePath);
		}
		else
		{
			GD.PrintErr("[SceneTransition] Не указан путь к целевой сцене!");
		}
	}
}
