using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ArchitectsInVoid.UI.UIElements;
using ArchitectsInVoid.WorldData;
using Godot;
using Godot.Collections;


namespace ArchitectsInVoid.UI;

[Tool]
public partial class WorldManager : Node
{
    [Export] private TextureButton _cancelBtn, _loadBtn;
    private List<WorldSaveTitle> _currentlySelected;
    [Export] private int _testAmount;
    [Export] private bool _testWorldList;
    [Export] private Window _winMainMenu, _winHud;
    [Export] private WindowManager _wmMain;
    [Export] private Node _worldListHolder;
    [Export] private PackedScene _worldSaveListScene;
    [Export] UIManager _UIManager;
    [Export] Data data;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (_wmMain == null)
        {
            _wmMain = (WindowManager)GetParent().GetParent();
            if (_wmMain == null)
            {
                GD.PushError("World Manager: missing WindowManger/s...");
                return;
            }
        }
        if (_winMainMenu == null || _winHud == null)
        {
            _winMainMenu = (Window)_wmMain.FindChild("MainMenu", false);
            _winHud = (Window)_wmMain.FindChild("HUD", false);
            if (_winMainMenu == null || _winHud == null)
            {
                GD.PushError("WorldManager: missing windows...");
                return;
            }
        }
        if (_worldSaveListScene == null)
        {
            _worldSaveListScene = (PackedScene)GD.Load("res://UI/UIElements/world_save.tscn");
            if (_worldSaveListScene == null)
            {
                GD.PushError("WorldManager: No Packed Scene found for worldSaveListScene...");
                return;
            }
        }
        if (_worldListHolder == null)
        {
            _worldListHolder = GetParent().FindChild("WorldListHolder");
            if (_worldListHolder == null)
            {
                GD.PushError("WorldManager: No node found for worldListHolder...");
                return;
            }
        }
        if (_cancelBtn == null || _loadBtn == null)
        {
            _cancelBtn = (TextureButton)GetParent().FindChild("CancelBtn");
            _loadBtn = (TextureButton)GetParent().FindChild("LoadBtn");
            if (_cancelBtn == null || _loadBtn == null)
            {
                GD.PushError("WorldManager: missing buttons...");
                return;
            }
        }

        if (!_cancelBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Cancel)))
        {
            _cancelBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Cancel));
        }
        if (!_loadBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(LoadSelectedWorld)))
        {
            _loadBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(LoadSelectedWorld));
        }

        _currentlySelected = new List<WorldSaveTitle>();
        _loadBtn.Disabled = true;

        _UIManager = ((UIManager)_wmMain.GetParent());
        data = (Data)_UIManager.GetParent().FindChild("Data");
    }

    private void Cancel()
    {
        GD.Print("Settings: Cancel Button Pressed");
        _wmMain.ShowWindow(_winMainMenu);
    }

    public void CallLoad()
    {
        GD.Print("WorldManager: load called");
        _currentlySelected = new List<WorldSaveTitle>();
        _loadBtn.Disabled = true;

        if (_testWorldList) Test();
        else RefreshSaves();
    }

    private void RefreshSaves()
    {
        foreach (var n in _worldListHolder.GetChildren()) _worldListHolder.RemoveChild(n);
        foreach (string worldName in data.RetrieveAllValidSavesAsList())
        {
            var inst = _worldSaveListScene.Instantiate();
            _worldListHolder.AddChild(inst);
            var wst = (WorldSaveTitle)inst;
            var title = worldName;
            var date = data.GetLastSavedFromFile(worldName);
            wst.UpdateWorldSaveTitle(title, date);
            wst.BindButtonToManager(this);
        }
    }

    private void Test()
    {
        foreach (var n in _worldListHolder.GetChildren()) _worldListHolder.RemoveChild(n);
        for (var i = 0; i < _testAmount; i++)
        {
            var inst = _worldSaveListScene.Instantiate();
            _worldListHolder.AddChild(inst);
            var wst = (WorldSaveTitle)inst;
            var title = "Lorem ipsum dolar " + i;
            var r = new Random();
            var date = $"{r.Next(0, 10)}{r.Next(0, 10)}-{r.Next(0, 10)}{r.Next(0, 10)}-{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)}{r.Next(0, 10)} {r.Next(0, 10)}{r.Next(0, 10)}:{r.Next(0, 10)}{r.Next(0, 10)}:{r.Next(0, 10)}{r.Next(0, 10)}";
            wst.UpdateWorldSaveTitle(title, date);
            wst.BindButtonToManager(this);
        }
    }

    public void CallNew()
    {
        GD.Print("WorldManager: new called");
        _currentlySelected = new List<WorldSaveTitle>();
        _loadBtn.Disabled = true;

        _UIManager._popup.DisplayInputPopUp("Enter new world name:", Callable.From(NewWorldConfirmed));
        RefreshSaves();
    }

    public void NewWorldConfirmed()
    {
        string input = _UIManager._popup.LastInput;
        if (input.Length == 0)
        {
            _UIManager._popup.DisplayError("Error: input", "Input cannot be blank... Try filling out the input :)");
            return;
        }

        data.NewGame(input);
        RefreshSaves();
    }

    private void LoadSelectedWorld()
    {
        GD.Print("WorldManager: loading selected world");
        GameManager.Singleton.SetGameState(GameManager.GameState.InGame);
        _wmMain.ShowWindow(_winHud);
        data.Load(_currentlySelected.First().Title);
    }

    public void ListedWorldClicked(WorldSaveTitle wst)
    {
        var currentState = wst.GetButtonState();
        GD.Print($"ListedWorldClicked: {wst.Title} received with state {currentState}");
        if (currentState == false) _currentlySelected.Remove(wst);
        if (currentState) _currentlySelected.Add(wst);

        if (_currentlySelected.Count == 0 || _currentlySelected.Count > 1)
            _loadBtn.Disabled = true;
        else if (_currentlySelected.Count == 1)
            _loadBtn.Disabled = false;
        else
            GD.PushError("WorldManger: invalid amount in list...");
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