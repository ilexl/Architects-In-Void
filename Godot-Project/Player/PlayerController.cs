using ArchitectsInVoid.Debug;
using ArchitectsInVoid.Debug.Meshes;
using Godot;

namespace ArchitectsInVoid.Player;

public partial class PlayerController : Node
{
    // Config
    private const float Acceleration = 25.0f;
    private const float MouseSensitivity = 0.05f;
    private const float RollSensitivity = 50.0f;

    // References to player parts
    public RigidBody3D Body;
    public Node3D Head;
    private Node3D _headPosition;
    private Camera3D _camera;

    
    public bool Dampeners = true;
    public bool Jetpack = true;
    private Vector3 _headRelativeRotation = Vector3.Zero;
    
    private Vector3 _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity_vector").As<Vector3>() *
                               ProjectSettings.GetSetting("physics/3d/default_gravity").As<float>();
    
    
    

    
    

    public override void _Ready()
    {
        DebugDraw.Line(Vector3.Zero, Vector3.Up * 100, Colors.Aqua, 10, 2, type:DebugMesh.Type.Wireframe);
        DebugDraw.Circle(Vector3.Right * 100, Colors.Green, 10, 10, type:DebugMesh.Type.Solid);
        // Assign our components
        Body = GetNode<RigidBody3D>("Body");
        Head = GetNode<Node3D>("Head");
        _headPosition = Body.GetNode<Node3D>("HeadPosition");
        _camera = Head.GetNode<Camera3D>("Camera");
        
        Head.Transform = _headPosition.Transform;
        
        // This probably shouldn't be here
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }


    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent)
        {
            Head.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity));
            Head.RotateObjectLocal(Vector3.Right, Mathf.DegToRad(-mouseEvent.Relative.Y * MouseSensitivity));
            _headRelativeRotation = Head.Rotation - Body.Rotation;
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        
        if (Input.IsKeyPressed(Key.Escape))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        
        // Multiply vectors in head space by this to convert to "PlayerOrigin" space
        var headTransform = Head.Transform.Basis.Inverse();
        var bodyTransform = Body.Transform.Basis.Inverse();

        // Multiply head position by body transform to get the head position in PlayerOrigin space
        Head.Position = Body.Position + _headPosition.Position * bodyTransform;

        // Process our key input
        var moveVector = KeyInputProcess(delta);

        if (Jetpack) JetpackProcess(headTransform, bodyTransform, moveVector, delta);
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

        Body.AngularDamp = 0.0f;
        Body.AngularVelocity = new Vector3(desiredAngularChangeX, desiredAngularChangeY, desiredAngularChangeZ) *
                                10.0f * bodyTransform;

        Body.LinearVelocity +=
            GetAcceleration(moveVector.Normalized(), headTransform) * delta; // Our moveVector in PlayerOrigin space
    }

    private void NoJetpackProcess(double delta, Basis headTransform, Basis bodyTransform)
    {
        Head.Rotation = Body.Rotation + _headRelativeRotation;
    }

    // This does not function as intended and needs to be rewritten
    private Vector3 GetAcceleration(Vector3 moveVector, Basis headTransform)
    {
        if (Dampeners)
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
        Head.RotateObjectLocal(Vector3.Forward, Mathf.DegToRad(inputRoll * RollSensitivity * (float)delta));
        var inputLeftRight = Input.GetAxis("move_left", "move_right");
        var inputUpDown = Input.GetAxis("move_down", "move_up");
        var inputForwardBackward = Input.GetAxis("move_forward", "move_backward");

        if (Input.IsActionJustPressed("toggle_dampeners")) Dampeners = !Dampeners;
        if (Input.IsActionJustPressed("toggle_jetpack")) Jetpack = !Jetpack;


        return new Vector3(inputLeftRight, inputUpDown, inputForwardBackward);
    }
}