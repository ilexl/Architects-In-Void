using ArchitectsInVoid.VesselComponent;
using Godot;

namespace ArchitectsInVoid.Player;

internal enum ComponentPlacerState
{
    Idle,
    Placing
}

public partial class ComponentCreator : Node
{
    [Export] private RigidBody3D _body;
    private Node3D _cursor;
    private Vector3 _cursorEnd;
    private Node3D _cursorNode;

    private Vector3 _cursorStart;
    [Export] private Node3D _head;

    private Window _root;
    private PackedScene _selectedComponentScene;

    private ComponentPlacerState _state = ComponentPlacerState.Idle;
    [Export] public PackedScene[] Hotbar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _cursor = GetNode<Node3D>("../Head/Cursor");

        _selectedComponentScene = (PackedScene)ResourceLoader.Load("res://Vessel.tscn");
        _cursorNode = (Node3D)_selectedComponentScene.Instantiate();
        _cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;

        CallDeferred("add_child", _cursorNode);

        _root = GetTree().Root;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // _cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
        if (Input.IsActionJustPressed("place_component"))
        {
            _state = ComponentPlacerState.Placing;
            _cursorStart = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
        }

        if (_state == ComponentPlacerState.Placing)
        {
            _cursorEnd = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
            var position = _cursorStart.Lerp(_cursorEnd, 0.5);
            var scale = _cursorStart - _cursorEnd;
            _cursorNode.Position = position;
            _cursorNode.Scale = scale;
        }

        if (Input.IsActionJustReleased("place_component"))
        {
            _state = ComponentPlacerState.Idle;
            var myInstance = (Node3D)_selectedComponentScene.Instantiate();
            _root.AddChild(myInstance);

            _cursorNode.Scale = Vector3.Zero;

            var placeableComponent = myInstance as PlaceableComponent;

            if (placeableComponent == null) return;

            var position = _cursorStart.Lerp(_cursorEnd, 0.5);
            var scale = _cursorStart - _cursorEnd;
            placeableComponent.Place(position, scale);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        for (var i = 0; i < 10; i++) // Assuming "hotbar_0" to "hotbar_9"
            if (Input.IsActionJustPressed($"hotbar_{i}"))
                _selectedComponentScene = Hotbar[i];
    }
}