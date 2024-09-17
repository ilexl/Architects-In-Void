using Godot;
using System;

[Tool]
public partial class ConfirmPopUp : Node
{
    [Export] RichTextLabel _title;
    [Export] TextureButton _buttonClose;
    [Export] TextureButton _buttonConfirm;
    [Export] Callable call;
    public void Setup(string message, Callable confirmBind)
	{
        if (!_buttonClose.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Close)))
        {
            _buttonClose.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Close));
        }
        if (!_buttonConfirm.IsConnected(BaseButton.SignalName.ButtonDown, confirmBind))
        {
            _buttonConfirm.Connect(BaseButton.SignalName.ButtonDown, confirmBind);
        }
        if (!_buttonConfirm.IsConnected(BaseButton.SignalName.ButtonDown, Callable.From(Close)))
        {
            _buttonConfirm.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Close));
        }
        _title.Text = message;
    }

    void Close()
    {
        GetParent().RemoveChild(this);
    }
}
