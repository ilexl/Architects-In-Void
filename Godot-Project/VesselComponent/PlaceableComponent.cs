using Godot;

namespace ArchitectsInVoid.VesselComponent;


public enum PlaceableComponentType
{
	FixedScale,
	DynamicScale
}
public partial class PlaceableComponent : Node3D
{
	protected virtual PlaceableComponentType Type => PlaceableComponentType.FixedScale;
	
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