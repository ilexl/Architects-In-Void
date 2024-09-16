using Godot;

namespace ArchitectsInVoid.VesselComponent;

public enum PlaceableComponentType
{
    FixedScale,
    DynamicScale
}
/// <summary>
/// Base class for all objects that can be attached to vessels.
/// </summary>
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