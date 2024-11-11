using Godot;

namespace ArchitectsInVoid.Audio;

public class FmodEvent
{
    #region Config
        private const string WrapperPath = "res://Audio/FmodServer/fmod_event_wrapper.gd";
        private const string ReceiveEvent = "re";
        private const string ReceiveFunction = "rf";
        private const string ReceiveArgument = "rx";
        private const string FinalizeCall = "fc";
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
            Wrapper.Call(ReceiveEvent, _event);
            GD.Print("FMODServerC: Begin wrapper call");
            Wrapper.Call(ReceiveFunction, functionName);
            GD.Print("FMODServerC: sent function name");
            foreach (var arg in args)
            {
                Wrapper.Call(ReceiveArgument, arg);
                GD.Print("FmodServerC: sent argument");
            }
            GD.Print("FmodServerC: finalizing call");
            return Wrapper.Call(FinalizeCall);
        }
    #endregion
        
    public void Start()
    {
        FmodCall("start");
    }

    public void Set3dAttributes(Transform3D transform)
    {
        FmodCall("set_3d_attributes", transform);
    }
}