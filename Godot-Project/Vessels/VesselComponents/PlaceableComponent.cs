using Godot;

namespace ArchitectsInVoid.VesselComponent;

public enum PlaceableComponentType
{
    FixedScale,
    DynamicScale
}

public enum PlaceableComponentResult
{
    Success,
    ErrorAddToVessel,
    ErrorCreateNewVessel,
    ErrorPositionOrScale,
}
/// <summary>
/// Base class for all objects that can be attached to vessels.
/// </summary>
public partial class PlaceableComponent : Node3D
{
    protected virtual PlaceableComponentType ComponentType { get; set; }
    
    [Export] protected double Density;
    [Export] protected CollisionShape3D Shape;

    
    
    
    
    
    
    /***************NEW VESSEL***************/
    #region NewVessel
    // Scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Vector3 scale, PackedScene vesselPrefab)
    {
        Position = position;
        Scale = scale;
        return AddToNewVessel(position, vesselPrefab);
    }
    
    // Not scaled
    public virtual PlaceableComponentResult Place(Vector3 position, PackedScene vesselPrefab)
    {
        Position = position;
        return AddToNewVessel(position, vesselPrefab);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="vesselPrefab"></param>
    /// <returns></returns>
    protected PlaceableComponentResult AddToNewVessel(Vector3 position, PackedScene vesselPrefab)
    {
        var vessel = vesselPrefab.Instantiate() as Vessel;
        if (vessel == null) return PlaceableComponentResult.ErrorCreateNewVessel;
        vessel.Position = position;
        
        return PlaceableComponentResult.Success;
    }
    #endregion
    /*************EXISTING VESSEL*************/
    #region ExistingVessel
    // Scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Vector3 scale, Vessel vessel)
    {
        Position = position;
        Scale = scale;
        
        return AddToVessel(vessel);
    }

    // Not scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Vessel vessel)
    {
        Position = position;
        
        return AddToVessel(vessel);
    }

    protected PlaceableComponentResult AddToVessel(Vessel vessel)
    {
        var rigidBody = vessel.RigidBody;
        Node componentRoot = vessel.ComponentRoot;
        
        vessel.AddChild(Shape);
        
        componentRoot.AddChild(this);
        return PlaceableComponentResult.Success;
    }
    #endregion
}