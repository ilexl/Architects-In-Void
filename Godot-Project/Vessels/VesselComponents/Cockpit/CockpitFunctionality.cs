using ArchitectsInVoid.Interactables;
using ArchitectsInVoid.Player;
using Godot;

namespace ArchitectsInVoid.Vessels.VesselComponents.Cockpit;

public partial class CockpitFunctionality : Node
{
    [Export] public InteractableObject _interactor;


    public override void _Ready()
    {
        _interactor.Interacted += CockpitInteracted;
    }

    private void CockpitInteracted(PlayerController player)
    {
        GD.Print("Interacted by player");
    }
}