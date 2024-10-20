using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchitectsInVoid.Settings;

[Tool]
public partial class Settings : Node
{
    [ExportGroup("General", "")]
    [Export] public Language _gameLanguage;

    // controls
    public List<Control> savedControls;

    [ExportGroup("Audio", "")]
    [Export] public double _masterVolume;
    [Export] public double _effectsVolume;
    [Export] public double _musicVolume;
    [Export] public double _dialougeVolume;
    [Export] public Language _spokenLanguage;
    [Export] public bool _subtitles;
    [Export] public Language _subtitlesLanguage;

    [ExportGroup("Display", "")]
    [Export] public Vector2I _resolution;
    [Export] public double _refreshRate;
    [Export] public DisplayMode _displayMode;
    [Export] public bool _vsync;

    public Vector2I[] SUPPORTED_RESOLUTIONS = { new Vector2I(1024, 768),
                                                new Vector2I(1280, 720),
                                                new Vector2I(1280, 800),
                                                new Vector2I(1280, 1024),
                                                new Vector2I(1366, 768),
                                                new Vector2I(1600, 900),
                                                new Vector2I(1680, 1050),
                                                new Vector2I(1440, 900),
                                                new Vector2I(1920, 1080),
                                                new Vector2I(2560, 1440),
                                                new Vector2I(3840, 2160)};
    public enum Language
    {
        English = 0, // currently only support english - plan to add more here at some point
        Mandarin = 1,
        Spanish = 2,
        Hindi = 3,
        Russian = 4,
        Japanese = 5,
        Korean = 6,
        French = 7, 
        German = 8,
        Italian = 9
    };

    public struct Control
    {
        public string InputMapName;
        public InputEvent inputEventPrimary;
        public InputEvent inputEventSecondary;
    }

    public override void _Ready()
    {
        base._Ready();
        LoadSettings();
        ApplyCurrentSettings();
    }

    public string LanguageToString(Language language)
    {
        switch (language)
        {
            case Language.English:
                return "English";
            case Language.Mandarin:
                return "Mandarin";
            case Language.Spanish:
                return "Spanish";
            case Language.Hindi:
                return "Hindi";
            case Language.Russian:
                return "Russian";
            case Language.Japanese:
                return "Japanese";
            case Language.Korean:
                return "Korean";
            case Language.French:
                return "French";
            case Language.German:
                return "German";
            case Language.Italian:
                return "Italian";
            default:
                GD.Print("Settings: language invalid - likely null data");
                return "NULL";
        }
    }

    public void ApplyCurrentSettings()
    {
        ApplyScreenSettings();
        // nothing else to "apply" yet
    }

    #region FileStuff
    string GetSavePath()
    {
        return "user://" + "application/";
    }
    public void SaveSettings()
    {
        GD.Print("Settings: saving to file...");
        DirAccess.MakeDirRecursiveAbsolute(GetSavePath());
        string _name = "settings";
        var file = FileAccess.Open($"{GetSavePath()}{_name}.dat", FileAccess.ModeFlags.Write);

        // file .StoreVar
        // save file stuff here

        // Audio
        file.StoreVar("Audio");
        file.StoreVar(_masterVolume);
        file.StoreVar(_effectsVolume);
        file.StoreVar(_musicVolume);
        file.StoreVar(_dialougeVolume);
        file.StoreVar((int)_spokenLanguage);
        file.StoreVar(_subtitles);
        file.StoreVar((int)_subtitlesLanguage);

        // Display
        file.StoreVar("Display");
        file.StoreVar(_resolution);
        file.StoreVar(_refreshRate);
        file.StoreVar((int)_displayMode);
        file.StoreVar(_vsync);

        // General
        file.StoreVar("General");
        file.StoreVar((int)_gameLanguage);

        // TODO: add controls here

        file.Close(); // must be called or else creates a bunch of .tmp files

        SaveControls();
    }

    public void SaveControls()
    {
        GD.Print("Settings: saving controls to file...");
        DirAccess.MakeDirRecursiveAbsolute(GetSavePath());
        string _name = "controls";
        var file = FileAccess.Open($"{GetSavePath()}{_name}.dat", FileAccess.ModeFlags.Write);
        int amount = savedControls.Count;
        file.StoreVar(amount);
        foreach(var control in savedControls)
        {
            file.StoreVar(control.InputMapName);
            SaveKey(file, control.inputEventPrimary);
            SaveKey(file, control.inputEventSecondary);
        }
        file.Close();
    }

