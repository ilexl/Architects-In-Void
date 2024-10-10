using Godot;
using System.Collections.Generic;
using System.Linq;

namespace ArchitectsInVoid.Settings;

[Tool]
public partial class Settings : Node
{
    [ExportGroup("General", "")]
    [Export] public Language _gameLanguage;

    [ExportGroup("Controls", "")]
    [Export] bool temp2;

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

        // Controls
        file.StoreVar("Controls");

        // TODO: add controls here

        file.Close(); // must be called or else creates a bunch of .tmp files
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
        SaveSettings();
    }
    public void LoadSettings()
    {
        GD.Print("Settings: loading from file...");
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

        // Controls
        if (file.GetVar().AsString() != "Controls")
        {
            file.Close();
            GD.PushError("Settings: loaded settings failed... file incorrect...");
            return;
        }
        // TODO: add controls here

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
        SetDisplayMode(_displayMode);
        SetVSync(_vsync);
        DisplayServer.WindowSetSize(_resolution);
        Engine.Singleton.MaxFps = (int)System.Math.Round(_refreshRate, 0, System.MidpointRounding.AwayFromZero);
        Engine.Singleton.PhysicsJitterFix = 0;
    }
    #endregion
}