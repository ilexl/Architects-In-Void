using ArchitectsInVoid.WorldData;
using Godot;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class Pause : Node
{
    [Export] TextureButton _resumeBtn, _saveGameBtn, _loadGameBtn, _mainMenuBtn, _desktopBtn;
    public static Pause Singleton => _singleton;
    private static Pause _singleton;
    public bool IsPaused => _isPaused;
    private bool _isPaused;

    private bool _gameSavedWhilePaused;
    private bool _readyForPauseInput;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gameSavedWhilePaused = false;
        if (_singleton != null)
        {
            GD.Print("Pause: singleton already exists.");
        }
        _singleton = this;

        if(_resumeBtn == null || _saveGameBtn == null || _loadGameBtn == null || _mainMenuBtn == null || _desktopBtn == null)
        {
            GD.Print("Pause: missing texture buttons...");
            return;
        }

        if (!_resumeBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(ResumeGame)))
        {
            _resumeBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(ResumeGame));
        }
        if (!_saveGameBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SaveGame)))
        {
            _saveGameBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SaveGame));
        }
        if (!_loadGameBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(LoadGame)))
        {
            _loadGameBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(LoadGame));
        }
        if (!_mainMenuBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(MainMenu)))
        {
            _mainMenuBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(MainMenu));
        }
        if (!_desktopBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Desktop)))
        {
            _desktopBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Desktop));
        }
        _readyForPauseInput = true;
    }

    public override void _Input(InputEvent @event)
    {
        if(GameManager.Singleton.CurrentGameState == GameManager.GameState.InGame)
        {
            if (@event.IsActionPressed("miscellaneous_pause") && _readyForPauseInput)
            {
                _readyForPauseInput = false;
                GD.Print(@event as InputEventAction);
                GD.Print("Pause: pause input received...");

                if (_isPaused)
                {
                    GD.Print("Pause: game is currently paused so unpausing it now");
                    SetPause(false);
                }
                else
                {
                    GD.Print("Pause: game is currently unpaused so pausing it now");
                    SetPause(true);
                }
                _readyForPauseInput = true;
            }
        }
        
    }

    public void SetPause(bool pause)
    {
        _gameSavedWhilePaused = false;
        _isPaused = pause;
        if(IsPaused)
        {
            ((UIManager)GameManager.Singleton.FindChild("UI"))._windowManager.ShowWindow("PauseMenu");
            GD.Print("Pause: game has been paused");
        }
        else
        {
            ((UIManager)GameManager.Singleton.FindChild("UI"))._windowManager.ShowWindow("HUD");
            GD.Print("Pause: game has been unpaused");
        }
    }

    private void ResumeGame()
    {
        GD.Print("Pause: resume btn pressed");
        SetPause(false);
    }
    private void SaveGame()
    {
        GD.Print("Pause: save btn pressed");
    }
    private void LoadGame()
    {
        GD.Print("Pause: load btn pressed");

    }
    private void MainMenu()
    {
        GD.Print("Pause: main menu btn pressed");
        if (_gameSavedWhilePaused)
        {
            MainMenuConfirmed();
        }
        else
        {
            ((UIManager)GameManager.Singleton.FindChild("UI"))._popup.DisplayConfirmPopUp("Are you sure you want to exit without saving?", Callable.From(MainMenuConfirmed));
        }
    }

    public void MainMenuConfirmed()
    {
        ((UIManager)GameManager.Singleton.FindChild("UI"))._windowManager.ShowWindow("MainMenu");
        GameManager.Singleton.SetGameState(GameManager.GameState.MainMenu);
        ((Data)GameManager.Singleton.FindChild("Data")).Clear();
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
    private void Desktop()
    {
        GD.Print("Pause: desktop btn pressed");
        if (_gameSavedWhilePaused)
        {
            DesktopConfirmed();
        }
        else
        {
            ((UIManager)GameManager.Singleton.FindChild("UI"))._popup.DisplayConfirmPopUp("Are you sure you want to exit without saving?", Callable.From(DesktopConfirmed));
        }
    }

    public void DesktopConfirmed()
    {
        Exit();
    }
}