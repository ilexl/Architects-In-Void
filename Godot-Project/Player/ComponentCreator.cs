using ArchitectsInVoid.VesselComponent;
using Godot;

namespace ArchitectsInVoid.Player;


enum ComponentPlacerState
{
	Idle,
	Placing
}
public partial class ComponentCreator : Node
{
	[Export] private RigidBody3D _body;
	[Export] private Node3D _head;
	private Node3D _cursor;
	private Node3D _cursorNode;
	private PackedScene _SelectedComponentScene;
	[Export] public PackedScene[] Hotbar;

	private Vector3 _cursorStart;
	private Vector3 _cursorEnd;
	
	private ComponentPlacerState _state = ComponentPlacerState.Idle;

	private Window _root;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_cursor = GetNode<Node3D>("../Head/Cursor");
		
		_SelectedComponentScene = (PackedScene)ResourceLoader.Load("res://Vessel.tscn");
		_cursorNode = (Node3D)_SelectedComponentScene.Instantiate();
		_cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
		
		CallDeferred("add_child", _cursorNode);

		_root = GetTree().Root;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("place_component"))
		{
			_state = ComponentPlacerState.Placing;
			_cursorStart = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
		}

		if (_state == ComponentPlacerState.Placing)
		{
			_cursorEnd = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
		}
		
		if (Input.IsActionJustReleased("place_component"))
		{
			_state = ComponentPlacerState.Idle;
			Node3D myInstance = (Node3D)_SelectedComponentScene.Instantiate();
			_root.AddChild(myInstance);
			

			PlaceableComponent placeableComponent = myInstance as PlaceableComponent;
			if (placeableComponent == null) return;
			
			Vector3 position = _cursorStart.Lerp(_cursorEnd, 0.5);
			Vector3 scale = _cursorStart - _cursorEnd;
			placeableComponent.Place(position, scale);
			
		}

		for (int i = 0; i < 10; i++) // Assuming "hotbar_0" to "hotbar_9"
		{
			if (Input.IsActionJustPressed($"hotbar_{i}"))
			{
				_SelectedComponentScene = Hotbar[i];
			}
		}
		
	}
}