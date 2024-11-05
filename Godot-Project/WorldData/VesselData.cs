using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ArchitectsInVoid.WorldData;

[Tool]
public partial class VesselData : Node
{
    [Export] PackedScene _vesselBlank;

    public static VesselData _VesselData;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Engine.IsEditorHint()) { return; } // do NOT run when not in game
        if (_vesselBlank == null)
        {
            _vesselBlank = (PackedScene)GD.Load("res://Scenes/BlankShip.tscn");
            if (_vesselBlank == null)
            {
                GD.PushError("PlayerData: No Packed Scene found for shipBlank...");
                return;
            }
        }

        if (_VesselData == null) _VesselData = this;

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void _Load(FileAccess file)
    {
        // get amount of ships
        _ = file.GetVar();
    }

    public void _DiscardLoadPast(FileAccess file)
    {
        // get amount of players
        _ = file.GetVar();
    }

    public void _Save(FileAccess file)
    {
        file.StoreVar(0);
        var options = new JsonSerializerOptions { WriteIndented = true };
        foreach(var child in GetChildren())
        {
            // each one of these is a child needing to be saved
            if(child is Vessel vessel)
            {
                string test = JsonSerializer.Serialize(vessel, options);
                GD.Print(test);
            }
        }

    }

    public Vessel CreateVessel(Vector3 position)
    {
        Vessel newVessel = (Vessel)_vesselBlank.Instantiate();
        newVessel.RigidBody.Position = position;
        AddChild(newVessel);
        return newVessel;
    }

    internal void _NewGame(FileAccess file)
    {
        file.StoreVar(0);
    }
}