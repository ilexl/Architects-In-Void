using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class PopUp : Node
{
    [Export] Node holder;
	[Export] PackedScene errorPopUp;
	[Export] PackedScene confirmPopUp;
	[Export] PackedScene confirmPopUpCD;
    [Export] PackedScene infoPopUp;
    [Export] PackedScene infoPopUpNC;
    [Export] PackedScene inputPopUp;

    public string LastInput;

    void TestCall()
    {
        GD.Print("Testing complete");
    }


    public void DisplayError(string title, string message)
	{
        var inst = errorPopUp.Instantiate();
        holder.AddChild(inst);
		ErrorPopUp epu = (ErrorPopUp)inst;
        epu.Setup(title, message);
    }

	public void DisplayConfirmPopUp(string message, Callable confirmBind)
	{
        var inst = confirmPopUp.Instantiate();
        holder.AddChild(inst);
        ConfirmPopUp cpu = (ConfirmPopUp)inst;
        cpu.Setup(message, confirmBind);
    }

    public void DisplayConfirmPopUpCD(string message, double timerStart, Callable confirmBind, Callable cancelBind)
    {
        var inst = confirmPopUpCD.Instantiate();
        holder.AddChild(inst);
        ConfirmPopUpCD cpu = (ConfirmPopUpCD)inst;
        cpu.Setup(message, timerStart, confirmBind, cancelBind);
    }

    public void DisplayInfoPopUp(string message)
    {
        var inst = infoPopUp.Instantiate();
        holder.AddChild(inst);
        InfoPopUp ipu = (InfoPopUp)inst;
        ipu.Setup(message);
    }

    public InfoPopUp DisplayInfoPopUpNC(string message)
    {
        var inst = infoPopUpNC.Instantiate();
        holder.AddChild(inst);
        InfoPopUp ipu = (InfoPopUp)inst;
        ipu.Setup(message);
        return ipu;
    }

    public void DisplayInputPopUp(string message, Callable confirmBind)
    {
        var inst = inputPopUp.Instantiate();
        holder.AddChild(inst);
        InputPopUp ipu = (InputPopUp)inst;
        ipu.Setup(message, confirmBind);
    }
}
