using Godot;
using Godot.Collections;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class WindowManager : Node
{
    [Export] private bool _startFeature = true;
    [Export] private Window[] _windows;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (!_startFeature) return;
        foreach (var window in _windows)
        {
            if (window == null) continue;
            if (window.ShowOnStart)
                window.Show();
            else
                window.Hide();
        }
    }

    public Array AddInspectorButtons()
    {
        var buttons = new Array();

        var gw = new Dictionary
        {
            { "name", "Get Windows (Children)" },
            { "icon", GD.Load("res://Testing/InspectorButtons/icon.svg") },
            { "pressed", Callable.From(GetWindowsInInspector) }
        };
        buttons.Add(gw);

        if (_windows == null || _windows.Length == 0) return buttons;

        foreach (var w in _windows)
        {
            if (w == null) continue;
            w.SetWindowManager(this);
            var windowButtonShowOnly = new Dictionary
            {
                { "name", $"Show Only {w.WGetName()}" },
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
        _windows = new Window[] { };
        var windowsAmount = 0;

        foreach (var child in GetChildren())
        {
            var w = child as Window;
            var successfullyFoundChildWindow = w != null;
            if (successfullyFoundChildWindow)
            {
                GD.Print($"Window: {w.WGetName()} added to {Name} successfully!");
                windowsAmount++;
            }
        }

        _windows = new Window[windowsAmount];
        var counter = 0;
        foreach (var child in GetChildren())
        {
            var w = child as Window;
            var successfullyFoundChildWindow = w != null;
            if (successfullyFoundChildWindow)
            {
                w.SetWindowManager(this);
                _windows[counter++] = w;
            }
        }
    }

    #region wmFunctions

    // Show window functions (hides all others)

    /// <summary>
    ///     Shows the individual window - Hides all other windows the WM manages
    /// </summary>
    /// <param name="window">The window to show</param>
    public void ShowWindow(Window window)
    {
        if (window == null) GD.PushError("Rebuild is needed for windows to work...");
        foreach (var _window in _windows)
            if (_window == window)
                _window.Show(); // Shows the required window
            else
                _window.Hide(); // Hides all others
    }

    /// <summary>
    ///     Shows the individual window - Hides all other windows the WM manages
    /// </summary>
    /// <param name="windowIndex">The index in windows to show</param>
    public void ShowWindow(int windowIndex)
    {
        if (windowIndex >= _windows.Length)
            GD.PushError($"Index out of range - {windowIndex} for Windows in WM...");
        else
            ShowWindow(_windows[windowIndex]);
    }

    /// <summary>
    ///     Shows the individual window - Hides all other windows the WM manages
    /// </summary>
    /// <param name="windowName">The transform name of the window to show</param>
    public void ShowWindow(string windowName)
    {
        var found = false;
        foreach (var window in _windows)
            if (window.WGetName() == windowName)
            {
                ShowWindow(window);
                found = true;
            }

        if (!found) GD.PushError($"Window: {windowName} NOT found...");
    }

    // Show window functions (leaves all others as current)

    /// <summary>
    ///     Shows the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="window">The window to show</param>
    public void ShowOnly(Window window)
    {
        if (window == null) GD.PushError("Rebuild is needed for windows to work...");
        foreach (var _window in _windows)
            if (_window == window)
                _window.Show(); // Shows the required window
    }

    /// <summary>
    ///     Shows the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="windowIndex">The index in windows to show</param>
    public void ShowOnly(int windowIndex)
    {
        if (windowIndex >= _windows.Length)
            GD.PushError($"Index out of range - {windowIndex} for Windows in WM...");
        else
            ShowOnly(_windows[windowIndex]);
    }

    /// <summary>
    ///     Shows the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="windowName">The transform name of the window to show</param>
    public void ShowOnly(string windowName)
    {
        var found = false;
        foreach (var window in _windows)
            if (window.WGetName() == windowName)
            {
                ShowOnly(window);
                found = true;
            }

        if (!found) GD.PushError($"Window not found - {windowName}");
    }

    // Hide All Windows Function

    /// <summary>
    ///     Hides all the windows managed by the WM
    /// </summary>
    public void HideAll()
    {
        foreach (var window in _windows)
        {
            if (window == null) GD.PushError("Rebuild is needed for windows to work...");
            window.Hide();
        }
    }

    // Hide Specific Window (Individually Hide)

    /// <summary>
    ///     Hides the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="window">The window to hide</param>
    public void HideOnly(Window window)
    {
        if (window == null) GD.PushError("Rebuild is needed for windows to work...");
        foreach (var thisWindow in _windows)
            if (thisWindow == window)
            {
                if (thisWindow == null) GD.PushError("Rebuild is needed for windows to work...");
                thisWindow.Hide(); // Shows the required window
            }
    }

    /// <summary>
    ///     Hides the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="windowIndex">The index in windows to Hide</param>
    public void HideOnly(int windowIndex)
    {
        if (windowIndex >= _windows.Length)
            GD.PushError($"Index out of range - {windowIndex} for Windows in WM...");
        else
            HideOnly(_windows[windowIndex]);
    }

    /// <summary>
    ///     Hides the individual window - Leaves all other windows the WM manages
    /// </summary>
    /// <param name="windowName">The transform name of the window to Hide</param>
    public void HideOnly(string windowName)
    {
        var found = false;
        foreach (var window in _windows)
            if (window.WGetName() == windowName)
            {
                HideOnly(window);
                found = true;
            }

        if (!found) GD.PushError($"Window not found - {windowName}");
    }

    /// <summary>
    ///     Gets all the windows as an array (readonly)
    /// </summary>
    /// <returns>WM windows</returns>
    public Window[] GetWindows()
    {
        return _windows;
    }

    #endregion
}