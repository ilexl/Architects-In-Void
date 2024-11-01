using ArchitectsInVoid.UI;
using Godot;
using System;

public partial class ComponentSelectionUI : Control
{
    [Export] Control _blockSelectionMenu;

    public override void _Ready()
    {
        MenuShown(false);
    }

    public void MenuShown(bool shown)
    {
        _blockSelectionMenu.Visible = shown;
    }

    public override void _Input(InputEvent @event)
    {
        if (GameManager.Singleton.CurrentGameState == GameManager.GameState.MainMenu)
        {
            return;
        }
        if (Pause.Singleton.IsPaused == true)
        {
            return;
        }
        if (@event.IsActionPressed("component_config_menu"))
        {
            GD.Print("ComponentSelectionUI: config menu button pressed");
            MenuShown(!_blockSelectionMenu.Visible);
        }
    }

}
