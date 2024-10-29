using Godot;
using System;

public partial class InventoryWindow : Node
{
    [Export] int _slotsAmount;
    [Export] PackedScene _inventorySlotScene;
    [Export] Window _window;
    [Export] VBoxContainer _containerVertical;

    public override void _Ready()
    {
        _window.SizeChanged += RecalculateSlots;
        RecalculateSlots(); 
    }

    void RecalculateSlots()
    {
        foreach(var child in _containerVertical.GetChildren())
        {
            child.QueueFree();
        }
        
        double horizontalSlotsRaw = ((double)(_window.Size.X - 8)) / 100;
        int horizontalSlots = (int)System.Math.Round(horizontalSlotsRaw, MidpointRounding.ToZero);
        if (horizontalSlots != horizontalSlotsRaw)
        {
            Vector2I windowSize = _window.Size;
            windowSize.X = 8 + (horizontalSlots * 100);
            _window.Size = windowSize;
            return; // this function will be call again bc anyway...
        }

        double verticalSlotsRaw = ((double)_slotsAmount) / ((double)horizontalSlots);
        double verticalSlots = System.Math.Round(verticalSlotsRaw);
        if (verticalSlotsRaw.ToString().Contains(".") && verticalSlots < verticalSlotsRaw)
        {
            verticalSlots++;
        }

        int slotCounter = 0;
        for(int i = 0 ; i < verticalSlots; i++)
        {
            HBoxContainer containerH = new HBoxContainer();
            for(int j = 0 ; j < horizontalSlots; j++)
            {
                if (slotCounter < _slotsAmount)
                {
                    var slot = _inventorySlotScene.Instantiate();
                    containerH.AddChild(slot);
                    slotCounter++;
                }
            }
            containerH.AddThemeConstantOverride("separation", -2);
            _containerVertical.AddChild(containerH);
        }

    }
}
