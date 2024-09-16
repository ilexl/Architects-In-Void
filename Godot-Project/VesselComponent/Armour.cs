using Godot;

namespace ArchitectsInVoid.VesselComponent;

/// <summary>
/// Scalable basic box to represent armour. Should be replaced with voxels at a future time.
/// </summary>
public partial class Armour : PlaceableComponent
{
    public override void _Ready()
    {
        ComponentType = PlaceableComponentType.DynamicScale;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}