using System.IO;
using ArchitectsInVoid.Helper;
using ArchitectsInVoid.WorldData;
using Godot;
using FileAccess = Godot.FileAccess;

using sf = Godot.ResourceSaver.SaverFlags;

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
    public virtual PlaceableComponentType ComponentType { get; set; }
    
    [Export] protected double Density;
    
    [Export] public Image Thumbnail;

    [Export] private string _thumbnailName = "thumb.res";


    public Vessel Vessel;
    private readonly sf _flags =
        sf.ReplaceSubresourcePaths & 
        sf.Compress;
    
    [Export] public Texture2D Thumbnail;

    #region Component Selection Data

    public enum Category
    {
        Other,
        Armour,
        Controls,
        Movement,
        Power,
        Storage,
        Utility,
        Defense,
        Production,
        Temperature,
        Doors,
        Aesthetics
    }

    [ExportGroup("Component Selection Data")]
    [Export] public string _cmpSlcData_Title;
    [Export] public string _cmpSlcData_Desc;
    [Export] public Category _category;
    [ExportSubgroup("Extra Properties")]
    [Export] public string _cmpSlcData_infoTitle0;
    [Export] public string _cmpSlcData_infoDesc0;
    [Export] public string _cmpSlcData_infoTitle1;
    [Export] public string _cmpSlcData_infoDesc1;
    [Export] public string _cmpSlcData_infoTitle2;
    [Export] public string _cmpSlcData_infoDesc2;
    [Export] public string _cmpSlcData_infoTitle3;
    [Export] public string _cmpSlcData_infoDesc3;
    [Export] public string _cmpSlcData_infoTitle4;
    [Export] public string _cmpSlcData_infoDesc4;
    [Export] public string _cmpSlcData_infoTitle5;
    [Export] public string _cmpSlcData_infoDesc5;
    [Export] public string _cmpSlcData_infoTitle6;
    [Export] public string _cmpSlcData_infoDesc6;
    [Export] public string _cmpSlcData_infoTitle7;
    [Export] public string _cmpSlcData_infoDesc7;
    [Export] public string _cmpSlcData_infoTitle8;
    [Export] public string _cmpSlcData_infoDesc8;
    [Export] public string _cmpSlcData_infoTitle9;
    [Export] public string _cmpSlcData_infoDesc9;

    #endregion


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
        Vessel = vessel;
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
        //vessel.AddComponent(this);

        var vesselRb = vessel.RigidBody;
        //var componentData = vessel.ComponentData;
        Transform = Transform with { Basis =  vesselRb.Transform.Basis.Inverse() * rotation };
        vesselRb.AddChild(this);
        Position = position * vesselRb.Transform;
        
        Scale = scale;
        vesselRb.Mass += Density * Scale.LengthSquared();
        Vessel = vessel;
        return PlaceableComponentResult.Success;
    }
    #endregion

    public override void _Process(double delta)
    {
        PackedScene scene = new PackedScene();
        // scene

    }


    public void RecieveThumbnail(string path, Texture2D preview, Texture2D thumb, Variant userData)
    {
        
        // Gets the actual raw image type which means it can be converted to binary
        var newThumbnail = preview.GetImage();
        // Gets the containing folder of the component scene
        path = SceneUtility.GetSceneDirectory(path)  + "/" + _thumbnailName;
        GD.Print(path);
        if (Thumbnail != null)
        {
            Thumbnail.ResourcePath = null;
        }
        Thumbnail = null;
        ResourceSaver.Save(newThumbnail, path, _flags);
        newThumbnail.ResourcePath = path;
        Thumbnail = newThumbnail;
    }


    public override void _Notification(int what)
    {
        if (what == NotificationEditorPostSave)
        {
            GenerateThumbnail();
        }
    }
    protected virtual void GenerateThumbnail()
    {
        if (Engine.IsEditorHint())
        {
            scenepreviewextractor.GetPreview(SceneFilePath, this, "RecieveThumbnail", 0);
        }
    }
}