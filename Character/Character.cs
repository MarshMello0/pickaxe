using Godot;

public partial class Character : RigidBody3D
{
	[Export] private Skeleton3D _skeleton;
	[Export] private Fabrik3D _ik;
	[Export] private Marker3D _mouseMarker;
	[Export] private float _armLength = 1.0f;

	private Viewport _viewport;
	private Camera3D _camera;
	private bool _canStick;
	private bool _isStuck;
	private Vector3 _offset;
	private Vector3 _armStuckPosition;

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (!_isStuck)
		{
			var pos = GetMouseWorldPosition();
			_mouseMarker.GlobalPosition = new Vector3(pos.X, pos.Y, 0);
		}
		else
		{
			_mouseMarker.GlobalPosition = _armStuckPosition;
		}
	}

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		base._IntegrateForces(state);

		if (!_isStuck)
		{
			return;
		}

		var target = GetMouseWorldPosition() - _offset;
		target.Z = 0;

		var position = state.Transform.Origin;
		var displacement = target - position;

		var stiffness = 80.0f;
		var damping = 12.0f;

		var force = displacement * stiffness - state.LinearVelocity * damping;
		ApplyCentralForce(force);
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
					if (mouseEvent.Pressed && _canStick)
					{
						var pos = GetMouseWorldPosition();
						_offset = pos - GlobalPosition;
						_armStuckPosition = _mouseMarker.GlobalPosition;
						_isStuck = true;
					}
					else if (mouseEvent.IsReleased())
					{
						_isStuck = false;
					}
					break;
			}
		}
	}

	public void Stick(Node3D node)
	{
		_canStick = true;
	}

	public void UnStick(Node3D node)
	{
		_canStick = false;
	}
}
