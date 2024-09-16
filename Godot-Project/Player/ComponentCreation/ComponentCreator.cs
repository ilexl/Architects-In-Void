using System;
using ArchitectsInVoid.VesselComponent;
using Godot;

namespace ArchitectsInVoid.Player.ComponentCreation;

internal enum ComponentPlacerState
{
    Idle,
    Placing,
    PlacingAfterHotbarChange
}

public partial class ComponentCreator : Node
{
    [Export] private double _placementDistanceSensitivity = 1.0;
    [Export] private double _minPlacementDistance = 1;
    [Export] private double _maxPlacementDistance = 30;
    [Export] private RigidBody3D _body;
    private Node3D _cursorPosition;
    private double _placementDistance = 10.0;
    private Vector3 _cursorEnd;
    private Cursor _cursor;

    private Vector3 _cursorStart;
    [Export] private Node3D _head;

    private Window _root;
    
    private PackedScene _selectedComponent;
    public PackedScene SelectedComponent // The property to be read and set by other classes
    {
        get => _selectedComponent;
        set
        {
            switch (_state)
            {
                case ComponentPlacerState.Idle:
                    _selectedComponent = value;
                    _cursor.SetLabelName(value);
                    break;
                
                case ComponentPlacerState.Placing: case ComponentPlacerState.PlacingAfterHotbarChange:
                    _state = ComponentPlacerState.PlacingAfterHotbarChange;
                    _selectedComponent = value;
                    _cursor.SetLabelName(value);
                    break;
                
            }
        } 
    }

    private ComponentPlacerState _state = ComponentPlacerState.Idle;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _cursorPosition = GetNode<Node3D>("../Head/Cursor");

        var cursorScene = (PackedScene)ResourceLoader.Load("res://Player/ComponentCreation/Cursor.tscn");
        var cursorObject = cursorScene.Instantiate();
        _cursor = cursorObject as Cursor;
        
        _cursor.Position = _cursorPosition.Position * _head.Transform.Basis.Inverse() + _head.Position;

        CallDeferred("add_child", _cursor);

        _root = GetTree().Root;
    }
    
    public override void _Process(double delta)
    {
        bool placeActionPressed = Input.IsActionJustPressed("place_component" );
        bool placeActionReleased = Input.IsActionJustReleased("place_component" );
        bool increaseDistance = Input.IsActionJustPressed("increase_placement_distance");
        bool decreaseDistance = Input.IsActionJustPressed("decrease_placement_distance");

        if (SelectedComponent is not null)
        {
            _cursor.Visible = true;
            SetPlaceDistance(increaseDistance, decreaseDistance);
        }
        else
        {
            _cursor.Visible = false;
        }
        
        
        
        switch (_state)
        {
            case ComponentPlacerState.Idle:
                if (placeActionPressed && SelectedComponent is not null)
                {
                    _state = ComponentPlacerState.Placing;
                    DebugLog($"state changed to { _state }");
                    DebugLog($"placing component { SelectedComponent.ResourcePath }");
                    _cursorStart = _cursorPosition.Position * _head.Transform.Basis.Inverse() + _head.Position - _head.Transform.Basis.Z * _placementDistance;
                    
                }
                else
                {
                    Vector3 position = _cursorPosition.Position * _head.Transform.Basis.Inverse() + _head.Position -
                                       _head.Transform.Basis.Z * _placementDistance;
                    _cursor.Position = position;
                    _cursor.SetScale(Vector3.Zero);
                    _cursor.SetCornerPosition(position);
                }

                
                break;
            
            case ComponentPlacerState.Placing:
                CursorVisuals();
                
                if (placeActionReleased) FinishPlace();
                break;
            
            case ComponentPlacerState.PlacingAfterHotbarChange:
                
                CursorVisuals();
                if (placeActionReleased)
                {
                    FinishPlace();
                    
                }
                break;
        }
    }

    private void SetPlaceDistance(bool increase, bool decrease)
    {
        
        if (increase)
        {
            _placementDistance = Math.Min(_placementDistance + _placementDistanceSensitivity, _maxPlacementDistance);
        }

        if (decrease)
        {
            _placementDistance = Math.Max(_placementDistance - _placementDistanceSensitivity, _minPlacementDistance);
        }
    }
    
    private void CursorVisuals()
    {
        _cursorEnd = _cursorPosition.Position * _head.Transform.Basis.Inverse() + _head.Position - _head.Transform.Basis.Z * _placementDistance;
        var position = _cursorStart.Lerp(_cursorEnd, 0.5);
        var scale = _cursorStart - _cursorEnd;
        _cursor.Position = position;
        _cursor.SetCornerPosition(_cursorEnd);
        _cursor.SetScale(scale);
        _cursor.SetLabelVisible(true);
    }

    private void FinishPlace()
    {
       
        _state = ComponentPlacerState.Idle;
        _cursor.SetLabelVisible(false);
        DebugLog($"state changed to { _state }");
        if (_selectedComponent is null) return;
        
        var placeableComponent = _selectedComponent.Instantiate() as PlaceableComponent;
        _root.AddChild(placeableComponent);
        
        var position = _cursorStart.Lerp(_cursorEnd, 0.5);
        var scale = _cursorStart - _cursorEnd;
        placeableComponent.Place(position, scale);
        
    }

    [Export] private bool _debug;
    private void DebugLog(object message)
    {
        if (!_debug) return;
        GD.Print(message);
    }
    

}