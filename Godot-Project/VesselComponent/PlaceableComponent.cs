using Godot;

namespace ArchitectsInVoid.VesselComponent;


public enum PlaceableComponentType
{
	FixedScale,
	DynamicScale
}
public partial class PlaceableComponent : Node3D
{
	protected virtual PlaceableComponentType ComponentType { get; set; }
	
	public virtual void Place(Vector3 position, Vector3 scale)
	{
		Position = position;
		Scale = scale;
	}

	public virtual void Place(Vector3 position)
	{
		Position = position;
	}
}