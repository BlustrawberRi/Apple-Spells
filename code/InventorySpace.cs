using Godot;
using System;

[Tool][GlobalClass]
/// <summary>
/// Represents a container in the Inventory that can hold an amount of the same type of ItemInstance.
/// </summary>
public partial class InventorySpace : GodotObject
{
    [Export]
    public ItemInstance Item { private set; get; }

    public int Amount = 0;
    public event Action Changed;

    /// <summary>
    /// Saves only ItemInstance ref of an Interactable.
    /// </summary>
    /// <param name="item"></param>
    public void Fill(Interactable item)
    {
        //todo: dont fill if full, to avoid errors
        Item = item.ItemInstance;
        Amount = 1;
        Changed?.Invoke();
    }
}
