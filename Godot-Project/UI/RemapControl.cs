using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[Tool]
public partial class RemapControl : Node
{
	[Export] public string InputMapName;
	[Export] public string DisplayName;
    [Export] TextureButton _primaryRemapBtn, _secondaryRemapBtn;
	[Export] RichTextLabel _title, _primaryRemapTxt, _secondaryRemapTxt;
    InputEvent _primaryIE, _secondaryIE;
    string _strprimaryIE, _strsecondaryIE;
    UIManager _uim;
    InfoPopUp _infoPopUp;

    bool remapPrimary, remapSecondary;

    public override void _Ready()
    {
        remapPrimary = false;
        remapSecondary = false;
        if (!_primaryRemapBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(RemapPrimary)))
        {
            _primaryRemapBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(RemapPrimary));
        }
        if (!_secondaryRemapBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(RemapSecondary)))
        {
            _secondaryRemapBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(RemapSecondary));
        }
    }

    void RemapPrimary()
    {
        if(Input.IsMouseButtonPressed(MouseButton.Right))
        {
            _primaryIE = null;
            SaveCurrent();
            DisplayCurrent();
            return;
        }
        remapPrimary = true;
        _infoPopUp = _uim._popup.DisplayInfoPopUpNC("Press any key/button...");
    }

    void RemapSecondary()
    {
        if (Input.IsMouseButtonPressed(MouseButton.Right))
        {
            _secondaryIE = null;
            SaveCurrent();
            DisplayCurrent();
            return;
        }
        remapSecondary = true;
        _infoPopUp = _uim._popup.DisplayInfoPopUpNC("Press any key/button...");
    }
    public override void _Input(InputEvent @event)
    {
        if (remapPrimary || remapSecondary)
        {
            if (@event is InputEventKey eventKey)
            {
                if (eventKey.Pressed)
                {
                    if (remapPrimary)
                    {
                        if(InputEventToString(eventKey) == InputEventToString(_secondaryIE))
                        {
                            _secondaryIE = null;
                        }
                        _primaryIE = eventKey;
                        remapPrimary = false;
                        remapSecondary = false;
                        _infoPopUp.Close();
                        _infoPopUp = null;
                        SaveCurrent();
                        DisplayCurrent();
                    }
                    if (remapSecondary)
                    {
                        if (InputEventToString(eventKey) == InputEventToString(_primaryIE))
                        {
                            _primaryIE = null;
                        }
                        _secondaryIE = eventKey;
                        remapPrimary = false;
                        remapSecondary = false;
                        _infoPopUp.Close();
                        _infoPopUp = null;
                        SaveCurrent();
                        DisplayCurrent();
                    }
                }
            }
            if (@event is InputEventMouseButton eventMouse)
            {
                if (eventMouse.Pressed)
                {
                    if (remapPrimary)
                    {
                        if (InputEventToString(eventMouse) == InputEventToString(_secondaryIE))
                        {
                            _secondaryIE = null;
                        }
                        _primaryIE = eventMouse;
                        remapPrimary = false;
                        remapSecondary = false;
                        _infoPopUp.Close();
                        _infoPopUp = null;
                        SaveCurrent();
                        DisplayCurrent();
                    }
                    if (remapSecondary)
                    {
                        if (InputEventToString(eventMouse) == InputEventToString(_primaryIE))
                        {
                            _primaryIE = null;
                        }
                        _secondaryIE = eventMouse;
                        remapPrimary = false;
                        remapSecondary = false;
                        _infoPopUp.Close();
                        _infoPopUp = null;
                        SaveCurrent();
                        DisplayCurrent();
                    }
                }
            }
            if (@event is InputEventJoypadButton eventJoy)
            {
                if (eventJoy.Pressed)
                {
                    if (remapPrimary)
                    {
                        if (InputEventToString(eventJoy) == InputEventToString(_secondaryIE))
                        {
                            _secondaryIE = null;
                        }
                        _primaryIE = eventJoy;
                        remapPrimary = false;
                        remapSecondary = false;
                        _infoPopUp.Close();
                        _infoPopUp = null;
                        SaveCurrent();
                        DisplayCurrent();
                    }
                    if (remapSecondary)
                    {
                        if (InputEventToString(eventJoy) == InputEventToString(_primaryIE))
                        {
                            _primaryIE = null;
                        }
                        _secondaryIE = eventJoy;
                        remapPrimary = false;
                        remapSecondary = false;
                        _infoPopUp.Close();
                        _infoPopUp = null;
                        SaveCurrent();
                        DisplayCurrent();
                    }
                }
            }
        }
    }

    public void DisplayCurrent()
	{
        if (DisplayName == "" || DisplayName == null) { DisplayName = InputMapName; }
        _title.Text = DisplayName;

        _strprimaryIE = InputEventToString(_primaryIE);
        _strsecondaryIE = InputEventToString(_secondaryIE);

        GD.Print($"RemapControl: {InputMapName} PRIMARY  : {_strprimaryIE}");
        _primaryRemapTxt.Text = _strprimaryIE;

        GD.Print($"RemapControl: {InputMapName} SECONDARY: {_strsecondaryIE}");
        _secondaryRemapTxt.Text = _strsecondaryIE;
    }

    public void LoadControlData()
    {
        int isValid = InputMap.GetActions().IndexOf(InputMapName);
        if (isValid == -1)
        {
            //GD.Print(InputMap.GetActions());
            GD.PushError($"RemapControl: {InputMapName} is not a valid Action in the InputMap...");
            return;
        }
        // try load from settings
        bool settingsFound = TryGetSettings();
        if (!settingsFound) // not found
        {
            GD.PushError("RemapControl: cant find settings...");
            return;
        }

        bool controlHasSavedData = false;
        if (_uim._settings.savedControls != null)
        {
            foreach (var c in _uim._settings.savedControls)
            {
                if (c.InputMapName == InputMapName)
                {
                    controlHasSavedData = true;
                    InputMap.ActionEraseEvents(InputMapName);
                    _primaryIE = c.inputEventPrimary;
                    _secondaryIE = c.inputEventSecondary;
                    if(_primaryIE != null)
                    {
                        InputMap.ActionAddEvent(InputMapName, _primaryIE);
                    }
                    if(_secondaryIE != null)
                    {
                        InputMap.ActionAddEvent(InputMapName, _secondaryIE);
                    }
                }
            }
        }


        // if no settings load default here
        ForceMaxInputs(InputMapName); // forces max of 2 input actions for primary | secondary
        Array<InputEvent> e = InputMap.ActionGetEvents(InputMapName);

        if (controlHasSavedData)
        {
            _strprimaryIE = InputEventToString(_primaryIE);
            _strsecondaryIE = InputEventToString(_secondaryIE);
        }
        else
        {
            _primaryIE = null;
            _secondaryIE = null;

            foreach (InputEvent ev in e)
            {
                if (ev is InputEventKey || ev is InputEventMouseButton)
                {
                    if (_primaryIE == null)
                    {
                        _primaryIE = ev;
                        _strprimaryIE = InputEventToString(ev);
                    }
                    else if (_secondaryIE == null)
                    {
                        _secondaryIE = ev;
                        _strsecondaryIE = InputEventToString(ev);
                    }
                    else
                    {
                        GD.PushError("RemapControl: forced inputs includes more than 2 inputs...");
                        return;
                    }
                }
                if (ev is InputEventJoypadButton)
                {
                    GD.PushError("RemapControl: forced inputs includes a joypad control (NOT IMPLEMENTED)...");
                    return;
                }
            }
        }
    }

    public void SaveCurrent()
    {
        DisplayCurrent();
        if (_uim._settings.savedControls == null)
        {
            _uim._settings.savedControls = new List<ArchitectsInVoid.Settings.Settings.Control>();
            var c = new ArchitectsInVoid.Settings.Settings.Control();
            c.InputMapName = InputMapName;
            c.inputEventPrimary = _primaryIE;
            c.inputEventSecondary = _secondaryIE;
            _uim._settings.savedControls.Add(c);
        }
        else
        {
            bool found = false;
            foreach(var c in _uim._settings.savedControls)
            {
                if(c.InputMapName == InputMapName)
                {
                    found = true;
                    _uim._settings.savedControls.Remove(c);
                    var cn = new ArchitectsInVoid.Settings.Settings.Control();
                    cn.InputMapName = InputMapName;
                    cn.inputEventPrimary = _primaryIE;
                    cn.inputEventSecondary = _secondaryIE;
                    _uim._settings.savedControls.Add(cn);
                    break;
                }
            }
            if (!found)
            {
                var cn = new ArchitectsInVoid.Settings.Settings.Control();
                cn.InputMapName = InputMapName;
                cn.inputEventPrimary = _primaryIE;
                cn.inputEventSecondary = _secondaryIE;
                _uim._settings.savedControls.Add(cn);
            }
        }
        LoadControlData();
    }

    private static void ForceMaxInputs(string inputMapName)
    {
        Array<InputEvent> e = InputMap.ActionGetEvents(inputMapName);
        int amount = 0;
        foreach(InputEvent ev in e)
        {
            if (ev is InputEventKey iek || ev is InputEventMouseButton)
            {
                amount++;
                if(amount > 2)
                {
                    InputMap.ActionEraseEvent(inputMapName, ev);
                }
            }
            if (ev is InputEventJoypadButton)
            {
                GD.PushWarning("Controller Support Not Implemented Yet...");
                InputMap.ActionEraseEvent(inputMapName, ev);
            }
        }
    }

    private static string InputEventToString(InputEvent ev)
    {
        if(ev == null)
        {
            return "None";
        }
        if (ev is InputEventKey iek)
        {
            return iek.AsTextPhysicalKeycode();

        }
        if (ev is InputEventMouseButton iemb)
        {
            return $"Mouse {iemb.ButtonIndex}";
        }
        if (ev is InputEventJoypadButton)
        {
            GD.PushError("RemapControl: forced inputs includes a joypad control (NOT IMPLEMENTED)...");
            return "NOT IMPLEMENTED";
        }
        GD.PushError("RemapControl: ie to string no valid type found...");
        return "INVALID";
    }

    bool TryGetSettings()
    {
        var target = GameManager.Singleton.FindChild("UI");
        var uim = (UIManager)target;
        if(uim != null)
        {
            _uim = uim;
            return true;
        }
        return false;
    }

    internal void Reset()
    {
        //GD.Print("RemapControl: reset called");
        InputMap.ActionEraseEvents(InputMapName);
        var test = ProjectSettings.GetSetting($"input/{InputMapName}");
        //GD.Print(test);
        Array<InputEvent> e = (Array<InputEvent>)test.AsGodotDictionary().GetValueOrDefault("events");

        _primaryIE = null;
        _secondaryIE = null;

        foreach (InputEvent ev in e)
        {
            if (ev is InputEventKey || ev is InputEventMouseButton)
            {
                if (_primaryIE == null)
                {
                    _primaryIE = ev;
                    _strprimaryIE = InputEventToString(ev);
                }
                else if (_secondaryIE == null)
                {
                    _secondaryIE = ev;
                    _strsecondaryIE = InputEventToString(ev);
                }
                else
                {
                    GD.PushError("RemapControl: reset inputs includes more than 2 inputs...");
                    return;
                }
            }
            if (ev is InputEventJoypadButton)
            {
                GD.PushError("RemapControl: reset inputs includes a joypad control (NOT IMPLEMENTED)...");
                return;
            }
        }
        if (_primaryIE != null)
        {
            InputMap.ActionAddEvent(InputMapName, _primaryIE);
        }
        if (_secondaryIE != null)
        {
            InputMap.ActionAddEvent(InputMapName, _secondaryIE);
        }
    }
}
