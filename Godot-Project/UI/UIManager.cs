using ArchitectsInVoid.Settings;
using ArchitectsInVoid.UI;
using Godot;
using Godot.Collections;
using System;

/// <summary>
/// Manager for anything UI related
/// <br/>Contains: MainMenu, SettingsMenu, Settings, WorldManager, LoadingScreen, HUD, Pause, PopUp, WindowManager
/// </summary>
[Tool]
public partial class UIManager : Node
{
    #region Variables

    [Export] MainMenu _mainMenu;
	[Export] SettingsMenu _settingsMenu;
	[Export] WorldManager _worldMenu;
	[Export] LoadingScreen _loadingMenu;
	[Export] HUD _hudMenu;
	[Export] Pause _pauseMenu;
    [Export] public PopUp PopUpManager;
	[Export] public WindowManager UIWindowManager;
    [Export] public Settings SettingsManager;

    #endregion

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
	{
        if (Engine.IsEditorHint()) { return; } // do NOT run when not in game
        _ = _GetDependents();
    }

    /// <summary>
    /// Ensures all managed nodes and scripts have been loaded and referenced
    /// <br/>Errors if something is missing...
    /// </summary>
    bool _GetDependents()
    {
        if (UIWindowManager == null)
        {
            UIWindowManager = (WindowManager)FindChild("WindowManager", recursive: false);

            if (UIWindowManager == null)
            {
                GD.PushError("UIManager: window manager not found...");
                return false;
            }
        }
        if (_mainMenu == null)
        {
            _mainMenu = (MainMenu)UIWindowManager.FindChild("MainMenuScript");

            if (_mainMenu == null)
            {
                GD.PushError("UIManager: main menu not found...");
                return false;
            }
        }
        if (_settingsMenu == null)
        {
            _settingsMenu = (SettingsMenu)UIWindowManager.FindChild("SettingsManager");

            if (_settingsMenu == null)
            {
                GD.PushError("UIManager: settings menu not found...");
                return false;
            }
        }
        if (SettingsManager == null)
        {
            SettingsManager = (Settings)this.GetParent().FindChild("Settings", false);
            if (SettingsManager == null)
            {
                GD.PushError("UIManager: settings not found...");
                return false;
            }
        }
        if (_worldMenu == null)
        {
            _worldMenu = (WorldManager)UIWindowManager.FindChild("WorldManagerButtons");

            if (_worldMenu == null)
            {
                GD.PushError("UIManager: world manager not found...");
                return false;
            }
        }
        if (_loadingMenu == null)
        {
            _loadingMenu = (LoadingScreen)UIWindowManager.FindChild("LoadingScreenLogic");

            if (_loadingMenu == null)
            {
                GD.PushError("UIManager: loading screen not found...");
                return false;
            }
        }
        if (_hudMenu == null)
        {
            _hudMenu = (HUD)UIWindowManager.FindChild("HUDButtons");


            if (_hudMenu == null)
            {
                GD.PushError("UIManager: HUD not found...");
                return false;
            }
        }
        if (_pauseMenu == null)
        {
            _pauseMenu = (Pause)UIWindowManager.FindChild("PauseMenuButtons");


            if (_pauseMenu == null)
            {
                GD.PushError("UIManager: pause menu not found...");
                return false;
            }
        }
        if (PopUpManager == null)
        {
            PopUpManager = (PopUp)UIWindowManager.FindChild("PopUp");

            if (PopUpManager == null)
            {
                GD.PushError("UIManager: popup not found...");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Calls _Ready to all scripts managed by this
    /// </summary>
    void _RefreshAllUIElements()
    {
        bool success = _GetDependents();

        if(UIWindowManager !=  null)
        {
            UIWindowManager.ManualRefresh();
        }

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

    /// <summary>
    /// Used for the Inspector Buttons plugin
    /// </summary>
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
