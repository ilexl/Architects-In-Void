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
[Tool]
public partial class PlaceableComponent : CollisionShape3D
{
    protected virtual PlaceableComponentType ComponentType { get; set; }
    
    [Export] protected double Density;

    [Export]
    public bool GenerateThumb
    {
        get => false;
        set
        {
            if (value)
            {
                GenerateThumbnail();
            }
        }
    }
    [Export] public Texture2D Thumbnail;
    
    
    
    
    
    /***************NEW VESSEL***************/
    #region NewVessel
    // Scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Vector3 scale, Basis rotation)
    {
        Scale = scale;
        return AddToNewVessel(position, rotation);
    }
    
    // Not scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Basis rotation)
    {
        return AddToNewVessel(position, rotation);
    }

    protected PlaceableComponentResult AddToNewVessel(Vector3 position, Basis rotation)
    {
        var vessel = VesselData._VesselData.CreateVessel(position);
        if (vessel == null) return PlaceableComponentResult.ErrorCreateNewVessel;
        
        
        var vesselRB = vessel.RigidBody;
        var componentData = vessel.ComponentData;
        vesselRB.AddChild(this);
        vesselRB.Mass += Density * Scale.LengthSquared();
        vesselRB.Transform = vesselRB.Transform with { Basis = rotation };
        return PlaceableComponentResult.Success;
        
    }
    #endregion
    /*************EXISTING VESSEL*************/
    #region ExistingVessel
    // Scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Vector3 scale, Basis rotation, Vessel vessel)
    {
        if (vessel == null)
        {
            GD.Print("Making new vessel");
            return Place(position, scale, rotation);
        }
        GD.Print("Adding to existing vessel");
        return AddToVessel(vessel, position, scale, rotation);
    }

    // Not scaled
    public virtual PlaceableComponentResult Place(Vector3 position, Vessel vessel, Basis rotation)
    {
        return AddToVessel(vessel, position, Vector3.One, rotation);
    }

    protected PlaceableComponentResult AddToVessel(Vessel vessel, Vector3 position, Vector3 scale, Basis rotation)
    {
        
        var vesselRb = vessel.RigidBody;
        var componentData = vessel.ComponentData;
        Transform = Transform with { Basis =  vesselRb.Transform.Basis.Inverse() * rotation };
        vesselRb.AddChild(this);
        Position = position * vesselRb.Transform;
        
        Scale = scale;
        vesselRb.Mass += Density * Scale.LengthSquared();
        
        return PlaceableComponentResult.Success;
    }
    #endregion

    public override void _Process(double delta)
    {
        PackedScene scene = new PackedScene();
        // scene

    }

    protected virtual void GenerateThumbnail()
    {
        if (Engine.IsEditorHint())
        {
            //scenepreviewextractor.GetPreview(SceneFilePath, this, "RecieveThumbnail", 0);
        }
    }
    public void RecieveThumbnail(string path, Texture2D preview, Texture2D thumb, Variant userData)
    {
        Thumbnail = preview;
    }

    public override void _EnterTree()
    {
        if (Engine.IsEditorHint())
        {
            GD.Print("Loaded");

        }
        
    }
}