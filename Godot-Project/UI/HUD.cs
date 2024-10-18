using Godot;
using System.Reflection.Metadata;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class HUD : Node
{
    [ExportGroup("ItemSlots")]
    [Export] Control[] _itemSlots;
    [Export] Control[] _itemSlotSelections;
    [Export] Control[] _itemSlotIcons;
    [ExportGroup("HotbarSelection")]
    [Export] Control[] _hotbarSelections;
    [Export] Control[] _hotbarSelectionEnabled;
    int _currentItemSlot, _currentHotbar;
    [ExportGroup("LeftInfo")]
    [Export] TextureButton _infoLeftOpen, _infoLeftClosed;
    [Export] TextureProgressBar _tpbOxygen, _tpbEnergy, _tpbFuel, _tpbHealth;
    [ExportGroup("RightInfo")]
    [Export] TextureButton _infoRightOpen, _infoRightClosed;
    [Export] TextureButton _dampenersToggle, _autorefToggle;
    [Export] Texture2D _buttonOnTexture, _buttonOffTexture;
    [Export] RichTextLabel _relativeToObjectNameTxt, _mpsTxt, _mpspsTxt;
    bool _dampeners, _autoref;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if(_itemSlots.Length != 10 || _itemSlotSelections.Length != 10 || _itemSlotIcons.Length != 10)
        {
            GD.PushError("HUD: item slots not configured correctly in editor...");
        }
        if (_hotbarSelections.Length != 10 || _hotbarSelectionEnabled.Length != 10)
        {
            GD.PushError("HUD: hotbar selection not configured correctly in editor...");
        }

        if(_infoLeftOpen == null || _infoLeftClosed == null)
        {
            GD.PushError("HUD: missing texture buttons...");
            return;
        }
        if (_infoRightOpen == null || _infoRightClosed == null)
        {
            GD.PushError("HUD: missing texture buttons...");
            return;
        }

        if (!_infoLeftOpen.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(CloseLeftInfo)))
        {
            _infoLeftOpen.Connect(BaseButton.SignalName.ButtonDown, Callable.From(CloseLeftInfo));
        }
        if (!_infoLeftClosed.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(OpenLeftInfo)))
        {
            _infoLeftClosed.Connect(BaseButton.SignalName.ButtonDown, Callable.From(OpenLeftInfo));
        }
        if (!_infoRightOpen.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(CloseRightInfo)))
        {
            _infoRightOpen.Connect(BaseButton.SignalName.ButtonDown, Callable.From(CloseRightInfo));
        }
        if (!_infoRightClosed.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(OpenRightInfo)))
        {
            _infoRightClosed.Connect(BaseButton.SignalName.ButtonDown, Callable.From(OpenRightInfo));
        }

        SelectItemSlot(0);
        SelectHotbar(1);
        OpenLeftInfo();
        OpenRightInfo();
        AutorefOn();
        DampenersOn();
    }

    void DampenersToggle()
    {
        if (_dampeners)
        {
            DampenersOff();
        }
        else
        {
            DampenersOn();
        }
    }

    public void DampenersOff()
    {
        GD.Print("HUD: dampeners toggled off");
        _dampeners = false;
        _dampenersToggle.TextureNormal = _buttonOffTexture;
        RichTextLabel rtl = _dampenersToggle.GetChild(0) as RichTextLabel;
        rtl.RemoveThemeColorOverride("default_color");
        rtl.AddThemeColorOverride("default_color", Color.Color8(112, 112, 112, 255));
    }

    public void DampenersOn()
    {
        GD.Print("HUD: dampeners toggled on");
        _dampeners = true;
        _dampenersToggle.TextureNormal = _buttonOnTexture;
        RichTextLabel rtl = _dampenersToggle.GetChild(0) as RichTextLabel;
        rtl.RemoveThemeColorOverride("default_color");
        rtl.AddThemeColorOverride("default_color", Color.Color8(255, 255, 255, 255));
    }

    void _AutorefToggle()
    {
        if (_autoref)
        {
            AutorefOff();
        }
        else
        {
            AutorefOn();
        }
    }

    public void AutorefOff()
    {
        GD.Print("HUD: autoref toggled off");
        _autoref = false;
        _autorefToggle.TextureNormal = _buttonOffTexture;
        RichTextLabel rtl = _autorefToggle.GetChild(0) as RichTextLabel;
        rtl.RemoveThemeColorOverride("default_color");
        rtl.AddThemeColorOverride("default_color", Color.Color8(112, 112, 112, 255));
    }

    public void AutorefOn()
    {
        GD.Print("HUD: autoref toggled on");
        _autoref = true;
        _autorefToggle.TextureNormal = _buttonOnTexture;
        RichTextLabel rtl = _autorefToggle.GetChild(0) as RichTextLabel;
        rtl.RemoveThemeColorOverride("default_color");
        rtl.AddThemeColorOverride("default_color", Color.Color8(255, 255, 255, 255));
    }

    public void SetRelativeToObjectText(string text)
    {
        GD.Print($"HUD: setting RelativeToObject text to {text}");
        _relativeToObjectNameTxt.Text = text;
    }

    public void SetMPSText(string text)
    {
        GD.Print($"HUD: setting MPS text to {text}");
        _mpsTxt.Text = text;
    }

    public void SetMPSPSText(string text)
    {
        GD.Print($"HUD: setting MPSPS text to {text}");
        _mpspsTxt.Text = text;
    }

    void OpenLeftInfo()
    {
        GD.Print("HUD: opening left info");
        _infoLeftOpen.Show();
        _infoLeftClosed.Hide();
    }

    void CloseLeftInfo()
    {
        GD.Print("HUD: closing left info");
        _infoLeftOpen.Hide();
        _infoLeftClosed.Show();
    }

    void OpenRightInfo()
    {
        GD.Print("HUD: opening right info");
        _infoRightOpen.Show();
        _infoRightClosed.Hide();
    }

    void CloseRightInfo()
    {
        GD.Print("HUD: closing right info");
        _infoRightOpen.Hide();
        _infoRightClosed.Show();
    }

    public void SetInfoOxygen(float value)
    {
        GD.Print($"HUD: set oxygen to {value}");
        _tpbOxygen.Value = value;
    }
    public void SetInfoEnergy(float value)
    {
        GD.Print($"HUD: set energy to {value}");
        _tpbEnergy.Value = value;
    }
    public void SetInfoFuel(float value)
    {
        GD.Print($"HUD: set fuel to {value}");
        _tpbFuel.Value = value;
    }
    public void SetInfoHealth(float value)
    {
        GD.Print($"HUD: set health to {value}");
        _tpbHealth.Value = value;
    }

    public int GetHotbar()
    {
        return _currentHotbar;
    }
    public int GetItemSlot()
    {
        return _currentItemSlot;
    }

    public void SelectItemSlot(int slot)
    {
        _currentItemSlot = slot;
        if (_itemSlotSelections == null || _itemSlotSelections.Length != 10) { return; }
        foreach(var item in _itemSlotSelections)
        {
            item.Visible = false;
        }
        _itemSlotSelections[slot].Visible = true;
    }

    public void SelectHotbar(int hotbar)
    {
        _currentHotbar = hotbar;
        if (_hotbarSelectionEnabled == null || _hotbarSelectionEnabled.Length != 10) { return; }
        foreach (var item in _hotbarSelectionEnabled)
        {
            item.Visible = false;
        }
        _hotbarSelectionEnabled[hotbar].Visible = true;
    }

    public void SelectHotbar(bool increase)
    {
        if (increase)
        {
            if(_currentHotbar == 9 || _currentHotbar > 9)
            {
                SelectHotbar(0);
                return;
            }
            else
            {
                SelectHotbar(_currentHotbar + 1);
                return;
            }
        }
        else
        {
            if (_currentHotbar == 0 || _currentHotbar < 0)
            {
                SelectHotbar(9);
                return;
            }
            else
            {
                SelectHotbar(_currentHotbar - 1);
                return;
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(GameManager.Singleton == null)
        {
            return;
        }
        if(GameManager.Singleton.CurrentGameState == GameManager.GameState.InGame)
        {
            for(int i = 0; i != 10; i++)
            {
                string inputName = $"toolbar_equip_slot_{i.ToString()}";
                if (@event.IsActionPressed(inputName))
                {
                    SelectItemSlot(i);
                }
            }
            if (@event.IsActionPressed("toolbar_next_toolbar"))
            {
                SelectHotbar(true);
            }
            if (@event.IsActionPressed("toolbar_previous_toolbar"))
            {
                SelectHotbar(false);
            }
        }
    }
}