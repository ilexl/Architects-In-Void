using Godot;
using System.Reflection.Metadata;
using ArchitectsInVoid.Player.HotBar;

namespace ArchitectsInVoid.UI;

/// <summary>
/// Manages the HUD visuals for the player to see
/// </summary>
[Tool]
public partial class HUD : Node
{
    #region Variables
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
    #endregion

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        if (Engine.IsEditorHint()) { return; } // do NOT run when not in game

        #region Error OR Null Checks

        if (_itemSlots.Length != 10 || _itemSlotSelections.Length != 10 || _itemSlotIcons.Length != 10)
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

        #endregion
        #region Bind Buttons

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

        #endregion
        
        // bind events
        HotBarManager.HotbarSlotChangedEvent += HotbarSlotChanged;
        HotBarManager.HotbarTextureChangedEvent += SetItemSlotIcon;
        
        // defaults shown
        SelectItemSlot(0);
        SelectHotbar(1);
        OpenLeftInfo();
        OpenRightInfo();
        AutorefOn();
        DampenersOn();
    }

    /// <summary>
    /// UI Button for toggling the dampeners
    /// <br/> NOTE: errors occured when doing this in a more practical way? DONT CHANGE THIS
    /// </summary>
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

    /// <summary>
    /// Turns dampeners button to off
    /// </summary>
    public void DampenersOff()
    {
        GD.Print("HUD: dampeners toggled off");
        _dampeners = false;
        _dampenersToggle.TextureNormal = _buttonOffTexture;
        RichTextLabel rtl = _dampenersToggle.GetChild(0) as RichTextLabel;
        rtl.RemoveThemeColorOverride("default_color");
        rtl.AddThemeColorOverride("default_color", Color.Color8(112, 112, 112, 255)); // off color
    }

    /// <summary>
    /// Turns dampeners button to on
    /// </summary>
    public void DampenersOn()
    {
        GD.Print("HUD: dampeners toggled on");
        _dampeners = true;
        _dampenersToggle.TextureNormal = _buttonOnTexture;
        RichTextLabel rtl = _dampenersToggle.GetChild(0) as RichTextLabel;
        rtl.RemoveThemeColorOverride("default_color");
        rtl.AddThemeColorOverride("default_color", Color.Color8(255, 255, 255, 255)); // on color
    }

    /// <summary>
    /// UI Button for toggling the Autoref
    /// <br/> NOTE: errors occured when doing this in a more practical way? DONT CHANGE THIS
    /// </summary>
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

    /// <summary>
    /// Turns autoref button to off
    /// </summary>
    public void AutorefOff()
    {
        GD.Print("HUD: autoref toggled off");
        _autoref = false;
        _autorefToggle.TextureNormal = _buttonOffTexture;
        RichTextLabel rtl = _autorefToggle.GetChild(0) as RichTextLabel;
        rtl.RemoveThemeColorOverride("default_color");
        rtl.AddThemeColorOverride("default_color", Color.Color8(112, 112, 112, 255)); // off color
    }

    /// <summary>
    /// Turns autoref button to on
    /// </summary>
    public void AutorefOn()
    {
        GD.Print("HUD: autoref toggled on");
        _autoref = true;
        _autorefToggle.TextureNormal = _buttonOnTexture;
        RichTextLabel rtl = _autorefToggle.GetChild(0) as RichTextLabel;
        rtl.RemoveThemeColorOverride("default_color");
        rtl.AddThemeColorOverride("default_color", Color.Color8(255, 255, 255, 255)); // on color
    }

    /// <summary>
    /// Sets the relative to object text
    /// </summary>
    public void SetRelativeToObjectText(string text)
    {
        GD.Print($"HUD: setting RelativeToObject text to {text}");
        _relativeToObjectNameTxt.Text = text;
    }

    /// <summary>
    /// Sets the meters per second text
    /// </summary>
    public void SetMPSText(string text)
    {
        GD.Print($"HUD: setting MPS text to {text}");
        _mpsTxt.Text = text;
    }

    /// <summary>
    /// Sets the meters per second per second (acceleration) text
    /// </summary>
    public void SetMPSPSText(string text)
    {
        GD.Print($"HUD: setting MPSPS text to {text}");
        _mpspsTxt.Text = text;
    }

    /// <summary>
    /// Opens the left info tab UI
    /// </summary>
    void OpenLeftInfo()
    {
        GD.Print("HUD: opening left info");
        _infoLeftOpen.Show();
        _infoLeftClosed.Hide();
    }

    /// <summary>
    /// Closes the left info tab UI
    /// </summary>
    void CloseLeftInfo()
    {
        GD.Print("HUD: closing left info");
        _infoLeftOpen.Hide();
        _infoLeftClosed.Show();
    }

    /// <summary>
    /// Opens the right info tab UI
    /// </summary>
    void OpenRightInfo()
    {
        GD.Print("HUD: opening right info");
        _infoRightOpen.Show();
        _infoRightClosed.Hide();
    }

    /// <summary>
    /// Closes the right info tab UI
    /// </summary>
    void CloseRightInfo()
    {
        GD.Print("HUD: closing right info");
        _infoRightOpen.Hide();
        _infoRightClosed.Show();
    }

    /// <summary>
    /// Sets the oxygen value
    /// <br>Hint: 0 -> 100</br>
    /// </summary>
    public void SetInfoOxygen(float value)
    {
        GD.Print($"HUD: set oxygen to {value}");
        _tpbOxygen.Value = value;
    }

    /// <summary>
    /// Sets the energy value
    /// <br>Hint: 0 -> 100</br>
    /// </summary>
    public void SetInfoEnergy(float value)
    {
        GD.Print($"HUD: set energy to {value}");
        _tpbEnergy.Value = value;
    }

    /// <summary>
    /// Sets the fuel value
    /// <br>Hint: 0 -> 100</br>
    /// </summary>
    public void SetInfoFuel(float value)
    {
        GD.Print($"HUD: set fuel to {value}");
        _tpbFuel.Value = value;
    }

    /// <summary>
    /// Sets the health value
    /// <br>Hint: 0 -> 100</br>
    /// </summary>
    public void SetInfoHealth(float value)
    {
        GD.Print($"HUD: set health to {value}");
        _tpbHealth.Value = value;
    }

    /// <summary>
    /// Returns the current hotbar index
    /// </summary>
    public int GetHotbar()
    {
        return _currentHotbar;
    }

    /// <summary>
    /// Returns the current item slot index
    /// </summary>
    public int GetItemSlot()
    {
        return _currentItemSlot;
    }

    /// <summary>
    /// Sets the current hotbar index
    /// </summary>
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

    /// <summary>
    /// Selects the hotbar index
    /// </summary>
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

    /// <summary>
    /// Selects the hotbar using increase ++ or decrease -- based on bool
    /// </summary>
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
    
    /// <summary>
    /// Current this function gets the input the manage the controls for hotbar/item control
    /// <br/> TODO: move this functionality over to main HUD management to keep this class as a visual manager not a logic manager
    /// </summary>
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
    
    // will leave this for now as i dont wish to break it :)
    /* William */
    private void HotbarSlotChanged(int index)
    {
        SelectItemSlot(index);
    }
    
    // Am I understanding this correctly?
    public void SetItemSlotIcon(Texture2D texture, int slot)
    {
        GD.Print("HUD: hotbar icon recieved");
        if(_itemSlotIcons == null || _itemSlotIcons.Length != 10) { return; }

        TextureRect icon = (TextureRect)_itemSlotIcons[slot];
        icon.Texture = texture;
    }
    /* End william */
}