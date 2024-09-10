using Godot;
using System;

[Tool]
public partial class Window : Control
{
    [Export] public bool ShowOnStart = false;
    private WindowManager wm;

    public void SetWindowManager(WindowManager windowManager)
    {
        wm = windowManager;
    }

    /// <summary> 
    /// Shows the window
    /// </summary>
    public void wShow()
    {
        Visible = true;
    }

    public void wShowOnly()
    {
        if(wm == null)
        {
            GD.PushWarning("Unable to use show only. Window manager is not set...");
            return;
        }
        wm.ShowWindow(this);
    }

    /// <summary>
    /// Hides the window
    /// </summary>
    public void wHide()
    {
        Visible = false;
    }

    /// <summary>
    /// Shows or Hides window
    /// </summary>
    /// <param name="active">determines if window shown</param>
    public void wSetActive(bool active)
    {
        Visible = active;
    }

    /// <summary>
    /// Gets the transforms name from GODOT_EDITOR
    /// </summary>
    /// <returns>(string) transform name of window</returns>
    public string wGetName()
    {
        return Name;
    }

    public Godot.Collections.Array AddInspectorButtons()
    {
        Godot.Collections.Array buttons = new Godot.Collections.Array();

        Godot.Collections.Dictionary show = new Godot.Collections.Dictionary
        {
            { "name", "Show Window" },
            { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
            { "pressed", Callable.From(Show)
            }
        };
        buttons.Add(show);

        Godot.Collections.Dictionary hide = new Godot.Collections.Dictionary
        {
            { "name", "Hide Window" },
            { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
            { "pressed", Callable.From(Hide)
            }
        };
        buttons.Add(hide);

        return buttons;
    }
}
