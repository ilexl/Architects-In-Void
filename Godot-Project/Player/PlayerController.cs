using Godot;

namespace ArchitectsInVoid.Player;

public partial class PlayerController : Node3D
{

	// Body
	private RigidBody3D _body;
	private Node3D _headPosition;

	// Head
	[Export] private Node3D _head;
	[Export] private Camera3D _camera;
	[Export] private Node3D _cursor;
	private Node3D _cursorNode;


	private bool _dampeners = false;
	private bool _jetpack = true;

	// Config
	private const float Acceleration = 25.0f;
	private const float MouseSensitivity = 0.05f;
	private const float RollSensitivity = 50.0f;

	// get gravity from the world
	private Vector3 _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity_vector").As<Vector3>() * ProjectSettings.GetSetting("physics/3d/default_gravity").As<float>();
	public override void _Ready()
	{
		_body = GetNode<RigidBody3D>("Body");
		_headPosition = _body.GetNode<Node3D>("HeadPosition");

		_head = GetNode<Node3D>("Head");
		_cursor = GetNode<Node3D>("Head/Cursor");
		_camera = _head.GetNode<Camera3D>("Camera");

		_head.Transform = _headPosition.Transform;

		
		PackedScene myPackedScene = (PackedScene)ResourceLoader.Load("res://Vessel.tscn");
		_cursorNode = (Node3D)myPackedScene.Instantiate();
		GetTree().Root.AddChild(_cursorNode);
		_cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
		
		CallDeferred("add_child", _cursorNode);
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{

			_head.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity));
			_head.RotateObjectLocal(Vector3.Right, Mathf.DegToRad(-mouseEvent.Relative.Y * MouseSensitivity));
			_headRelativeRotation = _head.Rotation - _body.Rotation;

		}
	}

	public override void _Process(double delta)
	{
		_cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
	}

	public override void _PhysicsProcess(double delta)
	{

		// Multiply vectors in head space by this to convert to "PlayerOrigin" space
		Basis headTransform = _head.Transform.Basis.Inverse();
		Basis bodyTransform = _body.Transform.Basis.Inverse();

		// Multiply head position by body transform to get the head position in PlayerOrigin space
		_head.Position = _body.Position + _headPosition.Position * bodyTransform;

		// Process our key input
		Vector3 moveVector = KeyInputProcess(delta);
		
		if (_jetpack) JetpackProcess(headTransform, bodyTransform, moveVector, delta);
		else NoJetpackProcess(delta, headTransform, bodyTransform);
		
	}

	private void JetpackProcess(Basis headTransform, Basis bodyTransform, Vector3 moveVector, double delta)
	{
		var forwardPlayerSpace = Vector3.Forward * headTransform; // Direction that the body should face in PlayerOrigin space
		var forwardBodySpace = bodyTransform * forwardPlayerSpace; // Direction that the body should face in body space
		var upPlayerSpace = Vector3.Up * headTransform;
		var upBodySpace = bodyTransform * upPlayerSpace;

		var desiredAngularChangeY = -forwardBodySpace.X;
		var desiredAngularChangeX = forwardBodySpace.Y;
		var desiredAngularChangeZ = -upBodySpace.X;

		_body.AngularDamp = 0.0f;
		_body.AngularVelocity = new Vector3(desiredAngularChangeX, desiredAngularChangeY, desiredAngularChangeZ) * 10.0f * bodyTransform;

		_body.LinearVelocity += GetAcceleration(moveVector.Normalized(), headTransform) * delta; // Our moveVector in PlayerOrigin space
	}

	Vector3 _headRelativeRotation = Vector3.Zero;
	private void NoJetpackProcess(double delta, Basis headTransform, Basis bodyTransform)
	{



		_head.Rotation = _body.Rotation + _headRelativeRotation;
	}

	private Vector3 GetAcceleration(Vector3 moveVector, Basis headTransform)
	{
		if (_dampeners)
		{
			Vector3 transformedMoveVector = moveVector * headTransform;
			double gravityLength = _gravity.Length();
			Vector3 gravityDir = -(_gravity/gravityLength);

			Vector3 counterGravityVector = -_gravity;
			Vector3 combined = counterGravityVector + (transformedMoveVector * Acceleration);

			var requiredAcceleration = combined.Length();

			if (requiredAcceleration <= Acceleration) return combined;
			var thing = ((gravityDir.Dot(transformedMoveVector) + 1)) / 2;
			
			counterGravityVector *= thing;
			var counterGravityAcceleration = counterGravityVector.Length();
			var remainingAcceleration = Acceleration - counterGravityAcceleration;

			return (counterGravityVector + (transformedMoveVector * remainingAcceleration));

		}
		else
		{
			return (moveVector * Acceleration * headTransform);
		}
		
	}



	private Vector3 KeyInputProcess(double delta)
	{
		float inputRoll = Input.GetAxis("roll_left", "roll_right");
		_head.RotateObjectLocal(Vector3.Forward, Mathf.DegToRad(inputRoll * RollSensitivity * (float)delta));
		float inputLeftRight = Input.GetAxis("move_left", "move_right");
		float inputUpDown = Input.GetAxis("move_down", "move_up");
		float inputForwardBackward = Input.GetAxis("move_forward", "move_backward");

		if (Input.IsActionJustPressed("toggle_dampeners"))
		{
			_dampeners = !_dampeners;
		}
		if (Input.IsActionJustPressed("toggle_jetpack"))
		{
			_jetpack = !_jetpack;
		}

		if (Input.IsActionJustReleased("place_component"))
		{
			PackedScene myPackedScene = (PackedScene)ResourceLoader.Load("res://Vessel.tscn");
			Node3D myInstance = (Node3D)myPackedScene.Instantiate();
			GetTree().Root.AddChild(myInstance);
			myInstance.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
		}
		
		return new Vector3(inputLeftRight, inputUpDown, inputForwardBackward);
	}
}