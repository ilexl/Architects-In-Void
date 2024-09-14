using Godot;

namespace ArchitectsInVoid.VesselComponent;

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

    public override void Place(Vector3 position, Vector3 scale)
    {
        base.Place(position, scale);
    }
}