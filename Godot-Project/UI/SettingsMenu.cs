using Godot;
using Godot.Collections;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class SettingsMenu : Node
{
    [Export] private TextureButton _cancelBtn, _applyBtn, _resetBtn, _gameSBtn, _controlsSBtn, _audioSBtn, _displaySBtn;
    [Export] private Window _winMainMenu, _winSubGame, _winSubControls, _winSubAudio, _winSubDisplay;

    [Export] private WindowManager _wmMain, _wmSettingSub;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (_wmMain == null || _wmSettingSub == null)
        {
            _wmMain = (WindowManager)GetParent().GetParent();
            _wmSettingSub = (WindowManager)GetParent().FindChild("SettingsSubMenus");
        }

        if (_wmMain == null || _wmSettingSub == null)
        {
            GD.PushError("Settings: missing WindowManger/s...");
            return;
        }

        if (_winMainMenu == null) _winMainMenu = (Window)_wmMain.FindChild("MainMenu", false);
        if (_winMainMenu == null)
        {
            GD.PushError("Settings: missing windows...");
            return;
        }

        if (_winSubGame == null || _winSubControls == null || _winSubAudio == null || _winSubDisplay == null)
        {
            _winSubGame = (Window)_wmSettingSub.FindChild("GameSettings");
            _winSubControls = (Window)_wmSettingSub.FindChild("ControlSettings");
            _winSubAudio = (Window)_wmSettingSub.FindChild("AudioSettings");
            _winSubDisplay = (Window)_wmSettingSub.FindChild("DisplaySettings");
        }

        if (_winSubGame == null || _winSubControls == null || _winSubAudio == null || _winSubDisplay == null)
        {
            GD.PushError("Settings: missing windows...");
            return;
        }

        if (_cancelBtn == null || _applyBtn == null || _resetBtn == null || _gameSBtn == null || _controlsSBtn == null ||
            _audioSBtn == null || _displaySBtn == null)
        {
            _cancelBtn = (TextureButton)GetParent().FindChild("CancelBtn");
            _applyBtn = (TextureButton)GetParent().FindChild("ApplyBtn");
            _resetBtn = (TextureButton)GetParent().FindChild("ResetSettingsBtn");
            _gameSBtn = (TextureButton)GetParent().FindChild("GameSettingsBtn");
            _controlsSBtn = (TextureButton)GetParent().FindChild("ControlsSettingsBtn");
            _audioSBtn = (TextureButton)GetParent().FindChild("AudioSettingsBtn");
            _displaySBtn = (TextureButton)GetParent().FindChild("DisplaySettingsBtn");
        }

        if (_cancelBtn == null || _applyBtn == null || _resetBtn == null || _gameSBtn == null || _controlsSBtn == null ||
            _audioSBtn == null || _displaySBtn == null)
        {
            GD.PushError("Settings: missing buttons...");
            return;
        }

        _cancelBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Cancel));
        _applyBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Apply));
        _resetBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Reset));
        _gameSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubGame));
        _controlsSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubControls));
        _audioSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubAudio));
        _displaySBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubDisplay));

        _gameSBtn.ButtonPressed = true;
    }

    private void Apply()
    {
        GD.Print("Settings: Apply Button Pressed");
    }

    private void Cancel()
    {
        GD.Print("Settings: Cancel Button Pressed");
        _wmMain.ShowWindow(_winMainMenu);
    }

    private void SubGame()
    {
        GD.Print("Settings: Game Settings Button Pressed");
        _controlsSBtn.ButtonPressed = false;
        _audioSBtn.ButtonPressed = false;
        _displaySBtn.ButtonPressed = false;
        _wmSettingSub.ShowWindow(_winSubGame);
    }

    private void SubControls()
    {
        GD.Print("Settings: Controls Settings Button Pressed");
        _gameSBtn.ButtonPressed = false;
        _audioSBtn.ButtonPressed = false;
        _displaySBtn.ButtonPressed = false;
        _wmSettingSub.ShowWindow(_winSubControls);
    }

    private void SubAudio()
    {
        GD.Print("Settings: Audio Settings Button Pressed");
        _gameSBtn.ButtonPressed = false;
        _controlsSBtn.ButtonPressed = false;
        _displaySBtn.ButtonPressed = false;
        _wmSettingSub.ShowWindow(_winSubAudio);
    }

    private void SubDisplay()
    {
        GD.Print("Settings: Display Settings Button Pressed");
        _gameSBtn.ButtonPressed = false;
        _controlsSBtn.ButtonPressed = false;
        _audioSBtn.ButtonPressed = false;
        _wmSettingSub.ShowWindow(_winSubDisplay);
    }

    private void Reset()
    {
        GD.Print("Settings: Reset Settings Button Pressed");
    }

    public Array AddInspectorButtons()
    {
        var buttons = new Array();

        var reload = new Dictionary
        {
            { "name", "Reload" },
            { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
            {
                "pressed", Callable.From(_Ready)
            }
        };
        buttons.Add(reload);


        return buttons;
    }
}