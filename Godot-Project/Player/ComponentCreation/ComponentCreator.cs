using System;
using ArchitectsInVoid.VesselComponent;
using Godot;

namespace ArchitectsInVoid.Player.ComponentCreation;

// Keeps track of the current state of the component placing
internal enum ComponentPlacerState
{
    Idle,
    Placing,
}

public partial class ComponentCreator : Node
{
    // Configuration
    [Export] private double _placementDistanceSensitivity = 1.0;
    [Export] private double _minPlacementDistance = 1;
    [Export] private double _maxPlacementDistance = 30;
    
    [Export] private Node3D _head;
    [Export] private RigidBody3D _body;
    
    
    // Placement cursor
    private double _placementDistance = 10.0;
    
    private Node3D _cursorPosition;
    private Vector3 _cursorStart;
    private Vector3 _cursorEnd;
    private Cursor _cursor;

    
    private Window _root;
    
    
    // Component selection
    private PackedScene _selectedComponent;
    public PackedScene SelectedComponent // Useful for setting the label in addition to the value
    {
        get => _selectedComponent;
        set
        {
            _selectedComponent = value;
            _cursor.SetLabelName(value);
        } 
    }

    private ComponentPlacerState _state = ComponentPlacerState.Idle;
    
    
    public override void _Ready()
    {
        
        // Cursor
        _cursorPosition = GetNode<Node3D>("../Head/Cursor");
        var cursorScene = (PackedScene)ResourceLoader.Load("res://Player/ComponentCreation/Cursor.tscn");
        var cursorObject = cursorScene.Instantiate();
        _cursor = cursorObject as Cursor;
        _cursor.Position = _cursorPosition.Position * _head.Transform.Basis.Inverse() + _head.Position;
        CallDeferred("add_child", _cursor);

        _root = GetTree().Root;
    }

    

    public override void _PhysicsProcess(double delta)
    {
        // Store if player inputs have been taken this frame
        // This should probably be in PhysicsProcess
        float placeAction = Input.GetActionStrength("place_component");
        float placementDistanceControl = (Input.IsActionJustPressed("increase_placement_distance") ? 1 : 0) + (Input.IsActionJustPressed("decrease_placement_distance") ? -1 : 0);
        
        // Show the cursor and handle placement distance if the player has a component selected on their hotbar
        if (SelectedComponent is not null)
        {
            _cursor.Visible = true; 
            SetPlaceDistance(placementDistanceControl);
        }
        else
        {
            _cursor.Visible = false;
        }
        
        
        switch (_state)
        {
            // The player isn't already placing something
            case ComponentPlacerState.Idle:
                // Set the starting position of the cursor and move on to the next state if the player has depressed the place button
                if (placeAction == 1.0 && SelectedComponent is not null)
                {
                    _state = ComponentPlacerState.Placing;
                    _cursorStart = _cursorPosition.Position * _head.Transform.Basis.Inverse() + _head.Position - _head.Transform.Basis.Z * _placementDistance;
                    
                }
                // Set the whole cursor to be at the cursor position
                else
                {
                    Vector3 position = _cursorPosition.Position * _head.Transform.Basis.Inverse() + _head.Position -
                                       _head.Transform.Basis.Z * _placementDistance;
                    _cursor.Position = position;
                    _cursor.SetScale(Vector3.Zero);
                    _cursor.SetCornerPosition(position);
                }
                
                break;
            // The player has started placing something
            case ComponentPlacerState.Placing:
                PlacingVisuals();
                if (placeAction == 0.0) FinishPlace();
                break;
        }
    }

    /// <summary>
    /// Changes the place distance by a set value and clamps it
    /// </summary>
    /// <param name="value"></param>
    private void SetPlaceDistance(float value)
    {
        double change = _placementDistanceSensitivity * value;
        
        _placementDistance = Math.Clamp(_placementDistance + change, _minPlacementDistance, _maxPlacementDistance);
    }
    /// <summary>
    /// Handles the visuals of placing
    /// </summary>
    private void PlacingVisuals()
    {
        _cursorEnd = _cursorPosition.Position * _head.Transform.Basis.Inverse() + _head.Position - _head.Transform.Basis.Z * _placementDistance;
        var position = _cursorStart.Lerp(_cursorEnd, 0.5);
        var scale = _cursorStart - _cursorEnd;
        _cursor.Position = position;
        _cursor.SetCornerPosition(_cursorEnd);
        _cursor.SetScale(scale);
        _cursor.SetLabelVisible(true);
    }

    /// <summary>
    /// Cleans up everything when finishing placement
    /// </summary>
    private void FinishPlace()
    {
       
        _state = ComponentPlacerState.Idle;
        _cursor.SetLabelVisible(false);
        if (_selectedComponent is null) return;
        
        var placeableComponent = _selectedComponent.Instantiate() as PlaceableComponent;
        _root.AddChild(placeableComponent);
        
        var position = _cursorStart.Lerp(_cursorEnd, 0.5);
        var scale = _cursorStart - _cursorEnd;
        /*placeableComponent.Place(position, scale);*/
        
    }


    

}