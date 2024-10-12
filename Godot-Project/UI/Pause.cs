using Godot;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class Pause : Node
{
    public static Pause Singleton => _singleton;
    private static Pause _singleton;
    public bool IsPaused => _isPaused;
    private bool _isPaused;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if(_singleton != null)
        {
            GD.PushError("Pause: singleton already exists.");
        }
        _singleton = this;
    }

    public override void _Input(InputEvent @event)
    {
        if(GameManager.Singleton.CurrentGameState == GameManager.GameState.InGame)
        {
            if (@event.IsActionPressed("miscellaneous_pause"))
            {
                GD.Print("Pause: pause input received...");
                SetPause(!_isPaused);
            }
        }
        
    }

    public void SetPause(bool pause)
    {
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
}