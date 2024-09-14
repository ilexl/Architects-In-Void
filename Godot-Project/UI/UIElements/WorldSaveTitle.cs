using Godot;

namespace ArchitectsInVoid.UI.UIElements;

public partial class WorldSaveTitle : AspectRatioContainer
{
    [Export] private TextureButton _btn;
    [Export] public string Date;
    [Export] public string Title;

    private WorldManager _worldManager;

    [Export] private RichTextLabel _worldName, _saveDate;

    public override void _Ready()
    {
        if (_worldName == null || _saveDate == null)
        {
            _worldName = (RichTextLabel)FindChild("WorldName");
            _saveDate = (RichTextLabel)FindChild("SaveDate");
        }

        if (_worldName == null || _saveDate == null)
        {
            GD.PushError("WorldSaveTitle: missing text labels...");
            return;
        }

        if (_btn == null) _btn = (TextureButton)FindChild("TextureButton");
        if (_btn == null) GD.PushError("WorldSaveTitle: missing button...");
    }

    public void BindButtonToManager(WorldManager manager)
    {
        _worldManager = manager;
        _btn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(ButtonPressed));
    }

    public bool GetButtonState()
    {
        return !_btn.ButtonPressed;
    }

    private void ButtonPressed()
    {
        _worldManager.ListedWorldClicked(this);
    }

    public void UpdateWorldSaveTitle(string title, string date)
    {
        Title = title;
        Date = date;
        UpdateWorldSaveTitle();
    }

    public void UpdateWorldSaveTitle()
    {
        if (_worldName == null || _saveDate == null)
        {
            GD.PushError("WorldSaveTitle: missing text labels...");
            return;
        }

        _worldName.Text = Title;
        _saveDate.Text = Date;
    }
}