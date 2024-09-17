using Godot;
using System;

[Tool]
public partial class ShipData : Node
{
    [Export] PackedScene shipBlank;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (shipBlank == null)
        {
            shipBlank = (PackedScene)GD.Load("res://Scenes/BlankShip.tscn");
            if (shipBlank == null)
            {
                GD.PushError("PlayerData: No Packed Scene found for shipBlank...");
                return;
            }
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _Load(FileAccess file)
	{
        // get amount of ships
        int shipsAmount = file.GetVar().AsInt32();
        for (int i = 0; i < shipsAmount; i++)
        {
            // load the ships data
        }
    }

    public void _DiscardLoadPast(FileAccess file)
    {
        // get amount of players
        int playerAmount = file.GetVar().AsInt32();
        for (int i = 0; i < playerAmount; i++)
        {
            // load the players data
        }
    }

    public void _Save(FileAccess file)
    {
        file.StoreVar(0);
    }
}
