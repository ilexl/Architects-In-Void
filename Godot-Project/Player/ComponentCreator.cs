using ArchitectsInVoid.VesselComponent;
using Godot;

namespace ArchitectsInVoid.Player;

internal enum ComponentPlacerState
{
    Idle,
    Placing,
    PlacingAfterHotbarChange
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
    private PackedScene _queuedComponentScene;
    public PackedScene SelectedComponentScene // The property to be read and set by other classes
    {
        get => _selectedComponentScene;
        set
        {
            switch (_state)
            {
                case ComponentPlacerState.Idle:
                    _selectedComponentScene = value;
                    break;
                
                case ComponentPlacerState.Placing: case ComponentPlacerState.PlacingAfterHotbarChange:
                    _queuedComponentScene = value;
                    break;
                
            }
        } 
    }

    private ComponentPlacerState _state = ComponentPlacerState.Idle;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _cursor = GetNode<Node3D>("../Head/Cursor");
        
        _cursorNode = (Node3D)((PackedScene)ResourceLoader.Load("res://Cursor.tscn")).Instantiate();
        _cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;

        CallDeferred("add_child", _cursorNode);

        _root = GetTree().Root;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // _cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
        if (Input.IsActionJustPressed("place_component") && SelectedComponentScene is not null)
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

        if (Input.IsActionJustReleased("place_component") && _state == ComponentPlacerState.Placing)
        {
            _state = ComponentPlacerState.Idle;
            var myInstance = (Node3D)SelectedComponentScene.Instantiate();
            _root.AddChild(myInstance);

            _cursorNode.Scale = Vector3.Zero;

            var placeableComponent = myInstance as PlaceableComponent;

            if (placeableComponent == null) return;

            var position = _cursorStart.Lerp(_cursorEnd, 0.5);
            var scale = _cursorStart - _cursorEnd;
            placeableComponent.Place(position, scale);
        }
    }

}