using ArchitectsInVoid.VesselComponent;
using Godot;

namespace ArchitectsInVoid.Player.HotBar;

public partial class HotBarManager : Node
{
	// Hotbar array made accessible to everything
	[Export] public PackedScene[] HotBar;
	
	// Reference to the component creator for use with any scenes that inherit from PlaceableComponent
	private ComponentCreation.ComponentCreator _componentCreator;
	private PackedScene _selectedScene;
	public int HotBarIndex;
	
	
	public override void _Ready()
	{
		_componentCreator = GetNode<ComponentCreation.ComponentCreator>("../ComponentCreator");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		for (var i = 0; i < 10; i++) // Assuming "hotbar_0" to "hotbar_9"
			if (Input.IsActionJustPressed($"hotbar_{i}"))
			{
				_selectedScene = HotBar[i];
				HotBarIndex = i;
				
				if (_selectedScene == null) // Hotbar slot is empty
				{
					GD.Print("Hotbar slot is empty");
					_componentCreator.SelectedComponent = null;
					return;
				}

				var instance = _selectedScene.Instantiate();
				_componentCreator.SelectedComponent = instance is PlaceableComponent ? _selectedScene : null;
				instance.Free(); // Immediately delete instance from memory as it is no longer required
				// Because of this, it is important that hotbar scenes have nothing in their constructor
				
			}
	}
}