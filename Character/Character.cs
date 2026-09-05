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
		_mouseMarker.GlobalPosition = new Vector3(pos.X, pos.Y, 0);
	}

	private Vector3 GetMouseWorldPosition()
	{
		_viewport ??= GetViewport();
		_camera ??= _viewport.GetCamera3D();

		var pos = _viewport.GetMousePosition();
		return _camera.ProjectPosition(pos, 0);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventMouseButton mouseEvent)
		{
			switch (mouseEvent.ButtonIndex)
			{
				case MouseButton.Left:
					if (mouseEvent.Pressed)
					{
						_leftArmMarker.GlobalPosition = GetMouseWorldPosition();
						_leftArmMarker.Reparent(_mouseMarker);
						_rightArmMarker.Reparent(this);
					}
					else if (mouseEvent.IsReleased())
					{
						_leftArmMarker.Reparent(this);
						_rightArmMarker.Reparent(this);
					}
					break;
				case MouseButton.Right:
					if (mouseEvent.Pressed)
					{
						_rightArmMarker.GlobalPosition = GetMouseWorldPosition();
						_leftArmMarker.Reparent(this);
						_rightArmMarker.Reparent(_mouseMarker);
					}
					else if (mouseEvent.IsReleased())
					{
						_leftArmMarker.Reparent(this);
						_rightArmMarker.Reparent(this);
					}
					break;
			}
		}
	}
}
