using Godot;
using System;

[Tool]
public partial class SettingsMenu : Node
{
    [Export] WindowManager wmMain, wmSettingSub;
    [Export] Window winMainMenu, winSubGame, winSubControls, winSubAudio, winSubDisplay;
    [Export] TextureButton cancelBtn, applyBtn, resetBtn, gameSBtn, controlsSBtn, audioSBtn, displaySBtn;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (wmMain == null || wmSettingSub == null)
        {
            wmMain = (WindowManager)GetParent().GetParent();
            wmSettingSub = (WindowManager)GetParent().FindChild("SettingsSubMenus", recursive:true);
        }
        if (wmMain == null || wmSettingSub == null)
        {
            GD.PushError("Settings: missing WindowManger/s...");
            return;
        }

        if (winMainMenu == null )
        {
            winMainMenu = (Window)wmMain.FindChild("MainMenu", recursive:false);
        }
        if (winMainMenu == null)
        {
            GD.PushError("Settings: missing windows...");
            return;
        }

        if (winSubGame == null || winSubControls == null || winSubAudio == null || winSubDisplay == null)
        {
            winSubGame = (Window)wmSettingSub.FindChild("GameSettings");
            winSubControls = (Window)wmSettingSub.FindChild("ControlSettings");
            winSubAudio = (Window)wmSettingSub.FindChild("AudioSettings");
            winSubDisplay = (Window)wmSettingSub.FindChild("DisplaySettings");
        }
        if (winSubGame == null || winSubControls == null || winSubAudio == null || winSubDisplay == null)
        {
            GD.PushError("Settings: missing windows...");
            return;
        }

        if (cancelBtn == null || applyBtn == null || resetBtn == null || gameSBtn == null || controlsSBtn == null || audioSBtn == null || displaySBtn == null)
        {
            cancelBtn = (TextureButton)GetParent().FindChild("CancelBtn");
            applyBtn = (TextureButton)GetParent().FindChild("ApplyBtn");
            resetBtn = (TextureButton)GetParent().FindChild("ResetSettingsBtn");
            gameSBtn = (TextureButton)GetParent().FindChild("GameSettingsBtn");
            controlsSBtn = (TextureButton)GetParent().FindChild("ControlsSettingsBtn");
            audioSBtn = (TextureButton)GetParent().FindChild("AudioSettingsBtn");
            displaySBtn = (TextureButton)GetParent().FindChild("DisplaySettingsBtn");
        }
        if (cancelBtn == null || applyBtn == null || resetBtn == null || gameSBtn == null || controlsSBtn == null || audioSBtn == null || displaySBtn == null)
        {
            GD.PushError("Settings: missing buttons...");
            return;
        }

        cancelBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Cancel));
        applyBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Apply));
        resetBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Reset));
        gameSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubGame));
        controlsSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubControls));
        audioSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubAudio));
        displaySBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubDisplay));

        gameSBtn.ButtonPressed = true;
    }

    void Apply()
    {
        GD.Print("Settings: Apply Button Pressed");
    }

    void Cancel()
    {
        GD.Print("Settings: Cancel Button Pressed");
        wmMain.ShowWindow(winMainMenu);
    }

    void SubGame()
    {
        GD.Print("Settings: Game Settings Button Pressed");
        controlsSBtn.ButtonPressed = false;
        audioSBtn.ButtonPressed = false;
        displaySBtn.ButtonPressed = false;
        wmSettingSub.ShowWindow(winSubGame);
    }

    void SubControls()
    {
        GD.Print("Settings: Controls Settings Button Pressed");
        gameSBtn.ButtonPressed = false;
        audioSBtn.ButtonPressed = false;
        displaySBtn.ButtonPressed = false;
        wmSettingSub.ShowWindow(winSubControls);

    }

    void SubAudio()
    {
        GD.Print("Settings: Audio Settings Button Pressed");
        gameSBtn.ButtonPressed = false;
        controlsSBtn.ButtonPressed = false;
        displaySBtn.ButtonPressed = false;
        wmSettingSub.ShowWindow(winSubAudio);

    }

    void SubDisplay()
    {
        GD.Print("Settings: Display Settings Button Pressed");
        gameSBtn.ButtonPressed = false;
        controlsSBtn.ButtonPressed = false;
        audioSBtn.ButtonPressed = false;
        wmSettingSub.ShowWindow(winSubDisplay);

    }

    void Reset()
    {
        GD.Print("Settings: Reset Settings Button Pressed");

    }

    public Godot.Collections.Array AddInspectorButtons()
    {
        Godot.Collections.Array buttons = new Godot.Collections.Array();

        Godot.Collections.Dictionary reload = new Godot.Collections.Dictionary
        {
            { "name", "Reload" },
            { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
            { "pressed", Callable.From(_Ready)
            }
        };
        buttons.Add(reload);


        return buttons;
    }
}
