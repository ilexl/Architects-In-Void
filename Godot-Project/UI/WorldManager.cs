using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class WorldManager : Node
{
    [Export] WindowManager wmMain;
    [Export] Window winMainMenu;
    [Export] Node worldListHolder;
	[Export] bool testWorldList;
	[Export] int testAmount;
	[Export] PackedScene worldSaveListScene;
    [Export] TextureButton cancelBtn, loadBtn;
    List<WorldSaveTitle> currentlySelected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        if(wmMain == null)
        {
            wmMain = (WindowManager)GetParent().GetParent();
        }
        if (wmMain == null)
        {
            GD.PushError("World Manager: missing WindowManger/s...");
            return;
        }
        if (winMainMenu == null)
        {
            winMainMenu = (Window)wmMain.FindChild("MainMenu", recursive: false);
        }
        if (winMainMenu == null)
        {
            GD.PushError("WorldManager: missing windows...");
            return;
        }
        if (worldSaveListScene == null)
		{
			GD.PushError("WorldManager: No Packed Scene found for worldSaveListScene...");
			return;
		}
		if(worldListHolder == null)
		{
			worldListHolder = (Node)GetParent().FindChild("WorldListHolder", recursive: true);
		}
        if (worldListHolder == null)
		{
            GD.PushError("WorldManager: No node found for worldListHolder...");
			return;	
        }
        if (cancelBtn == null || loadBtn == null)
        {
            cancelBtn = (TextureButton)GetParent().FindChild("CancelBtn", recursive: true);
            loadBtn = (TextureButton)GetParent().FindChild("LoadBtn", recursive: true);
        }
        if (cancelBtn == null || loadBtn == null)
        {
            GD.PushError("WorldManager: missing buttons...");
            return;
        }

        currentlySelected = new List<WorldSaveTitle>();
        cancelBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Cancel));
        loadBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(LoadSelectedWorld));
        loadBtn.Disabled = true;
    }

    void Cancel()
    {
        GD.Print("Settings: Cancel Button Pressed");
        wmMain.ShowWindow(winMainMenu);
    }

	public void CallLoad()
	{
		GD.Print("WorldManager: load called");
        foreach(Node n in worldListHolder.GetChildren())
        {
            worldListHolder.RemoveChild(n);
        }
        currentlySelected = new List<WorldSaveTitle>();
        loadBtn.Disabled = true;

        if (testWorldList)
        {
            Test();
        }
        else
        {

        }
    }

	void Test()
	{
        for (int i = 0; i < testAmount; i++)
        {
            var inst = worldSaveListScene.Instantiate();
            worldListHolder.AddChild(inst);
            WorldSaveTitle wst = (WorldSaveTitle)inst;
            string title = "Lorem ipsum dolar " + i.ToString();
            Random r = new Random();
            string date = $"{r.Next(0, 10)}{r.Next(0, 10)}-{r.Next(0, 10)}{r.Next(0, 10)}-{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)} {r.Next(0, 10)}{r.Next(0, 10)}:{r.Next(0, 10)}{r.Next(0, 10)}:{r.Next(0, 10)}{r.Next(0, 10)}";
            wst.UpdateWorldSaveTitle(title, date);
            wst.BindButtonToManager(this);
        }
    }
    public void CallNew()
    {
        GD.Print("WorldManager: new called");
    }

    void LoadSelectedWorld()
    {
        GD.Print("WorldManager: loading selected world");
    }

    public void ListedWorldClicked(WorldSaveTitle wst)
    {
        bool currentState = wst.GetButtonState();
        GD.Print($"ListedWorldClicked: {wst.title} received with state {currentState}");
        if (currentState == false) { currentlySelected.Remove(wst); }
        if (currentState == true) { currentlySelected.Add(wst); }

        if(currentlySelected.Count == 0 || currentlySelected.Count > 1)
        {
            loadBtn.Disabled = true;
        }
        else if (currentlySelected.Count == 1)
        {
            loadBtn.Disabled = false;
        }
        else
        {
            GD.PushError("WorldManger: invalid amount in list...");
        }
    }

}
