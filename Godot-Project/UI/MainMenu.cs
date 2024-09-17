using Godot;
using Godot.Collections;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class MainMenu : Node
{
    [Export] private TextureButton _resumeBtn, _newBtn, _loadBtn, _optionsBtn, _exitBtn;
    [Export] private Window _winSettings, _winWorldManager;

    [Export] private WindowManager _wm;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (_wm == null)
        {
            _wm = (WindowManager)GetParent().GetParent();
            if (_wm == null)
            {
                GD.PushError("MainMenu: missing WindowManger...");
                return;
            }
        }
        if (_winSettings == null || _winWorldManager == null)
        {
            _winSettings = (Window)_wm.FindChild("Settings", false);
            _winWorldManager = (Window)_wm.FindChild("WorldManager", false);
            if (_winSettings == null || _winWorldManager == null)
            {
                GD.PushError("MainMenu: missing windows...");
                return;
            }
        }
        if (_resumeBtn == null || _newBtn == null || _loadBtn == null || _optionsBtn == null || _exitBtn == null)
        {
            _resumeBtn = (TextureButton)FindChild("Resume");
            _newBtn = (TextureButton)FindChild("New");
            _loadBtn = (TextureButton)FindChild("Load");
            _optionsBtn = (TextureButton)FindChild("Options");
            _exitBtn = (TextureButton)FindChild("Exit");
            if (_resumeBtn == null || _newBtn == null || _loadBtn == null || _optionsBtn == null || _exitBtn == null)
            {
                GD.PushError("MainMenu: missing buttons...");
                return;
            }
        }

        if (!_resumeBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(ResumeGame)))
        {
            _resumeBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(ResumeGame));
        }
        if (!_newBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(NewGame)))
        {
            _newBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(NewGame));
        }
        if (!_loadBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(LoadGame)))
        {
            _loadBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(LoadGame));
        }
        if (!_optionsBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Options)))
        {
            _optionsBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Options));
        }
        if (!_exitBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Exit)))
        {
            _exitBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Exit));
        }
    }

    private void ResumeGame()
    {
        GD.Print("MainMenu: Resume Game");
    }

    private void NewGame()
    {
        GD.Print("MainMenu: New Game");
        _wm.ShowWindow(_winWorldManager);
        foreach (var n in _winWorldManager.GetChildren())
        {
            var w = (WorldManager)n;
            if (w != null)
            {
                w.CallNew();
                break;
            }
        }
    }

    private void LoadGame()
    {
        GD.Print("MainMenu: Load Game");
        _wm.ShowWindow(_winWorldManager);
        foreach (var n in _winWorldManager.GetChildren())
        {
            var w = (WorldManager)n;
            if (w != null)
            {
                w.CallLoad();
                break;
            }
        }
    }

    private void Options()
    {
        GD.Print("MainMenu: Options");
        _wm.ShowWindow(_winSettings);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest) Exit();
    }

    private void Exit()
    {
        GD.Print("INFO: Exit");
        GetTree().Quit();
    }

    public Godot.Collections.Array AddInspectorButtons()
    {
        var buttons = new Godot.Collections.Array();

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