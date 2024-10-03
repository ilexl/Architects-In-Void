using Godot;
using Godot.Collections;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class SettingsMenu : Node
{
    [ExportGroup("Settings-Buttons","")]
    [Export] private TextureButton _cancelBtn, _applyBtn, _resetBtn, _gameSBtn, _controlsSBtn, _audioSBtn, _displaySBtn;
    [ExportGroup("Settings-Windows", "")]
    [Export] private Window _winMainMenu, _winSubGame, _winSubControls, _winSubAudio, _winSubDisplay;
    [Export] private WindowManager _wmMain, _wmSettingSub;
    [ExportGroup("Settings-Game", "")]
    [Export] private TextureButton _langaugeLeftBtn, _langaugeRightBtn;
    [Export] private RichTextLabel _languageDisplay;
    [ExportGroup("Settings-Controls", "")]
    [Export] private bool tempBool;
    [ExportGroup("Settings-Audio", "")]
    [Export] private HSlider _masterVolumeSlider, _soundeffectsVolumeSlider, _musicVolumeSlider, _dialougeVolumeSlider;
    [Export] private TextureButton _spokenLangaugeLeftBtn, _spokenLangaugeRightBtn, _subtitlesOffBtn, _subtitlesOnBtn, 
                                   _subtitlesLangaugeLeftBtn, _subtitlesLangaugeRightBtn;
    [Export] private RichTextLabel _spokenLanguageDisplay, _subtitlesLanguageDisplay;
    [ExportGroup("Settings-Screen", "")]
    [Export] private TextureButton _resolutionLeftBtn, _resolutionRightBtn, _refreshRateLeftBtn, _refreshRateRightBtn,
                                   _fullscreenLeftBtn, _fullscreenRightBtn, _vsyncOffBtn, _vsyncOnBtn;
    [Export] private RichTextLabel _resolutionDisplay, _refreshRateDisplay, _fullscreenDisplay;


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
        #endregion

        #region Sub-ScreenSettings-Checks
        if(_resolutionLeftBtn == null || _resolutionRightBtn == null || _refreshRateLeftBtn == null || _refreshRateRightBtn == null || _fullscreenLeftBtn == null || _fullscreenRightBtn == null || _vsyncOffBtn == null || _vsyncOnBtn == null || _resolutionDisplay == null || _refreshRateDisplay == null || _fullscreenDisplay == null)
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

        _gameSBtn.ButtonPressed = true;
        SubGame();
    }

    private void Apply()
    {
        GD.Print("Settings: Apply Button Pressed");
    }

    private void Cancel()
    {
        GD.Print("Settings: Cancel Button Pressed");
        _wmMain.ShowWindow(_winMainMenu);
    }

    private void Reset()
    {
        GD.Print("Settings: Reset Settings Button Pressed");
    }

    #region Sub-ScreenSettings

    void ResolutionPrevious()
    {
        GD.Print("Settings: Previous Resolution Button Pressed");

    }
    void ResolutionNext()
    {
        GD.Print("Settings: Next Resolution Button Pressed");

    }
    void RefreshRatePrevious()
    {
        GD.Print("Settings: Previous Refresh Rate Button Pressed");

    }
    void RefreshRateNext()
    {
        GD.Print("Settings: Next Refresh Rate Button Pressed");

    }
    void FullScreenPrevious()
    {
        GD.Print("Settings: Previous Fullscreen Button Pressed");

    }
    void FullScreenNext()
    {
        GD.Print("Settings: Next Fullscreen Button Pressed");

    }
    void VSyncOn()
    {
        GD.Print("Settings: VSync Toggled On");
        _vsyncOnBtn.Disabled = true;
        _vsyncOffBtn.ButtonPressed = false;
        _vsyncOffBtn.Disabled = false;
    }
    void VSyncOff()
    {
        GD.Print("Settings: VSync Toggled Off");
        _vsyncOffBtn.Disabled = true;
        _vsyncOnBtn.ButtonPressed = false;
        _vsyncOnBtn.Disabled = false;
    }

    #endregion

    #region Sub-AudioSettings

    void SubtitlesOn()
    {
        GD.Print("Settings: Subtitles Toggled On");
        _subtitlesOnBtn.Disabled = true;
        _subtitlesOffBtn.ButtonPressed = false;
        _subtitlesOffBtn.Disabled = false;
    }
    void SubtitlesOff()
    {
        GD.Print("Settings: Subtitles Toggled Off");
        _subtitlesOffBtn.Disabled = true;
        _subtitlesOnBtn.ButtonPressed = false;
        _subtitlesOnBtn.Disabled = false;
    }
    void SpokenLanguageNext()
    {
        GD.Print("Settings: Next Spoken Language Button Pressed");

    }
    void SpokenLanguagePrev()
    {
        GD.Print("Settings: Previous Spoken Language Button Pressed");

    }
    void SubtitlesLanguageNext()
    {
        GD.Print("Settings: Next Subtitles Language Button Pressed");

    }
    void SubtitlesLanguagePrev()
    {
        GD.Print("Settings: Previous Subtitles Language Button Pressed");

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