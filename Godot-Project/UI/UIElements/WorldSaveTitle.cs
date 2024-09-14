using Godot;
using System;

public partial class WorldSaveTitle : AspectRatioContainer
{
	[Export] public string title;
	[Export] public string date;

	[Export] RichTextLabel worldName, saveDate;
    [Export] TextureButton btn;

    WorldManager worldManager;
    public override void _Ready()
    {
        if(worldName == null || saveDate == null)
		{
            worldName = (RichTextLabel)FindChild("WorldName");
            saveDate = (RichTextLabel)FindChild("SaveDate");
        }
        if (worldName == null || saveDate == null)
        {
            GD.PushError("WorldSaveTitle: missing text labels...");
            return;
        }
        if(btn == null)
        {
            btn = (TextureButton)FindChild("TextureButton", recursive: true);
        }
        if (btn == null)
        {
            GD.PushError("WorldSaveTitle: missing button...");
            return;
        }
    }

    public void BindButtonToManager(WorldManager manager)
    {
        worldManager = manager;
        btn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(ButtonPressed));
    }

    public bool GetButtonState()
    {
        return !btn.ButtonPressed;
    }

    void ButtonPressed()
    {
        worldManager.ListedWorldClicked(this);
    }
    public void UpdateWorldSaveTitle(string _title, string _date)
    {
        title = _title;
        date = _date;
        UpdateWorldSaveTitle();
    }
    public void UpdateWorldSaveTitle()
    {
        if (worldName == null || saveDate == null)
        {
            GD.PushError("WorldSaveTitle: missing text labels...");
            return;
        }

        worldName.Text = title;
        saveDate.Text = date;
    }
}
