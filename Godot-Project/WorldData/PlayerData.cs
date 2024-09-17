using Godot;
using System;

[Tool]
public partial class PlayerData : Node
{
    [Export] PackedScene playerBlank;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        if(playerBlank == null)
        {
            playerBlank = (PackedScene)GD.Load("res://Scenes/BlankPlayer.tscn");
            if (playerBlank == null)
            {
                GD.PushError("PlayerData: No Packed Scene found for playerBlank...");
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
        // get amount of players
        int playerAmount = file.GetVar().AsInt32();
        for (int i = 0; i < playerAmount; i++)
        {
            // load the players data
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
