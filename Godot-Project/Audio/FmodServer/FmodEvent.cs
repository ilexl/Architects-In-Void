using Godot;

namespace ArchitectsInVoid.Audio;

public class FmodEvent
{
    #region Config
    private const string WrapperPath = "res://Audio/FmodServer/fmod_event_wrapper.gd";
    private const string SetEvent = "re";
    private const string SetFunction = "rf";
    private const string SendArgument = "rx";
    private const string CallFunction = "fc";

    private const string WrapperGetProperty = "gp";
    private const string WrapperSetProperty = "sp";
    #endregion

    private Variant _event;

    public FmodEvent(Variant ev)
    {
        _event = ev;
    }

    #region InstancingWrapper
    private static Script _wrapper;
    private static Script Wrapper => _wrapper ??= (Script)GD.Load(WrapperPath);
    #endregion

    #region FunctionCallWrapper
    private Variant FmodCall(string functionName, params Variant[] args)
    {
        Wrapper.Call(SetEvent, _event);
        Wrapper.Call(SetFunction, functionName);
        foreach (var arg in args)
        {
            Wrapper.Call(SendArgument, arg);
        }

        return Wrapper.Call(CallFunction);
    }
    #endregion

    #region PropertyCallWrapper
    private Variant GetProperty(string propertyName)
    {
        Wrapper.Call(SetEvent, _event);
        return Wrapper.Call(WrapperGetProperty, propertyName);
    }

    private void SetProperty(string propertyName, Variant value)
    {
        Wrapper.Call(SetEvent, _event);
        Wrapper.Call(WrapperSetProperty, propertyName, value);
    }
    #endregion

    #region Properties
    public int ListenerMask
    {
        get => (int)GetProperty("listener_mask");
        set => SetProperty("listener_mask", value);
    }

    public bool Paused
    {
        get => (bool)GetProperty("paused");
        set => SetProperty("paused", value);
    }

    public float Pitch
    {
        get => (float)GetProperty("pitch");
        set => SetProperty("pitch", value);
    }

    public int Position
    {
        get => (int)GetProperty("position");
        set => SetProperty("position", value);
    }

    public Transform2D Transform2D
    {
        get => (Transform2D)GetProperty("transform_2d");
        set => SetProperty("transform_2d", value);
    }

    public Transform3D Transform3D
    {
        get => (Transform3D)GetProperty("transform_3d");
        set => SetProperty("transform_3d", value);
    }

    public float Volume
    {
        get => (float)GetProperty("volume");
        set => SetProperty("volume", value);
    }
    #endregion

    #region Methods
    public void EventKeyOff()
    {
        FmodCall("event_key_off");
    }

    public float GetParameterById(int id)
    {
        return (float)FmodCall("get_parameter_by_id", id);
    }

    public float GetParameterByName(string name)
    {
        return (float)FmodCall("get_parameter_by_name", name);
    }

    public int GetPlaybackState()
    {
        return (int)FmodCall("get_playback_state");
    }

    public string GetProgrammerCallbackSoundKey()
    {
        return (string)FmodCall("get_programmer_callback_sound_key");
    }

    public float GetReverbLevel(int index)
    {
        return (float)FmodCall("get_reverb_level", index);
    }

    public bool IsValid()
    {
        return (bool)FmodCall("is_valid");
    }

    public bool IsVirtual()
    {
        return (bool)FmodCall("is_virtual");
    }

    public void Release()
    {
        FmodCall("release");
    }

    public void Set3dAttributes(Transform3D transform)
    {
        FmodCall("set_3d_attributes", transform);
    }

    public void SetCallback(Callable callback, int callBackMask)
    {
        FmodCall("set_callback", callback, callBackMask);
    }

    public void SetParameterById(int parameterId, float value)
    {
        FmodCall("set_parameter_by_id", parameterId, value);
    }

    public void SetParameterByIdWithLabel(int parameterId, string label, bool ignoreSeekSpeed)
    {
        FmodCall("set_parameter_by_id_with_label", parameterId, label, ignoreSeekSpeed);
    }

    public void SetParameterByName(string parameterName, float value)
    {
        FmodCall("set_parameter_by_name", parameterName, value);
    }

    public void SetParameterByNameWithLabel(string parameterName, string label, bool ignoreSeekSpeed)
    {
        FmodCall("set_parameter_by_name_with_label", parameterName, label, ignoreSeekSpeed);
    }

    public void SetProgrammerCallback(string pProgrammersCallbackSoundKey)
    {
        FmodCall("set_programmer_callback", pProgrammersCallbackSoundKey);
    }

    public void SetReverbLevel(int index, float level)
    {
        FmodCall("set_reverb_level", index, level);
    }

    public void Start()
    {
        FmodCall("start");
    }

    public void Stop(int stopMode)
    {
        FmodCall("stop", stopMode);
    }
    #endregion
}