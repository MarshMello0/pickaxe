using Godot;

public partial class MainMenu : Node3D
{
	public void LoadGame()
	{
		GetTree().ChangeSceneToFile("res://game/game.tscn");
	}
}
