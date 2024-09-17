using Godot;

namespace ArchitectsInVoid;
/// <summary>
/// A class for containing information on a vessel and all PlaceableComponents attached to it.
/// </summary>
public partial class Vessel : Node3D
{
    [Export] public RigidBody3D RigidBody;
    [Export] public Node ComponentRoot;
    public override void _Ready()
    {
    }
    
}