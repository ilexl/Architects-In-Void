using Godot;

namespace ArchitectsInVoid.VesselComponent;

public partial class PlaceableComponent : Node3D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// make an overridable function
	protected virtual void Place(Vector3 position, Vector3 placementScale)
	{
		Position = position;
		Transform.Scaled(placementScale);
	}

	protected virtual void Place(Vector3 position)
	{
		Position = position;
	}
}