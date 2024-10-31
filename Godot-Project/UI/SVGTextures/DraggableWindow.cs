using Godot;
using System;

public partial class DraggableWindow : Window
{
    bool _focused = false;
    Vector2I _startPos, _newPos;
    public override void _Ready()
    {
    }

    private void Test(InputEvent @event)
    {
        GD.Print(@event);
    }

    /// <summary>
    /// Closes the window when the X is pressed
    /// </summary>
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            Visible = false;
            if(GetParent() is InventoryWindow iw)
            {
                iw.CallCloseFW();
            }
        }
        if (what == NotificationWMWindowFocusIn)
        {
            _focused = true;
            _startPos = GetPositionWithDecorations();
            GD.Print("DraggableWindow: focused window");
        }
        if (what == NotificationWMWindowFocusOut)
        {
            _focused = false;
            GD.Print("DraggableWindow: unfocused window");

        }
    }

    public override void _Input(InputEvent @event)
    {
        ArchitectsInVoid.UI.Pause.Singleton._Input(@event);
    }

    public override void _Process(double delta)
    {
        if (_focused) // force the window to stay on screen
        {
            if(_startPos == GetPositionWithDecorations())
            {
                return;
            }
            _newPos = GetPositionWithDecorations();
            if (GetPositionWithDecorations().X < 0)
            {
                _newPos.X = 10;
            }
            if (GetPositionWithDecorations().X > 2560 - GetSizeWithDecorations().X)
            {
                _newPos.X = (2560 - GetSizeWithDecorations().X) + 10;
            }
            if (GetPositionWithDecorations().Y < 0)
            {
                _newPos.Y = 35;
            }
            if (GetPositionWithDecorations().Y > 1440 - GetSizeWithDecorations().Y)
            {
                _newPos.Y = (1440 - GetSizeWithDecorations().Y) + 25;
            }
            if(_newPos != GetPositionWithDecorations())
            {
                GD.Print("DraggableWindow: exceeded screen size - moved within boundaries...");
                Position = _newPos;
            }
        }
    }

}
