using Godot;
using System;

public partial class FmodSoundMananger : Node
{
	bool loaded = false;
	[Export] PackedScene soundTemp; 
	double time = 10;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        time -= delta;
        if (time < 0)
        {
			loaded = false;
			time = 10;
        }

        if (loaded) return;
		var _s = soundTemp.Instantiate();
		AddChild(_s);
		loaded = true;

    }
}
