using ArchitectsInVoid.WorldData;
using Godot;

namespace ArchitectsInVoid;
/// <summary>
/// A class for containing information on a vessel and all PlaceableComponents attached to it.
/// </summary>
public partial class Vessel : Node
{
    [Export] public RigidBody3D RigidBody;
    [Export] public Node ComponentData;
    public override void _Ready()
    {
    }
    
}