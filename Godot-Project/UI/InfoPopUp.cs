using Godot;
using System;

[Tool]
public partial class InfoPopUp : Node
{
    [Export] RichTextLabel _title;
    [Export] TextureButton _buttonClose;
    public void Setup(string message)
    {
        if (!_buttonClose.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Close)))
        {
            _buttonClose.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Close));
        }
        _title.Text = message;
    }

    void Close()
    {
        GetParent().RemoveChild(this);
    }
}
