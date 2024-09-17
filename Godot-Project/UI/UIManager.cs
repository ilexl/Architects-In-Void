using ArchitectsInVoid.UI;
using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class UIManager : Node
{
	[Export] MainMenu _mainMenu;
	[Export] SettingsMenu _settingsMenu;
	[Export] WorldManager _worldMenu;
	[Export] LoadingScreen _loadingMenu;
	[Export] HUD _hudMenu;
	[Export] Pause _pauseMenu;
    [Export] public PopUp _popup;

	[Export] WindowManager _windowManager;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _GetDependents();
    }

    bool _GetDependents()
    {
        if (_windowManager == null)
        {
            _windowManager = (WindowManager)FindChild("WindowManager", recursive: false);

            if (_windowManager == null)
            {
                GD.PushError("UIManager: window manager not found...");
                return false;
            }
        }
        if (_mainMenu == null)
        {
            _mainMenu = (MainMenu)_windowManager.FindChild("MainMenuScript");

            if (_mainMenu == null)
            {
                GD.PushError("UIManager: main menu not found...");
                return false;
            }
        }
        if (_settingsMenu == null)
        {
            _settingsMenu = (SettingsMenu)_windowManager.FindChild("SettingsManager");

            if (_settingsMenu == null)
            {
                GD.PushError("UIManager: settings not found...");
                return false;
            }
        }
        if (_worldMenu == null)
        {
            _worldMenu = (WorldManager)_windowManager.FindChild("WorldManagerButtons");

            if (_worldMenu == null)
            {
                GD.PushError("UIManager: world manager not found...");
                return false;
            }
        }
        if (_loadingMenu == null)
        {
            _loadingMenu = (LoadingScreen)_windowManager.FindChild("LoadingScreenLogic");

            if (_loadingMenu == null)
            {
                GD.PushError("UIManager: loading screen not found...");
                return false;
            }
        }
        if (_hudMenu == null)
        {
            _hudMenu = (HUD)_windowManager.FindChild("HUDButtons");


            if (_hudMenu == null)
            {
                GD.PushError("UIManager: HUD not found...");
                return false;
            }
        }
        if (_pauseMenu == null)
        {
            _pauseMenu = (Pause)_windowManager.FindChild("PauseMenuButtons");


            if (_pauseMenu == null)
            {
                GD.PushError("UIManager: pause menu not found...");
                return false;
            }
        }
        if (_popup == null)
        {
            _popup = (PopUp)_windowManager.FindChild("PopUp");

            if (_popup == null)
            {
                GD.PushError("UIManager: popup not found...");
                return false;
            }
        }

        return true;
    }

    void _RefreshAllUIElements()
    {
        bool success = _GetDependents();
        if(success)
        {
            _mainMenu._Ready();
            _settingsMenu._Ready();
            _worldMenu._Ready();
            _loadingMenu._Ready();
            _hudMenu._Ready();
            _pauseMenu._Ready();
        }
    }

    public Godot.Collections.Array AddInspectorButtons()
    {
        var buttons = new Godot.Collections.Array();

        var btnRefresh = new Dictionary
            {
                { "name", "Refresh All" },
                { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
                { "pressed", Callable.From(_RefreshAllUIElements) }
            };
        buttons.Add(btnRefresh);

        return buttons;
    }
}
