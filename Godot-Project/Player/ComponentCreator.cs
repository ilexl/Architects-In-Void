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
	private PackedScene _cursorScene;
	[Export] public PackedScene[] Hotbar;

	
	private ComponentPlacerState _state = ComponentPlacerState.Idle;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_cursor = GetNode<Node3D>("../Head/Cursor");
		
		_cursorScene = (PackedScene)ResourceLoader.Load("res://Vessel.tscn");
		_cursorNode = (Node3D)_cursorScene.Instantiate();
		_cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
		
		CallDeferred("add_child", _cursorNode);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_cursorNode.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("place_component"))
		{
			_state = ComponentPlacerState.Placing;
		}
		
		if (Input.IsActionJustReleased("place_component"))
		{
			_state = ComponentPlacerState.Idle;
			Node3D myInstance = (Node3D)_cursorScene.Instantiate();
			GetTree().Root.AddChild(myInstance);
			myInstance.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
		}

		for (int i = 0; i < 10; i++) // Assuming "hotbar_0" to "hotbar_9"
		{
			if (Input.IsActionJustPressed($"hotbar_{i}"))
			{
				// Handle the hotbar action for index i
				GD.Print($"Hotbar {i} pressed");
				_cursorScene = Hotbar[i];
			}
		}
		
	}
}