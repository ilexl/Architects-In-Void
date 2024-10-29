using ArchitectsInVoid.UI;
using Godot;
using System;
using System.Runtime.InteropServices;

[Tool]
public partial class InventoryManager : Node
{
    [Export] PackedScene _inventoryUIWindow;
    [Export] Control _inventoryWindowParent;
    [Export] Control _inventoriesList;
    [Export] bool _inventoryListShown;

    public override void _Ready()
    {
        _inventoriesList.Hide();
        _inventoryListShown = false;
    }

    void TryShowInventoryList()
    {
        GD.Print("InventoryManager: trying to show list of inventories");
        if (GameManager.Singleton.CurrentGameState == GameManager.GameState.MainMenu)
        {
            GD.Print("InventoryManager: cant show list while in main menu");
            return;
        }
        _inventoryListShown = true;
        _inventoriesList.Show();
        GD.Print("InventoryManager: list of inventories now being shown");

    }
    public void HideInventoryList()
    {
        _inventoriesList.Hide();
        _inventoryListShown = false;
        GD.Print("InventoryManager: list of inventories now being hidden");
    }

    public override void _Input(InputEvent @event)
    {
        if (GameManager.Singleton.CurrentGameState == GameManager.GameState.MainMenu)
        {
            return;
        }
        if(Pause.Singleton.IsPaused == true)
        {
            return;
        }
        if (@event.IsActionPressed("interactions_inventory"))
        {
            if (_inventoryListShown)
            {
                HideInventoryList();
            }
            else
            {
                TryShowInventoryList();
            }
        }
    }


}