    void SaveKey(FileAccess file, InputEvent ie)
    {
        if(ie == null)
        {
            file.StoreVar(-1);
        }
        else if (ie is InputEventMouseButton iemb)
        {
            file.StoreVar(0);
            file.StoreVar(ie.Device);
            file.StoreVar((long)iemb.ButtonIndex);
        }
        else if (ie is InputEventJoypadButton iejb)
        {
            file.StoreVar(1);
            file.StoreVar(ie.Device);
            file.StoreVar((long)iejb.ButtonIndex);

        }
        else if (ie is InputEventKey iek)
        {
            file.StoreVar(2);
            file.StoreVar(ie.Device);
            file.StoreVar((long)iek.Keycode);
            file.StoreVar((long)iek.PhysicalKeycode);
            file.StoreVar((long)iek.Unicode);
        }
        else
        {
            file.StoreVar(-1);
            GD.PushError("Settings: invalid InputEvent type...");
        }
    }

    InputEvent LoadKey(FileAccess file)
    {
        int type = file.GetVar().AsInt32();
        InputEvent output = null;
        if(type == -1)
        {
            return output;
        }
        if (type == 0) // InputEventMouseButton
        {
            var o = new InputEventMouseButton();
            o.Device = file.GetVar().AsInt32();
            o.ButtonIndex = (MouseButton)file.GetVar().AsInt64();
            output = o;
        }
        if (type == 1) // InputEventJoypadButton
        {
            var o = new InputEventJoypadButton();
            o.Device = file.GetVar().AsInt32();
            o.ButtonIndex = (JoyButton)file.GetVar().AsInt64();
            output = o;

        }
        if (type == 2) // InputEventKey
        {
            var o = new InputEventKey();
            o.Device = file.GetVar().AsInt32();
            o.Keycode = (Key)file.GetVar().AsInt64();
            o.PhysicalKeycode = (Key)file.GetVar().AsInt64();
            o.Unicode = file.GetVar().AsInt64();
            output = o;

        }
        return output;
    }
    

    public void DefaultSettings()
    {
        GD.Print("Settings: setting default settings");
        _gameLanguage = Language.English;
        _masterVolume = 100d;
        _effectsVolume = 100d;
        _musicVolume = 100d;
        _dialougeVolume = 100d;
        _spokenLanguage = Language.English;
        _subtitles = true;
        _subtitlesLanguage = Language.English;
        _resolution = SUPPORTED_RESOLUTIONS[0];
        _refreshRate = 60;
        _displayMode = DisplayMode.Windowed;
        _vsync = false;
        GD.Print("Settings: default settings set");

        DefaultControls();
        SaveSettings();
    }

    private void DefaultControls()
    {
        GD.Print("Settings: default controls to file...");
        DirAccess.MakeDirRecursiveAbsolute(GetSavePath());
        savedControls = new();
        string _name = "controls";
        var file = FileAccess.Open($"{GetSavePath()}{_name}.dat", FileAccess.ModeFlags.Write);
        int zero = 0;
        file.StoreVar(zero); // store 0 to show there are no saved controls only default set in godot
        file.Close();
    }

    public void LoadSettings()
    {
        GD.Print("Settings: loading settings from file...");
        DirAccess.MakeDirRecursiveAbsolute(GetSavePath());
        string _name = "settings";
        if (!FileAccess.FileExists($"{GetSavePath()}{_name}.dat"))
        {
            GD.Print("Settings: no file exists... creating one with default settings");
            DefaultSettings(); // load default settings and save to file
            return; // no need to continue loading as default will load
        }
        var file = FileAccess.Open($"{GetSavePath()}{_name}.dat", FileAccess.ModeFlags.Read);

        // file .GetVar() .AsSomething()
        // load file stuff here

        // Audio
        if(file.GetVar().AsString() != "Audio")
        {
            file.Close();
            GD.PushError("Settings: loaded settings failed... file incorrect...");
            return;
        }
        _masterVolume = file.GetVar().AsDouble();
        _effectsVolume = file.GetVar().AsDouble();
        _musicVolume = file.GetVar().AsDouble();
        _dialougeVolume = file.GetVar().AsDouble();
        _spokenLanguage = (Language)file.GetVar().AsInt32();
        _subtitles = file.GetVar().AsBool();
        _subtitlesLanguage = (Language)file.GetVar().AsInt32();

        // Display
        if (file.GetVar().AsString() != "Display")
        {
            file.Close();
            GD.PushError("Settings: loaded settings failed... file incorrect...");
            return;
        }
        _resolution = file.GetVar().AsVector2I();
        _refreshRate = file.GetVar().AsInt32();
        _displayMode = (DisplayMode)file.GetVar().AsInt32();
        _vsync = file.GetVar().AsBool();

        // General
        if (file.GetVar().AsString() != "General")
        {
            file.Close();
            GD.PushError("Settings: loaded settings failed... file incorrect...");
            return;
        }
        _gameLanguage = (Language)file.GetVar().AsInt32();

        file.Close(); // must be called or else creates a bunch of .tmp files

        LoadControls();
    }

