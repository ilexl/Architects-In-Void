using System;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

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
        Wrapper.Call(ReceiveFunction, functionName);
        foreach (var arg in args)
        {
            Wrapper.Call(ReceiveArgument, arg);
        }
        return Wrapper.Call(FinalizeCall);
    }
    #endregion


    public static void AddListener(int index, Variant gameObject)
    {
        FmodCall("add_listener", index, gameObject);
    }

    public static bool BanksStillLoading()
    {
        return (bool)FmodCall("banks_still_loading");
    }

    public static bool CheckBusGuid(string guid)
    {
        return (bool)FmodCall("check_bus_guid", guid);
    }
    public static bool CheckBusPath(string busPath)
    {
        return (bool)FmodCall("check_bus_path", busPath);
    }

    public static bool CheckEventGuid(string guid)
    {
        return (bool)FmodCall("check_event_guid", guid);
    }

    public static bool CheckEventPath(string eventPath)
    {
        return (bool)FmodCall("check_event_path", eventPath);
    }

    public static bool CheckVcaGuid(string guid)
    {
        return (bool)FmodCall("check_vca_guid", guid);
    }

    public static bool CheckVcaPath(string vcaPath)
    {
        return (bool)FmodCall("check_vca_path", vcaPath);
    }
    public static FmodEvent CreateEventInstance(string eventPath)
    {
        var gdEvent = FmodCall("create_event_instance", eventPath);
        return new FmodEvent(gdEvent);
    }

    public static FmodEvent CreateEventInstanceFromDescription(FmodEventDescription description)
    {
        var gdEvent = FmodCall("create_event_instance_from_description", description);
        return new FmodEvent(gdEvent);
    }

    public static FmodEvent CreateEventInstanceWithGuid(string guid)
    {
        var gdEvent = FmodCall("create_event_instance_with_guid", guid);
        return new FmodEvent(gdEvent);
    }

    public static FmodSound CreateSoundInstance(string path)
    {
        var gdEvent = FmodCall("create_sound_instance", path);
        return new FmodSound(gdEvent);
    }

    public static Array GetAllBanks()
    {
        return (Array)FmodCall("get_all_banks");
    }

    public static Array GetAllBuses()
    {
        return (Array)FmodCall("get_all_buses");
    }

    public static Array GetAllEventDescriptions()
    {
        return (Array)FmodCall("get_all_event_descriptions");
    }

    public static Array GetAllVca()
    {
        return (Array)FmodCall("get_all_vca");
    }

    public static Array GetAvailableDrivers()
    {
        return (Array)FmodCall("get_available_drivers");
    }

    public static FmodBus GetBus(string busPath)
    {
        var gdBus = FmodCall("get_bus", busPath);
        return new FmodBus(gdBus);
    }

    public static FmodBus GetBusFromGuid(string guid)
    {
        var gdBus = FmodCall("get_bus_from_guid", guid);
        return new FmodBus(gdBus);
    }

    public static int GetDriver()
    {
        
    }

    public static FmodEventDescription GetEvent(string eventPath)
    {
        
    }

    public static FmodEventDescription GetEventFromGuid(string guid)
    {
        
    }

    public static float GetGlobalParameterById(int id)
    {
        
    }

    public static float GetGlobalParameterNyName(string name)
    {
        
    }

    public static Dictionary GetGlobalParameterDescById(int id)
    {
        
    }

    public static Dictionary GetGlobalParameterDescByname(string name)
    {
        
    }

    public static int GetGlobalParameterDescCount()
    {
        
    }

    public static Array GetGlobalParameterDescList()
    {
        
    }

    public static Vector2 GetListener2dVelocity(int index)
    {
        
    }

    public static Vector3 GetListener3DVelocity(int index)
    {
        
    }

    public static bool GetListenerLock(int index)
    {
        
    }

    public static int GetListenerNumber()
    {
        
    }

    public static Transform2D GetListenerTransform2D(int index)
    {
        
    }

    public static Transform3D GetListenerTransform3D(int index)
    {
        
    }

    public static float GetListenerWeight(int index)
    {
        
    }

    public static Object GetObjectAttachedToListener(int index)
    {
        
    }

    public static FmodPerformanceData GetPerformanceData()
    {
        
    }

    public static int GetSystemDspBufferLength()
    {
        
    }

    public static FmodDspSettings GetSystemDspBufferSettings()
    {
        
    }

    public static int GetSystemDspNumBuffers()
    {
        
    }

    public static FmodVCA GetVca(string vcaPath)
    {
        
    }

    public static FmodVCA GetVcaFromGuid(string guid)
    {
        
    }

    public static void Init(FmodGeneralSettings settings)
    {
        
    }

    public static FmodBank LoadBank(string PathToBank, int flags)
    {
        
    }

    public static FmodFile LoadFileAsMusic(string path)
    {
        
    }

    public static FmodFile LoadFileAsSound(string path)
    {
        
    }

    public static void MuteAllEvents()
    {
        
    }

    public static void PauseAllEvents()
    {
        
    }

    public static void PlayOneShot(string eventName)
    {
        
    }

    public static void PlayOneShotAttached(string eventName, Node gameObject)
    {
        
    }

    public static void PlayOneShotAttachedWithParams(string eventName, Node gameObject, Dictionary parameters)
    {
        
    }

    public static void PlayOneShotUsingEventDescription(FmodEventDescription description)
    {
        
    }
    public static void PlayOneShotUsingEventDescriptionAttached(FmodEventDescription description, Node gameObject)
    {
        
    }
    
    public static void PlayOneShotUsingEventDescriptionWithParams(FmodEventDescription description, Dictionary parameters)
    {
        
    }
    public static void PlayOneShotUsingEventDescriptionAttachedWithParams(FmodEventDescription description, Node gameObject, Dictionary parameters)
    {
        
    }

    public static void PlayOneShotUsingGuid(string guid)
    {
        
    }

    public static void PlayOneShotUsingGuidAttached(string guid, Node gameObject)
    {
        
    }

    public static void PlayOneShotUsingGuidWithParams(string guid, Dictionary parameters)
    {
        
    }

    public static void PlayOneShotUsingGuidAttachedWithParams(string guid, Node gameObject, Dictionary parameters)
    {
        
    }

    public static void RemoveListener(int index)
    {
        
    }

    public static void SetDriver(int id)
    {
        
    }

    public static void SetGlobalParameterById(int parameterId, float value)
    {
        
    }

    public static void SetGlobalParameterByIdWithLabel(int parameterId, string label)
    {
        
    }

    public static void SetGlobalParameterByName(string parameterName, float value)
    {
        
    }

    public static void SetGlobalParameterByNameWithLabel(string parameterName, string label)
    {
        
    }

    public static void SetListenerLock(int index, bool isLocked)
    {
        
    }

    public static void SetListenerNumber(int listenerNumber)
    {
        
    }

    public static void SetListenerTransform2D(int index, Transform2D transform)
    {
        
    }

    public static void SetListenerTransform3D(int index, Transform3D transform)
    {
        
    }

    public static void SetListenerWeight(int index, float weight)
    {
        
    }

    public static void SetSoftwareFormat(FmodSoftwareFormatSettings pSettings)
    {
        
    }

    public static void SetSound3DSettings(FmodSound3DSettings pSettings)
    {
        
    }

    public static void SetSystemDspBufferSize(FmodDspSettings dspSettings)
    {
        
    }

    public static void Shutdown()
    {
        
    }

    public static void UnloadBank(string pathToBank)
    {
        
    }

    public static void UnloadFile(string pathToFile)
    {
        
    }

    public static void UnmuteAllEvents()
    {
        
    }

    public static void UnpauseAllEvents()
    {
        
    }

    public static void Update()
    {
        
    }

    public static void WaitForAllLoads()
    {
        
    }
}

