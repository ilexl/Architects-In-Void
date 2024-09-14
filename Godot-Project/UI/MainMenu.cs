using Godot;
using System;

public partial class MainMenu : Node
{
    [Export] WindowManager wm;
    [Export] Window winSettings, winWorldManager;
    [Export] TextureButton resumeBtn, newBtn, loadBtn, optionsBtn, exitBtn;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (wm == null)
        {
            wm = (WindowManager)GetParent().GetParent();
        }
        if(wm == null)
        {
            GD.PushError("MainMenu: missing WindowManger...");
            return;
        }

        if (winSettings == null || winWorldManager == null)
        {
            winSettings = (Window)wm.FindChild("Settings", recursive: false);
            winWorldManager = (Window)wm.FindChild("WorldManager", recursive: false);
        }
        if (winSettings == null || winWorldManager == null)
        {
            GD.PushError("MainMenu: missing windows...");
            return;
        }

        if (resumeBtn == null || newBtn == null || loadBtn == null || optionsBtn == null || exitBtn == null)
        {
            resumeBtn = (TextureButton)FindChild("Resume");
            newBtn = (TextureButton)FindChild("New");
            loadBtn = (TextureButton)FindChild("Load");
            optionsBtn = (TextureButton)FindChild("Options");
            exitBtn = (TextureButton)FindChild("Exit");
        }
        if (resumeBtn == null || newBtn == null || loadBtn == null || optionsBtn == null || exitBtn == null)
        {
            GD.PushError("MainMenu: missing buttons...");
            return;
        }

        resumeBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(ResumeGame));
        newBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(NewGame));
        loadBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(LoadGame));
        optionsBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Options));
        exitBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Exit));
    }

    void ResumeGame()
	{
        GD.Print("MainMenu: Resume Game");
	}
    void NewGame()
    {
        GD.Print("MainMenu: New Game");
        wm.ShowWindow(winWorldManager);
        foreach (Node n in winWorldManager.GetChildren())
        {
            WorldManager w = (WorldManager)n;
            if (w != null)
            {
                w.CallNew();
                break;
            }
        }
    }
    void LoadGame()
    {
        GD.Print("MainMenu: Load Game");
        wm.ShowWindow(winWorldManager);
        foreach (Node n in winWorldManager.GetChildren())
        {
            WorldManager w = (WorldManager)n;
            if (w != null)
            {
                w.CallLoad();
                break;
            }
        }
    }
    void Options()
    {
        GD.Print("MainMenu: Options");
        wm.ShowWindow(winSettings);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            Exit();
        }
    }
    void Exit()
    {
        GD.Print("INFO: Exit");
        GetTree().Quit();
    }
}