    private void LoadControls()
    {
        GD.Print("Settings: loading controls from file...");
        DirAccess.MakeDirRecursiveAbsolute(GetSavePath());
        string _name = "controls";
        if (!FileAccess.FileExists($"{GetSavePath()}{_name}.dat"))
        {
            GD.Print("Settings: no file exists... creating one with default settings");
            DefaultControls(); // load default settings and save to file
            return; // no need to continue loading as default will load
        }
        var file = FileAccess.Open($"{GetSavePath()}{_name}.dat", FileAccess.ModeFlags.Read);

        savedControls = new List<Control>();

        // file .GetVar() .AsSomething()
        // load file stuff here

        // get amount as int
        int amount = file.GetVar().AsInt32();

        // loop amount
        for (; amount > 0; amount--)
        {
            Control c = new Control();
            c.InputMapName = file.GetVar().AsString();
            c.inputEventPrimary = LoadKey(file);
            c.inputEventSecondary = LoadKey(file);
            savedControls.Add(c);
        }

        file.Close(); // must be called or else creates a bunch of .tmp files
    }
    #endregion

    #region DisplayMode
    public enum DisplayMode
    {
        Fullscreen_Exclusive = 0,
        Fullscreen_Borderless = 1,
        Windowed = 2
    }
    public string DisplayModeToString(DisplayMode mode)
    {
        switch (mode)
        {
            case DisplayMode.Fullscreen_Exclusive:
                {
                    return "Fullscreen (Exclusive)";
                }
            case DisplayMode.Fullscreen_Borderless:
                {
                    return "Fullscreen (Borderless)";
                }
            case DisplayMode.Windowed:
                {
                    return "         Windowed       ";
                }
            default:
                {
                    GD.Print("Settings: display mode invalid - likely null data");
                    return "NULL";
                }
        }
    }
    void SetDisplayMode(DisplayMode mode)
    {
        switch (mode)
        {
            case DisplayMode.Fullscreen_Exclusive:
            {
                GD.Print("Settings: fullscreen exclusive applied");
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                break;
            }
            case DisplayMode.Fullscreen_Borderless:
            {
                GD.Print("Settings: fullscreen borderless applied");
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
                break;
            }
            case DisplayMode.Windowed:
            {
                GD.Print("Settings: windowed applied");
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                break;
            }
            default:
            {
                GD.PushError("Settings: no such display mode | it could also be null?");
                break;
            }
        }
    }
    void SetVSync(bool sync)
    {
        if (sync)
        {
            DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
        }
        else
        {
            DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
        }
    }

    void ApplyScreenSettings()
    {
        if (Engine.IsEditorHint()) { return; }
        SetDisplayMode(_displayMode);
        SetVSync(_vsync);
        DisplayServer.WindowSetSize(_resolution);
        Engine.Singleton.MaxFps = (int)System.Math.Round(_refreshRate, 0, System.MidpointRounding.AwayFromZero);
        Engine.Singleton.PhysicsJitterFix = 0;
        if (_displayMode == DisplayMode.Windowed)
        {
            var centered = new Vector2I((DisplayServer.WindowGetSize().X / 2) - (_resolution.X / 4), (DisplayServer.WindowGetSize().Y / 2) - (_resolution.Y / 4));
            DisplayServer.WindowSetPosition(centered);
        }
    }
    #endregion
}