using Godot;
using System;

[Tool]
public partial class WindowManager : Node
{
    [Export] private Window[] Windows;
    [Export] private bool startFeature = true;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (!startFeature) { return; }
        foreach (Window window in Windows)
        {
            if(window == null) { continue; }
            if (window.ShowOnStart)
            {
                window.Show();
            }
            else
            {
                window.Hide();
            }
        }
    }

    public Godot.Collections.Array AddInspectorButtons()
    {
        Godot.Collections.Array buttons = new Godot.Collections.Array();

        Godot.Collections.Dictionary gw = new Godot.Collections.Dictionary
        {
            { "name", "Get Windows (Children)" },
            { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
            { "pressed", Callable.From(GetWindowsInInspector) }
        };
        buttons.Add(gw);

        if(Windows == null || Windows.Length == 0)
        {
            return buttons;
        }

        foreach(Window w in Windows)
        {
            if(w == null) { continue; } 
            w.SetWindowManager(this);
            Godot.Collections.Dictionary windowButtonShowOnly = new Godot.Collections.Dictionary
            {
                { "name", $"Show Only {w.wGetName()}" },
                { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
                { "pressed", Callable.From(w.wShowOnly) }
            };
            buttons.Add(windowButtonShowOnly);
        }

        return buttons;
    }

    // Gets the windows for this manager to manage!
    public void GetWindowsInInspector()
    {
        Windows = new Window[] { };
        int windowsAmount = 0;

        foreach (var child in GetChildren())
        {
            Window w = child as Window;
            bool successfullyFoundChildWindow = (w != null);
            if (successfullyFoundChildWindow)
            {
                GD.Print($"Window: {w.wGetName()} added to {Name} successfully!");
                windowsAmount++;
            }
        }
        
        Windows = new Window[windowsAmount];
        int counter = 0;
        foreach (var child in GetChildren())
        {
            Window w = child as Window;
            bool successfullyFoundChildWindow = (w != null);
            if (successfullyFoundChildWindow)
            {
                w.SetWindowManager(this);
                Windows[counter++] = w;
            }
        }
        
    }
    #region wmFunctions

    // Show window functions (hides all others)

    /// <summary>
    /// Shows the individual window - Hides all other windows the WM manages
    /// </summary>
    /// <param name="window">The window to show</param>
    public void ShowWindow(Window window)
    {
        if(window == null)
        {
            GD.PushError("Rebuild is needed for windows to work...");
        }
        foreach (Window _window in Windows)
        {
            if (_window == window)
            {
                _window.Show(); // Shows the required window
            }
            else
            {
                _window.Hide(); // Hides all others
            }
        }
    }

    /// <summary>
    /// Shows the individual window - Hides all other windows the WM manages
    /// </summary>
    /// <param name="windowIndex">The index in windows to show</param>
    public void ShowWindow(int windowIndex)
    {
        if (windowIndex >= Windows.Length)
        {
            GD.PushError($"Index out of range - {windowIndex} for Windows in WM...");
        }
        else
        {
            ShowWindow(Windows[windowIndex]);
        }
    }

    /// <summary>
    /// Shows the individual window - Hides all other windows the WM manages
    /// </summary>
    /// <param name="windowName">The transform name of the window to show</param>
    public void ShowWindow(string windowName)
    {
        bool found = false;
        foreach (Window window in Windows)
        {
            if (window.wGetName() == windowName)
            {
                ShowWindow(window);
                found = true;
            }
        }
        if (!found)
        {
            GD.PushError($"Window: {windowName} NOT found...");
        }
    }

    // Show window functions (leaves all others as current)

    /// <summary>
    /// Shows the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="window">The window to show</param>
    public void ShowOnly(Window window)
    {
        if (window == null)
        {
            GD.PushError("Rebuild is needed for windows to work...");
        }
        foreach (Window _window in Windows)
        {
            if (_window == window)
            {
                _window.Show(); // Shows the required window
            }
        }
    }

    /// <summary>
    /// Shows the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="windowIndex">The index in windows to show</param>
    public void ShowOnly(int windowIndex)
    {
        if (windowIndex >= Windows.Length)
        {
            GD.PushError($"Index out of range - {windowIndex} for Windows in WM...");
        }
        else
        {
            ShowOnly(Windows[windowIndex]);
        }
    }

    /// <summary>
    /// Shows the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="windowName">The transform name of the window to show</param>
    public void ShowOnly(string windowName)
    {
        bool found = false;
        foreach (Window window in Windows)
        {
            if (window.wGetName() == windowName)
            {
                ShowOnly(window);
                found = true;
            }
        }
        if (!found)
        {
            GD.PushError($"Window not found - {windowName}");
        }
    }

    // Hide All Windows Function

    /// <summary>
    /// Hides all the windows managed by the WM
    /// </summary>
    public void HideAll()
    {
        foreach (Window window in Windows)
        {
            if (window == null)
            {
                GD.PushError("Rebuild is needed for windows to work...");
            }
            window.Hide();
        }
    }

    // Hide Specific Window (Individually Hide)

    /// <summary>
    /// Hides the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="window">The window to hide</param>
    public void HideOnly(Window window)
    {
        if (window == null)
        {
            GD.PushError("Rebuild is needed for windows to work...");
        }
        foreach (Window _window in Windows)
        {
            if (_window == window)
            {
                if (window == null)
                {
                    GD.PushError("Rebuild is needed for windows to work...");
                }
                _window.Hide(); // Shows the required window
            }
        }
    }

    /// <summary>
    /// Hides the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="windowIndex">The index in windows to Hide</param>
    public void HideOnly(int windowIndex)
    {
        if (windowIndex >= Windows.Length)
        {
            GD.PushError($"Index out of range - {windowIndex} for Windows in WM...");
        }
        else
        {
            HideOnly(Windows[windowIndex]);
        }
    }

    /// <summary>
    /// Hides the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="windowName">The transform name of the window to Hide</param>
    public void HideOnly(string windowName)
    {
        bool found = false;
        foreach (Window window in Windows)
        {
            if (window.wGetName() == windowName)
            {
                HideOnly(window);
                found = true;
            }
        }
        if (!found)
        {
            GD.PushError($"Window not found - {windowName}");
        }
    }

    /// <summary>
    /// Gets all the windows as an array (readonly)
    /// </summary>
    /// <returns>WM windows</returns>
    public Window[] GetWindows()
    {
        return Windows;
    }
    #endregion

    
}
