using System;
using Godot;

namespace ArchitectsInVoid.Audio;

/// <summary>
/// A GDscript to C# wrapper for FMOD bindings
/// </summary>
public static class FmodServer
{
    #region Config
    private const string WrapperPath = "res://Audio/FmodServer/fmod_server_wrapper.gd";
    private const string ReceiveFunction = "rf";
    private const string ReceiveArgument = "rx";
    private const string FinalizeCall = "fc";
    #endregion
    
    #region InstancingWrapper
    
    private static Script _wrapper;
    private static Script Wrapper => _wrapper ??= (Script)GD.Load(WrapperPath);

    #endregion
    #region FunctionCallWrapper
    private static Variant FmodCall(string functionName, params Variant[] args)
    {
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
        
    public static FmodEvent CreateEventInstance(string eventPath)
    {
        var gdEvent = FmodCall("create_event_instance", eventPath);
        return new FmodEvent(gdEvent);
    }
}

#region Archived TLDR Rambling
// This is an unfortunately necessary hacky solution:
// GDScript supports nested variant lists. C# does not.
// Wrapper.Call takes a string as the function to call in the GDScript script and a list of variants as the args.
// You might be thinking "well that's fine, just call the wrapper reciever function with a list of variants as the args"
// The problem here is that in gdscript, it's not one parameter, but rather every item in the list is a single parameter.
// So it goes from Wrapper.Call("fmod_func", ["CreateEventInstance", "event:/whatever"])
// to static func fmod_func("CreateEventInstance", "event:/whatever")
// This would be fine... IF you could take a list with a params keyword like this function right here. But you can't. Not in GDScript.
// So the hacky workaround is to forcibly cast the Variant[] to Variant, which means we end up with an asymmetric variant array, like:
// Wrapper.Call("fmod_func", ["CreateEventInstance", ["event:/whatever", "anotherargwhocares"]])
// If you try to do anything with this in C#, it nopes out... but just as long as nothing checks it along the way it will get past
// API border control. And GDScript kinda just handles it.
#endregion