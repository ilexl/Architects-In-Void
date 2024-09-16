using ArchitectsInVoid.VesselComponent;
using Godot;

namespace ArchitectsInVoid.Player;

public partial class HotbarManager : Node
{
	[Export] public PackedScene[] Hotbar;
	
	private ComponentCreator _componentCreator;
	private PackedScene _selectedComponentScene;
	private int _hotbarIndex;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_componentCreator = GetNode<ComponentCreator>("../ComponentCreator");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		for (var i = 0; i < 10; i++) // Assuming "hotbar_0" to "hotbar_9"
			if (Input.IsActionJustPressed($"hotbar_{i}"))
			{
				_selectedComponentScene = Hotbar[i];
				_hotbarIndex = i;
				
				_componentCreator.SelectedComponentScene = Hotbar[i].GetScript().As<PlaceableComponent>() != null ? _selectedComponentScene : null;
			}
	}
}