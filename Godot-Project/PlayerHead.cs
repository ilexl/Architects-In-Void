using Godot;

namespace ArchitectsInVoid;

public partial class PlayerHead : Node3D
{

	[Export] public Node3D Cursor;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}