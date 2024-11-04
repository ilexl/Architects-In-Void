using ArchitectsInVoid.Debug;
using ArchitectsInVoid.Interactables;
using ArchitectsInVoid.Player;
using Godot;

namespace ArchitectsInVoid.Vessels.VesselComponents.Cockpit;

public partial class CockpitBehavior : Node
{
    [Export] private InteractableObject _interactor;
    [Export] private Cockpit _cockpit;

    public override void _Ready()
    {
        _interactor.Interacted += BreakInteraction;
    }
    
    private void BreakInteraction(PlayerController player, InteractableObject.InteractionType type)
    {
        var position = _cockpit.GlobalPosition;
        var basis = _cockpit.Transform.Basis;
        var globalBasis = _cockpit.GlobalTransform.Basis;
        
        var halfScale =  globalBasis.Orthonormalized() * (basis.Scale / 2) ;
        
        switch (type)
        {
            case InteractableObject.InteractionType.LookedAt:
                DebugDraw.Box(position - halfScale, position + halfScale, globalBasis.Orthonormalized(), Colors.Aqua, drawOnTop:true);
                break;
            case InteractableObject.InteractionType.UseAction:
                GD.Print("Interacted by player");
                break;
        }
        
        
        
        
        
    }
}