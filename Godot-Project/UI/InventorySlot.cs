using ArchitectsInVoid.Inventory;
using Godot;
using System;

public partial class InventorySlot : Control
{
    [Export] TextureRect _slotIcon;
    [Export] TextureButton _button;
    [Export] RichTextLabel _amountText;
    Item _item;

    public override void _Ready()
    {
        _button.Connect(BaseButton.SignalName.ButtonDown, Callable.From(Pressed));
    }

    void Pressed()
    {
        if (Input.IsMouseButtonPressed(MouseButton.Right))
        {
            // right mouse button needs to be able to split the stack later
            return;
        }
        InventoryManager.Singleton.SlotActivated(this);
    }

    public void SetItem(Item item)
    {
        _item = item;
        _amountText.Text = _item.ShortHandAmount();
        _slotIcon.Texture = _item.GetIcon();
    }

    public Item GetItem()
    {
        return _item;
    } 

}
