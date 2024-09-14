using Godot;

namespace ArchitectsInVoid.Player;

public partial class ComponentCreator : Node
{
	private RigidBody3D _body;
	[Export] private Node3D _head;
	[Export] private Node3D _cursor;
	private Node3D _cursorNode;
	
	private PackedScene _cursorScene;
	[Export] public PackedScene[] Hotbar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_body = GetNode<RigidBody3D>("../Body");
		_head = GetNode<Node3D>("../Head");
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
		if (Input.IsActionJustReleased("place_component"))
		{
			Node3D myInstance = (Node3D)_cursorScene.Instantiate();
			GetTree().Root.AddChild(myInstance);
			myInstance.Position = _cursor.Position * _head.Transform.Basis.Inverse() + _head.Position;
		}
	}
}