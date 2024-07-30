using Godot;
using System;
using System.Diagnostics;

public partial class PlayerController : Node3D
{

	// Body
	RigidBody3D body;
	Node3D headPosition;

	// Head
	[Export] Node3D head;
	[Export] Camera3D camera;


	public bool Dampeners = false;
	public bool Jetpack = true;

	// Config
	public const float Acceleration = 25.0f;
	public const float MouseSensitivity = 0.05f;
	public const float RollSensitivity = 50.0f;

	// get gravity from the world
	public Vector3 gravity = ProjectSettings.GetSetting("physics/3d/default_gravity_vector").As<Vector3>() * ProjectSettings.GetSetting("physics/3d/default_gravity").As<float>();
	public override void _Ready()
	{
		
		body = GetNode<RigidBody3D>("Body");
		headPosition = body.GetNode<Node3D>("HeadPosition");

		head = GetNode<Node3D>("Head");
		camera = head.GetNode<Camera3D>("Camera");

		head.Transform = headPosition.Transform;

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{

			head.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity));
			head.RotateObjectLocal(Vector3.Right, Mathf.DegToRad(-mouseEvent.Relative.Y * MouseSensitivity));
		}
	}

	public override void _PhysicsProcess(double delta)
	{

		// Multiply vectors in head space by this to convert to "PlayerOrigin" space
		Basis headTransform = head.Transform.Basis.Inverse();
		Basis bodyTransform = body.Transform.Basis.Inverse();



		Vector3 moveVector = ProcessKeyInput(delta);

		Debug.WriteLine(GetAcceleration(moveVector.Normalized(), headTransform).Length());
		body.LinearVelocity += GetAcceleration(moveVector.Normalized(), headTransform) * (float)delta; // Our moveVector in PlayerOrigin space
		
		//DebugDraw3D.DrawLine(body.Position, body.Position + body.LinearVelocity, Colors.Red, 0.1f);
		// Multiply head position by body transform to get the head position in PlayerOrigin space
		head.Position = body.Position + headPosition.Position * bodyTransform;



		var forwardPlayerSpace = Vector3.Forward * headTransform; // Direction that the body should face in PlayerOrigin space
		var forwardBodySpace = bodyTransform * forwardPlayerSpace; // Direction that the body should face in body space
		var upPlayerSpace = Vector3.Up * headTransform;
		var upBodySpace = bodyTransform * upPlayerSpace;

		var desiredAngularChangeY = -forwardBodySpace.X;
		var desiredAngularChangeX = forwardBodySpace.Y;
		var desiredAngularChangeZ = -upBodySpace.X;

		body.AngularDamp = 0.0f;
		body.AngularVelocity = new Vector3(desiredAngularChangeX, desiredAngularChangeY, desiredAngularChangeZ) * 10.0f * bodyTransform;
	}


	private Vector3 GetAcceleration(Vector3 moveVector, Basis headTransform)
	{
		if (Dampeners)
		{
			Vector3 transformedMoveVector = moveVector * headTransform;
			double gravityLength = gravity.Length();
			Vector3 gravityDir = -(gravity/gravityLength);

			Vector3 counterGravityVector = -gravity;
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



	private Vector3 ProcessKeyInput(double delta)
	{
		float inputRoll = Input.GetAxis("roll_left", "roll_right");
		head.RotateObjectLocal(Vector3.Forward, Mathf.DegToRad(inputRoll * RollSensitivity * (float)delta));
		float inputLeftRight = Input.GetAxis("move_left", "move_right");
		float inputUpDown = Input.GetAxis("move_down", "move_up");
		float inputForwardBackward = Input.GetAxis("move_forward", "move_backward");

		if (Input.IsActionJustPressed("toggle_dampeners"))
		{
			Dampeners = !Dampeners;
		}
		if (Input.IsActionJustPressed("toggle_jetpack"))
		{
			Jetpack = !Jetpack;
		}

		return new Vector3(inputLeftRight, inputUpDown, inputForwardBackward);
	}
}
