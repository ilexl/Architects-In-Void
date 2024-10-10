using Godot;
using Godot.Collections;
using System.Linq;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class SettingsMenu : Node
{
    [ExportGroup("Settings-Buttons","")]
    [Export] private TextureButton _cancelBtn, _applyBtn, _resetBtn, _gameSBtn, _controlsSBtn, _audioSBtn, _displaySBtn;
    [ExportGroup("Settings-Windows", "")]
    [Export] private Window _winMainMenu, _winSubGame, _winSubControls, _winSubAudio, _winSubDisplay;
    [Export] private WindowManager _wmMain, _wmSettingSub;
    [Export] private ArchitectsInVoid.Settings.Settings _settings;
    [ExportGroup("Settings-Game", "")]
    [Export] private TextureButton _langaugeLeftBtn, _langaugeRightBtn;
    [Export] private RichTextLabel _languageDisplay;
    private ArchitectsInVoid.Settings.Settings.Language _currentGameLanguage;
    [ExportGroup("Settings-Controls", "")]
    [Export] private bool tempBool;
    [ExportGroup("Settings-Audio", "")]
    [Export] private HSlider _masterVolumeSlider, _soundeffectsVolumeSlider, _musicVolumeSlider, _dialougeVolumeSlider;
    [Export] private TextureButton _spokenLangaugeLeftBtn, _spokenLangaugeRightBtn, _subtitlesOffBtn, _subtitlesOnBtn, 
                                   _subtitlesLangaugeLeftBtn, _subtitlesLangaugeRightBtn;
    [Export] private RichTextLabel _spokenLanguageDisplay, _subtitlesLanguageDisplay;
    private ArchitectsInVoid.Settings.Settings.Language _currentSpokenLanguage, _currentSubtitlesLanguage;
    [ExportGroup("Settings-Screen", "")]
    [Export] private TextureButton _resolutionLeftBtn, _resolutionRightBtn, _refreshRateLeftBtn, _refreshRateRightBtn,
                                   _fullscreenLeftBtn, _fullscreenRightBtn, _vsyncOffBtn, _vsyncOnBtn;
    [Export] private RichTextLabel _resolutionDisplay, _refreshRateDisplay, _fullscreenDisplay;
    private Vector2I _currentResolution;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        #region Sub-Windows-Checks
        if (_wmMain == null || _wmSettingSub == null)
        {
            _wmMain = (WindowManager)GetParent().GetParent();
            _wmSettingSub = (WindowManager)GetParent().FindChild("SettingsSubMenus");
            if (_wmMain == null || _wmSettingSub == null)
            {
                GD.PushError("Settings: missing WindowManger/s...");
                return;
            }
        }
        if (_winMainMenu == null)
        {
            _winMainMenu = (Window)_wmMain.FindChild("MainMenu", false);
            if (_winMainMenu == null)
            {
                GD.PushError("Settings: missing windows...");
                return;
            }
        }
        if (_winSubGame == null || _winSubControls == null || _winSubAudio == null || _winSubDisplay == null)
        {
            _winSubGame = (Window)_wmSettingSub.FindChild("GameSettings");
            _winSubControls = (Window)_wmSettingSub.FindChild("ControlSettings");
            _winSubAudio = (Window)_wmSettingSub.FindChild("AudioSettings");
            _winSubDisplay = (Window)_wmSettingSub.FindChild("DisplaySettings");
            if (_winSubGame == null || _winSubControls == null || _winSubAudio == null || _winSubDisplay == null)
            {
                GD.PushError("Settings: missing windows...");
                return;
            }
        }
        if (_cancelBtn == null || _applyBtn == null || _resetBtn == null || _gameSBtn == null ||
            _controlsSBtn == null || _audioSBtn == null || _displaySBtn == null)
        {
            _cancelBtn = (TextureButton)GetParent().FindChild("CancelBtn");
            _applyBtn = (TextureButton)GetParent().FindChild("ApplyBtn");
            _resetBtn = (TextureButton)GetParent().FindChild("ResetSettingsBtn");
            _gameSBtn = (TextureButton)GetParent().FindChild("GameSettingsBtn");
            _controlsSBtn = (TextureButton)GetParent().FindChild("ControlsSettingsBtn");
            _audioSBtn = (TextureButton)GetParent().FindChild("AudioSettingsBtn");
            _displaySBtn = (TextureButton)GetParent().FindChild("DisplaySettingsBtn");

            if (_cancelBtn == null || _applyBtn == null || _resetBtn == null || _gameSBtn == null ||
            _controlsSBtn == null || _audioSBtn == null || _displaySBtn == null)
            {
                GD.PushError("Settings: missing buttons...");
                return;
            }
        }

        if (!_cancelBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Cancel)))
        {
            _cancelBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Cancel));
        }
        if (!_applyBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Apply)))
        {
            _applyBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Apply));
        }
        if (!_resetBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Reset)))
        {
            _resetBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Reset));
        }
        if (!_gameSBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SubGame)))
        {
            _gameSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubGame));
        }
        if (!_controlsSBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SubControls)))
        {
            _controlsSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubControls));
        }
        if (!_audioSBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SubAudio)))
        {
            _audioSBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubAudio));
        }
        if (!_displaySBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SubDisplay)))
        {
            _displaySBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubDisplay));
        }
        #endregion

        #region Sub-GameSettings-Checks
        if (_langaugeLeftBtn == null || _langaugeRightBtn == null)
        {
            _langaugeLeftBtn = (TextureButton)GetParent().FindChild("LanguageLeftBtn");
            _langaugeRightBtn = (TextureButton)GetParent().FindChild("LanguageRightBtn");
            _languageDisplay = (RichTextLabel)GetParent().FindChild("LanguageDisplayText");

            if (_langaugeLeftBtn == null || _langaugeRightBtn == null)
            {
                GD.PushError("Settings: missing buttons...");
                return;
            }
        }

        if (!_langaugeLeftBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(LanguagePrev)))
        {
            _langaugeLeftBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(LanguagePrev));
        }
        if (!_langaugeRightBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(LanguageNext)))
        {
            _langaugeRightBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(LanguageNext));
        }
        #endregion

        #region Sub-AudioSettings-Checks
        if (_masterVolumeSlider == null || _soundeffectsVolumeSlider == null || _musicVolumeSlider == null || _dialougeVolumeSlider == null || _spokenLangaugeLeftBtn == null || _spokenLangaugeRightBtn == null || _subtitlesOffBtn == null || _subtitlesOnBtn == null || _subtitlesLangaugeLeftBtn == null || _subtitlesLangaugeRightBtn == null || _spokenLanguageDisplay == null || _subtitlesLanguageDisplay == null)
        {
            _masterVolumeSlider = (HSlider)GetParent().FindChild("MasterVolumeSlider");
            _soundeffectsVolumeSlider = (HSlider)GetParent().FindChild("SoundEffectsVolumeSlider");
            _musicVolumeSlider = (HSlider)GetParent().FindChild("MusicVolumeSlider");
            _dialougeVolumeSlider = (HSlider)GetParent().FindChild("DialougeVolumeSlider");
            _spokenLangaugeLeftBtn = (TextureButton)GetParent().FindChild("LanguageDisplayText");
            _spokenLangaugeRightBtn = (TextureButton)GetParent().FindChild("LanguageDisplayText");
            _subtitlesOffBtn = (TextureButton)GetParent().FindChild("LanguageDisplayText");
            _subtitlesOnBtn = (TextureButton)GetParent().FindChild("LanguageDisplayText");
            _subtitlesLangaugeLeftBtn = (TextureButton)GetParent().FindChild("LanguageDisplayText");
            _subtitlesLangaugeRightBtn = (TextureButton)GetParent().FindChild("LanguageDisplayText");
            _spokenLanguageDisplay = (RichTextLabel)GetParent().FindChild("LanguageDisplayText");
            _subtitlesLanguageDisplay = (RichTextLabel)GetParent().FindChild("LanguageDisplayText");

            if (_masterVolumeSlider == null || _soundeffectsVolumeSlider == null || _musicVolumeSlider == null || _dialougeVolumeSlider == null || _spokenLangaugeLeftBtn == null || _spokenLangaugeRightBtn == null || _subtitlesOffBtn == null || _subtitlesOnBtn == null || _subtitlesLangaugeLeftBtn == null || _subtitlesLangaugeRightBtn == null || _spokenLanguageDisplay == null || _subtitlesLanguageDisplay == null)
            {
                GD.PushError("Settings: missing buttons...");
                return;
            }
        }

        if (!_spokenLangaugeLeftBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SpokenLanguagePrev)))
        {
            _spokenLangaugeLeftBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SpokenLanguagePrev));
        }
        if (!_spokenLangaugeRightBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SpokenLanguageNext)))
        {
            _spokenLangaugeRightBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SpokenLanguageNext));
        }
        if (!_subtitlesOffBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SubtitlesOff)))
        {
            _subtitlesOffBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubtitlesOff));
        }
        if (!_subtitlesOnBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SubtitlesOn)))
        {
            _subtitlesOnBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubtitlesOn));
        }
        if (!_subtitlesLangaugeLeftBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SubtitlesLanguagePrev)))
        {
            _subtitlesLangaugeLeftBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubtitlesLanguagePrev));
        }
        if (!_subtitlesLangaugeRightBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(SubtitlesLanguageNext)))
        {
            _subtitlesLangaugeRightBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(SubtitlesLanguageNext));
        }

        if (!_masterVolumeSlider.IsConnected(Slider.SignalName.DragEnded, Callable.From((Variant _) => { MasterVolumeChanged(); })))
        {
            _masterVolumeSlider.Connect(Slider.SignalName.DragEnded, Callable.From((Variant v) => { MasterVolumeChanged(); }));
        }
        if (!_soundeffectsVolumeSlider.IsConnected(Slider.SignalName.DragEnded, Callable.From((Variant _) => { EffectsVolumeChanged(); })))
        {
            _soundeffectsVolumeSlider.Connect(Slider.SignalName.DragEnded, Callable.From((Variant v) => { EffectsVolumeChanged(); }));
        }
        if (!_musicVolumeSlider.IsConnected(Slider.SignalName.DragEnded, Callable.From((Variant _) => { MusicVolumeChanged(); })))
        {
            _musicVolumeSlider.Connect(Slider.SignalName.DragEnded, Callable.From((Variant v) => { MusicVolumeChanged(); }));
        }
        if (!_dialougeVolumeSlider.IsConnected(Slider.SignalName.DragEnded, Callable.From((Variant _) => { DialougeVolumeChanged(); })))
        {
            _dialougeVolumeSlider.Connect(Slider.SignalName.DragEnded, Callable.From((Variant v) => { DialougeVolumeChanged(); }));
        }


        #endregion

        #region Sub-ScreenSettings-Checks
        if (_resolutionLeftBtn == null || _resolutionRightBtn == null || _refreshRateLeftBtn == null || _refreshRateRightBtn == null || _fullscreenLeftBtn == null || _fullscreenRightBtn == null || _vsyncOffBtn == null || _vsyncOnBtn == null || _resolutionDisplay == null || _refreshRateDisplay == null || _fullscreenDisplay == null)
        {
            _resolutionLeftBtn = (TextureButton)GetParent().FindChild("ResolutionLeftBtn");
            _resolutionRightBtn = (TextureButton)GetParent().FindChild("ResolutionRightBtn");
            _refreshRateLeftBtn = (TextureButton)GetParent().FindChild("RefreshRateLeftBtn");
            _refreshRateRightBtn = (TextureButton)GetParent().FindChild("RefreshRateRightBtn");
            _fullscreenLeftBtn = (TextureButton)GetParent().FindChild("FullScreenLeftBtn");
            _fullscreenRightBtn = (TextureButton)GetParent().FindChild("FullScreenRightBtn");
            _vsyncOffBtn = (TextureButton)GetParent().FindChild("VSyncOffBtn");
            _vsyncOnBtn = (TextureButton)GetParent().FindChild("VSyncOnBtn");
            _resolutionDisplay = (RichTextLabel)GetParent().FindChild("ResolutionDisplayText");
            _refreshRateDisplay = (RichTextLabel)GetParent().FindChild("RefreshRateDisplayText");
            _fullscreenDisplay = (RichTextLabel)GetParent().FindChild("FullScreenDisplayText");

            if (_resolutionLeftBtn == null || _resolutionRightBtn == null || _refreshRateLeftBtn == null || _refreshRateRightBtn == null || _fullscreenLeftBtn == null || _fullscreenRightBtn == null || _vsyncOffBtn == null || _vsyncOnBtn == null || _resolutionDisplay == null || _refreshRateDisplay == null || _fullscreenDisplay == null)
            {
                GD.PushError("Settings: missing buttons...");
                return;
            }
        }

        if (!_resolutionLeftBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(ResolutionPrevious)))
        {
            _resolutionLeftBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(ResolutionPrevious));
        }
        if (!_resolutionRightBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(ResolutionNext)))
        {
            _resolutionRightBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(ResolutionNext));
        }
        if (!_refreshRateLeftBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(RefreshRatePrevious)))
        {
            _refreshRateLeftBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(RefreshRatePrevious));
        }
        if (!_refreshRateRightBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(RefreshRateNext)))
        {
            _refreshRateRightBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(RefreshRateNext));
        }
        if (!_fullscreenLeftBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(FullScreenPrevious)))
        {
            _fullscreenLeftBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(FullScreenPrevious));
        }
        if (!_fullscreenRightBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(FullScreenNext)))
        {
            _fullscreenRightBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(FullScreenNext));
        }
        if (!_vsyncOffBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(VSyncOff)))
        {
            _vsyncOffBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(VSyncOff));
        }
        if (!_vsyncOnBtn.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(VSyncOn)))
        {
            _vsyncOnBtn.Connect(BaseButton.SignalName.ButtonDown, Callable.From(VSyncOn));
        }

        #endregion

        #region Settings-Checks
        if(_settings == null)
        {
            _settings = ((UIManager)_wmMain.GetParent())._settings;
            if (_settings == null)
            {
                GD.PushError("SettingsMenu: no settings found...");
            }
        }
        #endregion

        _gameSBtn.ButtonPressed = true;
        SubGame();
    }

    #region Settings-Main-Buttons

    private void Apply()
    {
        GD.Print("Settings: Apply Button Pressed");
        _settings.ApplyCurrentSettings();
        ((UIManager)_wmMain.GetParent())._popup.DisplayConfirmPopUpCD("Keep these NEW settings?", 10d, Callable.From(() => { ApplyConfirmed(); }), Callable.From(() => { ApplyCancelled(); }));
    }

    private void ApplyConfirmed()
    {
        GD.Print("Settings: apply confirmed");
        _settings.SaveSettings();
        _wmMain.ShowWindow(_winMainMenu);

        ((UIManager)_wmMain.GetParent())._popup.DisplayInfoPopUp("Settings successfully APPLIED...");
    }

    private void ApplyCancelled()
    {
        GD.Print("Settings: apply cancelled");

        _settings.LoadSettings();
        _settings.ApplyCurrentSettings();
        DisplayCurrentAudioSettings();
        DisplayCurrentControlsSettings();
        DisplayCurrentDisplaySettings();
        DisplayCurrentGameSettings();

        ((UIManager)_wmMain.GetParent())._popup.DisplayInfoPopUp("Settings REVERTED...");

    }

    private void Cancel()
    {
        GD.Print("Settings: Cancel Button Pressed");
        _wmMain.ShowWindow(_winMainMenu);
        _settings.LoadSettings();
        DisplayCurrentAudioSettings();
        DisplayCurrentControlsSettings();
        DisplayCurrentDisplaySettings();
        DisplayCurrentGameSettings();
    }

    private void Reset()
    {
        GD.Print("Settings: Reset Settings Button Pressed");
        ((UIManager)_wmMain.GetParent())._popup.DisplayConfirmPopUp("Reset ALL settings?", Callable.From(ResetConfirmed));
    }

    private void ResetConfirmed()
    {
        GD.Print("Settings: resetting all settings");
        _settings.DefaultSettings();

        _settings.LoadSettings();
        _settings.ApplyCurrentSettings();
        DisplayCurrentAudioSettings();
        DisplayCurrentControlsSettings();
        DisplayCurrentDisplaySettings();
        DisplayCurrentGameSettings();

        ((UIManager)_wmMain.GetParent())._popup.DisplayInfoPopUp("Settings RESET...");
    }

    #endregion

    #region Sub-ScreenSettings

    void ResolutionPrevious()
    {
        GD.Print("Settings: Previous Resolution Button Pressed");
        int index = _settings.SUPPORTED_RESOLUTIONS.ToList().IndexOf(_currentResolution);
        if(index == -1)
        {
            GD.PushError("Resolution Loaded Wrongly");
            return;
        }
        index -= 1;
        if(index < 0)
        {
            index = _settings.SUPPORTED_RESOLUTIONS.Length - 1;
            if(DisplayServer.ScreenGetSize() < _settings.SUPPORTED_RESOLUTIONS[index])
            {
                int infiniteLoopCatch = 100;
                while(DisplayServer.ScreenGetSize() < _settings.SUPPORTED_RESOLUTIONS[index] && infiniteLoopCatch != 0)
                {
                    infiniteLoopCatch--;
                    index--;
                }
                if(infiniteLoopCatch == 0)
                {
                    GD.PushError("INFINITE LOOP DETECTED...");
                    ((UIManager)_wmMain.GetParent())._popup.DisplayError("Error: Infinite Loop Detected","Screen Size NOT Supported...");
                }
            }
        }
        _currentResolution = _settings.SUPPORTED_RESOLUTIONS[index];
        _settings._resolution = _currentResolution;
        _resolutionDisplay.Text = _currentResolution.X.ToString() + "X" + _currentResolution.Y.ToString();
    }
    void ResolutionNext()
    {
        GD.Print("Settings: Next Resolution Button Pressed");
        int index = _settings.SUPPORTED_RESOLUTIONS.ToList().IndexOf(_currentResolution);
        if (index == -1)
        {
            GD.PushError("Resolution Loaded Wrongly");
            return;
        }
        index += 1;
        if (DisplayServer.ScreenGetSize() < _settings.SUPPORTED_RESOLUTIONS[index])
        {
            index = 0;
        }
        if (index == _settings.SUPPORTED_RESOLUTIONS.Length)
        {
            index = 0;
        }
        _currentResolution = _settings.SUPPORTED_RESOLUTIONS[index];
        _settings._resolution = _currentResolution;
        _resolutionDisplay.Text = _currentResolution.X.ToString() + "X" + _currentResolution.Y.ToString();
    }
    void RefreshRatePrevious()
    {
        GD.Print("Settings: Previous Refresh Rate Button Pressed");
        _refreshRateDisplay.Text = System.Math.Round(--_settings._refreshRate, 0).ToString() + "hz";
    }
    void RefreshRateNext()
    {
        GD.Print("Settings: Next Refresh Rate Button Pressed");
        _refreshRateDisplay.Text = System.Math.Round(++_settings._refreshRate, 0).ToString() + "hz";

    }
    void FullScreenPrevious()
    {
        GD.Print("Settings: Previous Fullscreen Button Pressed");
        _settings._displayMode--;
        if (_settings._displayMode < 0)
        {
            _settings._displayMode = System.Enum.GetValues(typeof(ArchitectsInVoid.Settings.Settings.DisplayMode)).Cast<Settings.Settings.DisplayMode>().Max();
        }
        _fullscreenDisplay.Text = _settings.DisplayModeToString(_settings._displayMode);

    }
    void FullScreenNext()
    {
        GD.Print("Settings: Next Fullscreen Button Pressed");
        _settings._displayMode++;
        if (_settings._displayMode > System.Enum.GetValues(typeof(ArchitectsInVoid.Settings.Settings.DisplayMode)).Cast<Settings.Settings.DisplayMode>().Max())
        {
            _settings._displayMode = 0;
        }
        _fullscreenDisplay.Text = _settings.DisplayModeToString(_settings._displayMode);

    }
    void VSyncOn()
    {
        GD.Print("Settings: VSync Toggled On");
        _vsyncOnBtn.Disabled = true;
        _vsyncOffBtn.ButtonPressed = false;
        _vsyncOffBtn.Disabled = false;
        _refreshRateLeftBtn.Disabled = true;
        _refreshRateRightBtn.Disabled = true;
        _settings._refreshRate = DisplayServer.ScreenGetRefreshRate();
        _refreshRateDisplay.Text = System.Math.Round(_settings._refreshRate, 1).ToString() + "hz";
    }
    void VSyncOff()
    {
        GD.Print("Settings: VSync Toggled Off");
        _vsyncOffBtn.Disabled = true;
        _vsyncOnBtn.ButtonPressed = false;
        _vsyncOnBtn.Disabled = false;
        _refreshRateLeftBtn.Disabled = false;
        _refreshRateRightBtn.Disabled = false;
    }

    #endregion

    #region Sub-AudioSettings

    void SubtitlesOn()
    {
        GD.Print("Settings: Subtitles Toggled On");
        _subtitlesOnBtn.Disabled = true;
        _subtitlesOffBtn.ButtonPressed = false;
        _subtitlesOffBtn.Disabled = false;
        _settings._subtitles = true;
    }
    void SubtitlesOff()
    {
        GD.Print("Settings: Subtitles Toggled Off");
        _subtitlesOffBtn.Disabled = true;
        _subtitlesOnBtn.ButtonPressed = false;
        _subtitlesOnBtn.Disabled = false;
        _settings._subtitles = false;

    }
    void SpokenLanguageNext()
    {
        GD.Print("Settings: Next Spoken Language Button Pressed");
        _currentSpokenLanguage += 1;
        if(_currentSpokenLanguage > System.Enum.GetValues(typeof(ArchitectsInVoid.Settings.Settings.Language)).Cast<Settings.Settings.Language>().Max())
        {
            _currentSpokenLanguage = 0;
        }

        _spokenLanguageDisplay.Text = _settings.LanguageToString(_currentSpokenLanguage);
        _settings._spokenLanguage = _currentSpokenLanguage;
    }
    void SpokenLanguagePrev()
    {
        GD.Print("Settings: Previous Spoken Language Button Pressed");
        _currentSpokenLanguage -= 1;
        if (_currentSpokenLanguage < 0)
        {
            _currentSpokenLanguage = System.Enum.GetValues(typeof(ArchitectsInVoid.Settings.Settings.Language)).Cast<Settings.Settings.Language>().Max();
        }

        _spokenLanguageDisplay.Text = _settings.LanguageToString(_currentSpokenLanguage);
        _settings._spokenLanguage = _currentSpokenLanguage;
    }
    void SubtitlesLanguageNext()
    {
        GD.Print("Settings: Next Subtitles Language Button Pressed");
        _currentSubtitlesLanguage += 1;
        if (_currentSubtitlesLanguage > System.Enum.GetValues(typeof(ArchitectsInVoid.Settings.Settings.Language)).Cast<Settings.Settings.Language>().Max())
        {
            _currentSubtitlesLanguage = 0;
        }

        _subtitlesLanguageDisplay.Text = _settings.LanguageToString(_currentSubtitlesLanguage);
        _settings._subtitlesLanguage = _currentSubtitlesLanguage;
    }
    void SubtitlesLanguagePrev()
    {
        GD.Print("Settings: Previous Subtitles Language Button Pressed");
        _currentSubtitlesLanguage -= 1;
        if (_currentSubtitlesLanguage < 0)
        {
            _currentSubtitlesLanguage = System.Enum.GetValues(typeof(ArchitectsInVoid.Settings.Settings.Language)).Cast<Settings.Settings.Language>().Max();
        }

        _subtitlesLanguageDisplay.Text = _settings.LanguageToString(_currentSubtitlesLanguage);
        _settings._subtitlesLanguage = _currentSubtitlesLanguage;
    }

    void MasterVolumeChanged()
    {
        GD.Print("Settings: slider (Master) changed");
        _settings._masterVolume = _masterVolumeSlider.Value;

    }

    void EffectsVolumeChanged()
    {
        GD.Print("Settings: slider (Effects) changed");
        _settings._effectsVolume = _soundeffectsVolumeSlider.Value;

    }

    void MusicVolumeChanged()
    {
        GD.Print("Settings: slider (Music) changed");
        _settings._musicVolume = _musicVolumeSlider.Value;

    }

    void DialougeVolumeChanged()
    {
        GD.Print("Settings: slider (Dialouge) changed");
        _settings._dialougeVolume = _dialougeVolumeSlider.Value;

    }

    #endregion

    #region Sub-GameSettings

    void LanguageNext()
    {
        GD.Print("Settings: Next Language Button Pressed");

    }
    void LanguagePrev()
    {
        GD.Print("Settings: Previous Language Button Pressed");

    }

    #endregion

    #region Sub-Windows
    private void SubGame()
    {
        _gameSBtn.Disabled = true;

        GD.Print("Settings: Game Settings Button Pressed");
        _controlsSBtn.ButtonPressed = false;
        _audioSBtn.ButtonPressed = false;
        _displaySBtn.ButtonPressed = false;
        _controlsSBtn.Disabled = false;
        _audioSBtn.Disabled = false;
        _displaySBtn.Disabled = false;
        _wmSettingSub.ShowWindow(_winSubGame);
        DisplayCurrentGameSettings();
    }

    private void SubControls()
    {
        _controlsSBtn.Disabled = true;

        GD.Print("Settings: Controls Settings Button Pressed");
        _gameSBtn.ButtonPressed = false;
        _audioSBtn.ButtonPressed = false;
        _displaySBtn.ButtonPressed = false;
        _gameSBtn.Disabled = false;
        _audioSBtn.Disabled = false;
        _displaySBtn.Disabled = false;
        _wmSettingSub.ShowWindow(_winSubControls);
        DisplayCurrentControlsSettings();
    }

    private void SubAudio()
    {
        _audioSBtn.Disabled = true;

        GD.Print("Settings: Audio Settings Button Pressed");
        _gameSBtn.ButtonPressed = false;
        _controlsSBtn.ButtonPressed = false;
        _displaySBtn.ButtonPressed = false;
        _gameSBtn.Disabled = false;
        _controlsSBtn.Disabled = false;
        _displaySBtn.Disabled = false;
        _wmSettingSub.ShowWindow(_winSubAudio);
        DisplayCurrentAudioSettings();
    }

    private void SubDisplay()
    {
        _displaySBtn.Disabled = true;

        GD.Print("Settings: Display Settings Button Pressed");
        _gameSBtn.ButtonPressed = false;
        _controlsSBtn.ButtonPressed = false;
        _audioSBtn.ButtonPressed = false;
        _gameSBtn.Disabled = false;
        _controlsSBtn.Disabled = false;
        _audioSBtn.Disabled = false;
        _wmSettingSub.ShowWindow(_winSubDisplay);
        DisplayCurrentDisplaySettings();
    }
    #endregion

    #region ConnectSettingsToUI
    void DisplayCurrentGameSettings()
    {
        
    }

    void DisplayCurrentControlsSettings()
    {

    }

    void DisplayCurrentAudioSettings()
    {
        _masterVolumeSlider.Value = _settings._masterVolume;
        _soundeffectsVolumeSlider.Value = _settings._effectsVolume;
        _musicVolumeSlider.Value = _settings._musicVolume;
        _dialougeVolumeSlider.Value = _settings._dialougeVolume;
        _currentSpokenLanguage = _settings._spokenLanguage;
        _spokenLanguageDisplay.Text = _settings.LanguageToString(_currentSpokenLanguage);
        if (_settings._subtitles)
        {
            SubtitlesOn();
        }
        else
        {
            SubtitlesOff();
        }
        _currentSubtitlesLanguage = _settings._subtitlesLanguage;
        _subtitlesLanguageDisplay.Text = _settings.LanguageToString(_currentSubtitlesLanguage);
        _spokenLangaugeLeftBtn.Disabled = false;
        _spokenLangaugeRightBtn.Disabled = false;
        _subtitlesLangaugeLeftBtn.Disabled = false;
        _subtitlesLangaugeRightBtn.Disabled = false;
    }

    void DisplayCurrentDisplaySettings()
    {
        _resolutionLeftBtn.Disabled = false;
        _resolutionRightBtn.Disabled = false;
        _currentResolution = _settings._resolution;
        _resolutionDisplay.Text = _currentResolution.X.ToString() + "X" + _currentResolution.Y.ToString();
        _refreshRateDisplay.Text = System.Math.Round(_settings._refreshRate, 1).ToString() + "hz";
        _refreshRateLeftBtn.Disabled = _settings._vsync;
        _refreshRateRightBtn.Disabled = _settings._vsync;
        _vsyncOffBtn.Disabled = !_settings._vsync;
        _vsyncOnBtn.Disabled = _settings._vsync;
        _fullscreenDisplay.Text = _settings.DisplayModeToString(_settings._displayMode);
        _fullscreenLeftBtn.Disabled = false;
        _fullscreenRightBtn.Disabled = false;
    }
    #endregion

    public Array AddInspectorButtons()
    {
        var buttons = new Array();

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