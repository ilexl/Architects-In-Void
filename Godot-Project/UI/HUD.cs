using Godot;
using System.Reflection.Metadata;
using ArchitectsInVoid.Player.HotBar;

namespace ArchitectsInVoid.UI;

[Tool]
public partial class HUD : Node
{
    [ExportGroup("ItemSlots")]
    [Export] Control[] _itemSlots;
    [Export] Control[] _itemSlotSelections;
    [Export] Control[] _itemSlotIcons;
    [ExportGroup("HotbarSelection")]
    [Export] Control[] _hotbarSelections;
    [Export] Control[] _hotbarSelectionEnabled;
    int _currentItemSlot, _currentHotbar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if(_itemSlots.Length != 10 || _itemSlotSelections.Length != 10 || _itemSlotIcons.Length != 10)
        {
            GD.PushError("HUD: item slots not configured correctly in editor...");
        }
        if (_hotbarSelections.Length != 10 || _hotbarSelectionEnabled.Length != 10)
        {
            GD.PushError("HUD: hotbar selection not configured correctly in editor...");
        }
        SelectItemSlot(0);
        SelectHotbar(1);
        HotBarManager.HotbarSlotChangedEvent += HotbarSlotChanged;
    }

    public int GetHotbar()
    {
        return _currentHotbar;
    }
    public int GetItemSlot()
    {
        return _currentItemSlot;
    }

    public void SelectItemSlot(int slot)
    {
        _currentItemSlot = slot;
        if (_itemSlotSelections == null || _itemSlotSelections.Length != 10) { return; }
        foreach(var item in _itemSlotSelections)
        {
            item.Visible = false;
        }
        _itemSlotSelections[slot].Visible = true;
    }

    public void SelectHotbar(int hotbar)
    {
        _currentHotbar = hotbar;
        if (_hotbarSelectionEnabled == null || _hotbarSelectionEnabled.Length != 10) { return; }
        foreach (var item in _hotbarSelectionEnabled)
        {
            item.Visible = false;
        }
        _hotbarSelectionEnabled[hotbar].Visible = true;
    }

    public void SelectHotbar(bool increase)
    {
        if (increase)
        {
            if(_currentHotbar == 9 || _currentHotbar > 9)
            {
                SelectHotbar(0);
                return;
            }
            else
            {
                SelectHotbar(_currentHotbar + 1);
                return;
            }
        }
        else
        {
            if (_currentHotbar == 0 || _currentHotbar < 0)
            {
                SelectHotbar(9);
                return;
            }
            else
            {
                SelectHotbar(_currentHotbar - 1);
                return;
            }
        }
    }

    private void HotbarSlotChanged(int index)
    {
        SelectItemSlot(index);
    }
    
    public override void _Input(InputEvent @event)
    {
        if(GameManager.Singleton == null)
        {
            return;
        }
        if(GameManager.Singleton.CurrentGameState == GameManager.GameState.InGame)
        {
            for(int i = 0; i != 10; i++)
            {
                string inputName = $"toolbar_equip_slot_{i.ToString()}";
                if (@event.IsActionPressed(inputName))
                {
                    SelectItemSlot(i);
                }
            }
            if (@event.IsActionPressed("toolbar_next_toolbar"))
            {
                SelectHotbar(true);
            }
            if (@event.IsActionPressed("toolbar_previous_toolbar"))
            {
                SelectHotbar(false);
            }
        }
    }
}