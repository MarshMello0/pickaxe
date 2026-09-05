using Godot;

public partial class Character : MeshInstance3D
{
	[ExportGroup("Targets")]
	[Export] private Marker3D _mouseMarker;
	[Export] private Marker3D _rightArmMarker;
	[Export] private Marker3D _leftArmMarker;

	private Viewport _viewport;
	private Camera3D _camera;

	public override void _Process(double delta)
	{
		base._Process(delta);
		var pos = GetMouseWorldPosition();
		_mouseMarker.GlobalPosition = pos;
	}

	private Vector3 GetMouseWorldPosition()
	{
		_viewport ??= GetViewport();
		_camera ??= _viewport.GetCamera3D();

		var pos = _viewport.GetMousePosition();
		return _camera.ProjectPosition(pos, 0);
	}
}
