using ArchitectsInVoid.WorldData;
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
    public virtual PlaceableComponentResult Place(Vector3 position, Vector3 scale)
    {
        Scale = scale;
        return AddToNewVessel(position);
    }
    
    // Not scaled
    public virtual PlaceableComponentResult Place(Vector3 position)
    {
        return AddToNewVessel(position);
    }

    protected PlaceableComponentResult AddToNewVessel(Vector3 position)
    {
        var vessel = VesselData._VesselData.CreateVessel(position);
        if (vessel == null) return PlaceableComponentResult.ErrorCreateNewVessel;
        
        
        var rigidBody = vessel.RigidBody;
        var componentData = vessel.ComponentData;
        rigidBody.AddChild(this);
        Shape.Reparent(rigidBody, false);
        Shape.Scale = Scale;
        rigidBody.Mass += Density * Scale.LengthSquared();
        
        return PlaceableComponentResult.Success;
        
    }
    #endregion
    /*************EXISTING VESSEL*************/
    #region ExistingVessel
    // Scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Vector3 scale, Vessel vessel)
    {
        if (vessel == null)
        {
            GD.Print("Making new vessel");
            return Place(position, scale);
        }
        Scale = scale;

        GD.Print("Adding to existing vessel");
        return AddToVessel(vessel, position);
    }

    // Not scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Vessel vessel)
    {
        Position = position;
        
        return AddToVessel(vessel, position);
    }

    protected PlaceableComponentResult AddToVessel(Vessel vessel, Vector3 position)
    {
        var rigidBody = vessel.RigidBody;
        var componentData = vessel.ComponentData;
        rigidBody.AddChild(this);
        Shape.Reparent(rigidBody, false);
        Position = position - rigidBody.Position;
        Shape.Position = Position;
        Shape.Scale = Scale;
        rigidBody.Mass += Density * Scale.LengthSquared();
        
        return PlaceableComponentResult.Success;
    }
    #endregion
}