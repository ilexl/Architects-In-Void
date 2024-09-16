using ArchitectsInVoid.VesselComponent;
using Godot;

namespace ArchitectsInVoid.Player;

public partial class HotbarManager : Node
{
	[Export] public PackedScene[] Hotbar;
	
	private ComponentCreator _componentCreator;
	private PackedScene _selectedScene;
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
				GD.Print("Pressed");
				_selectedScene = Hotbar[i];
				_hotbarIndex = i;
				
				if (_selectedScene == null) // Hotbar slot is empty
				{
					GD.Print("Hotbar slot is empty");
					return;
				}

				var instance = _selectedScene.Instantiate();
				_componentCreator.SelectedComponentScene = instance is PlaceableComponent ? _selectedScene : null;
				instance.Free(); // Immediately delete instance from memory as it is no longer required
				// Because of this, it is important that hotbar scenes have nothing in their constructor
				
			}
	}
}