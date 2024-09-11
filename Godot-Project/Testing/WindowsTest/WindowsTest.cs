using Godot;
using System;

[Tool]
public partial class WindowsTest : CanvasLayer
{
    Node win1, win2, win3;
    WindowManager wm;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        wm = (WindowManager)GetNode("WindowManager");
        if(wm != null )
        {
            win1 = GetNode(GetPath().ToString() + "/WindowManager/Window1");
            win2 = GetNode(GetPath().ToString() + "/WindowManager/Window2");
            win3 = GetNode(GetPath().ToString() + "/WindowManager/Window3");

            Node but1 = win1.GetNode("Button");
            Node but2 = win2.GetNode("Button");
            Node but3 = win3.GetNode("Button");

            but1.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Button1));
            but2.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Button2));
            but3.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Button3));
        }
        else
        {
            GD.PushError("No window manager found");
        }
    }

    void Button1()
    {
        wm.ShowWindow("Window2");
    }
    void Button2()
    {
        wm.ShowWindow("Window3");
    }
    void Button3()
    {
        wm.ShowWindow("Window1");
    }

    public Godot.Collections.Array AddInspectorButtons()
    {
        Godot.Collections.Array buttons = new Godot.Collections.Array();

        Godot.Collections.Dictionary wt = new Godot.Collections.Dictionary
        {
            { "name", "Get Windows (Children)" },
            { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
            { "pressed", Callable.From(_Ready) }
        };
        buttons.Add(wt);

        return buttons;
    }
}
