using Godot;

namespace ArchitectsInVoid.Player;

public partial class PlayerController : Node
{
    // Config
    private const float Acceleration = 25.0f;
    private const float MouseSensitivity = 0.05f;
    private const float RollSensitivity = 50.0f;

    // References to player parts
    private RigidBody3D _body;
    private Node3D _head;
    private Node3D _headPosition;
    private Camera3D _camera;

    
    private bool _dampeners = true;
    private bool _jetpack = true;
    private Vector3 _headRelativeRotation = Vector3.Zero;
    
    private Vector3 _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity_vector").As<Vector3>() *
                               ProjectSettings.GetSetting("physics/3d/default_gravity").As<float>();
    
    
    

    
    

    public override void _Ready()
    {
        // Assign our components
        _body = GetNode<RigidBody3D>("Body");
        _head = GetNode<Node3D>("Head");
        _headPosition = _body.GetNode<Node3D>("HeadPosition");
        _camera = _head.GetNode<Camera3D>("Camera");
        
        _head.Transform = _headPosition.Transform;
        
        // This probably shouldn't be here
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


    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        
        // Multiply vectors in head space by this to convert to "PlayerOrigin" space
        var headTransform = _head.Transform.Basis.Inverse();
        var bodyTransform = _body.Transform.Basis.Inverse();

        // Multiply head position by body transform to get the head position in PlayerOrigin space
        _head.Position = _body.Position + _headPosition.Position * bodyTransform;

        // Process our key input
        var moveVector = KeyInputProcess(delta);

        if (_jetpack) JetpackProcess(headTransform, bodyTransform, moveVector, delta);
        else NoJetpackProcess(delta, headTransform, bodyTransform);
    }

    private void JetpackProcess(Basis headTransform, Basis bodyTransform, Vector3 moveVector, double delta)
    {
        var forwardPlayerSpace =
            Vector3.Forward * headTransform; // Direction that the body should face in PlayerOrigin space
        var forwardBodySpace = bodyTransform * forwardPlayerSpace; // Direction that the body should face in body space
        var upPlayerSpace = Vector3.Up * headTransform;
        var upBodySpace = bodyTransform * upPlayerSpace;

        var desiredAngularChangeY = -forwardBodySpace.X;
        var desiredAngularChangeX = forwardBodySpace.Y;
        var desiredAngularChangeZ = -upBodySpace.X;

        _body.AngularDamp = 0.0f;
        _body.AngularVelocity = new Vector3(desiredAngularChangeX, desiredAngularChangeY, desiredAngularChangeZ) *
                                10.0f * bodyTransform;

        _body.LinearVelocity +=
            GetAcceleration(moveVector.Normalized(), headTransform) * delta; // Our moveVector in PlayerOrigin space
    }

    private void NoJetpackProcess(double delta, Basis headTransform, Basis bodyTransform)
    {
        _head.Rotation = _body.Rotation + _headRelativeRotation;
    }

    // This does not function as intended and needs to be rewritten
    private Vector3 GetAcceleration(Vector3 moveVector, Basis headTransform)
    {
        if (_dampeners)
        {
            var transformedMoveVector = moveVector * headTransform;
            var gravityLength = _gravity.Length();
            var gravityDir = -(_gravity / gravityLength);

            var counterGravityVector = -_gravity;
            var combined = counterGravityVector + transformedMoveVector * Acceleration;

            var requiredAcceleration = combined.Length();

            if (requiredAcceleration <= Acceleration) return combined;
            var thing = (gravityDir.Dot(transformedMoveVector) + 1) / 2;

            counterGravityVector *= thing;
            var counterGravityAcceleration = counterGravityVector.Length();
            var remainingAcceleration = Acceleration - counterGravityAcceleration;

            return counterGravityVector + transformedMoveVector * remainingAcceleration;
        }

        return moveVector * Acceleration * headTransform;
    }


    private Vector3 KeyInputProcess(double delta)
    {
        var inputRoll = Input.GetAxis("roll_left", "roll_right");
        _head.RotateObjectLocal(Vector3.Forward, Mathf.DegToRad(inputRoll * RollSensitivity * (float)delta));
        var inputLeftRight = Input.GetAxis("move_left", "move_right");
        var inputUpDown = Input.GetAxis("move_down", "move_up");
        var inputForwardBackward = Input.GetAxis("move_forward", "move_backward");

        if (Input.IsActionJustPressed("toggle_dampeners")) _dampeners = !_dampeners;
        if (Input.IsActionJustPressed("toggle_jetpack")) _jetpack = !_jetpack;


        return new Vector3(inputLeftRight, inputUpDown, inputForwardBackward);
    }
}