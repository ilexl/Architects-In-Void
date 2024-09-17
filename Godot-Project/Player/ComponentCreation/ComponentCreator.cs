using System;
using System.Diagnostics;
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
    private double _desiredPlacementDistance = 10.0;
    private double _truncatedPlacementDistance;
    private double _storedPlacementDistance;
    
    private Vector3 _truncatedPlacementPosition;
    
    private Vector3 _cursorStart;
    private Vector3 _cursorEnd;
    private Cursor _cursor;

    private Vessel _targetedVessel;
    
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
        var cursorScene = (PackedScene)ResourceLoader.Load("res://Player/ComponentCreation/Cursor.tscn");
        var cursorObject = cursorScene.Instantiate();
        _cursor = cursorObject as Cursor;
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
                    StartPlace();
                    
                }
                // Set the whole cursor to be at the cursor position
                else
                {
                    Idle();
                }
                
                break;
            // The player has started placing something
            case ComponentPlacerState.Placing:
                PlacingVisuals();
                if (placeAction == 0.0) FinishPlace();
                break;
        }
    }
    
    private void SetPlaceDistance(float value)
    {
        
        
        double change = _placementDistanceSensitivity * value;


       
        
        var spaceState = _head.GetWorld3D().DirectSpaceState;
        
        
        // This overrides the stored distance only if the player changes their distance while placing
        _desiredPlacementDistance = Math.Clamp(_desiredPlacementDistance + change, _minPlacementDistance, _maxPlacementDistance);
        
        if (value != 0) _storedPlacementDistance = _desiredPlacementDistance;
        var query = PhysicsRayQueryParameters3D.Create(_head.Position, CalculateCursorPosition(), 2);
        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            _truncatedPlacementPosition = (Vector3)result["position"];
            _truncatedPlacementDistance = (_truncatedPlacementPosition - _head.Position).Length();
            if (_state == ComponentPlacerState.Idle) _targetedVessel = (Vessel)((RigidBody3D)result["collider"]).GetParent();
            
            
        }
        else
        {
            _truncatedPlacementPosition = Vector3.Zero;
            _truncatedPlacementDistance = _desiredPlacementDistance;
            if (_state == ComponentPlacerState.Idle) _targetedVessel = null;
        }
        
        
    }
    /// <summary>
    /// Handles the visuals of placing
    /// </summary>
    private void PlacingVisuals()
    {
        _cursorEnd = CalculateCursorPosition();
        var position = _cursorStart.Lerp(_cursorEnd, 0.5);
        var scale = _cursorStart - _cursorEnd;
        _cursor.Position = position;
        _cursor.SetCornerPosition(_cursorEnd);
        _cursor.SetScale(scale);
        _cursor.SetLabelVisible(true);
    }

    private void StartPlace()
    {
        _state = ComponentPlacerState.Placing;
        _cursorStart = _truncatedPlacementPosition == Vector3.Zero ? CalculateCursorPosition() : _truncatedPlacementPosition;
        
        // Store the placement distance to stop the node from getting constantly closer to the player
        _storedPlacementDistance = _desiredPlacementDistance;
        // Set the place distance to start at the current truncated distance to make thins more intuitive
        _desiredPlacementDistance = _truncatedPlacementDistance;
    }

    private void Idle()
    {
        // Set the position to the truncated position if it exists, or just go the full distance
        Vector3 position = _truncatedPlacementPosition == Vector3.Zero ? CalculateCursorPosition() : _truncatedPlacementPosition;
        _cursor.Position = position;
        _cursor.SetScale(Vector3.Zero);
        _cursor.SetCornerPosition(position);
    }
    

    /// <summary>
    /// Cleans up everything when finishing placement
    /// </summary>
    private void FinishPlace()
    {
        // Resolve the stored distance
        _desiredPlacementDistance = _storedPlacementDistance;
        _state = ComponentPlacerState.Idle;
        _cursor.SetLabelVisible(false);
        if (_selectedComponent is null) return;
        
        var placeableComponent = _selectedComponent.Instantiate() as PlaceableComponent;
        
        var position = _cursorStart.Lerp(_cursorEnd, 0.5);
        var scale = _cursorStart - _cursorEnd;
        
        
        placeableComponent.Place(position, scale, _targetedVessel);
        
        

    }

    private Vector3 CalculateCursorPosition()
    {
        return _head.Position - _head.Transform.Basis.Z * _desiredPlacementDistance;
    }


    

}