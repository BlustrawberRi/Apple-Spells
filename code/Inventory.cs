using Godot;
using System;
using System.Collections.Generic;
using System.Xml;

[GlobalClass]
public partial class Inventory : Resource
{
    [Export]
    private Godot.Collections.Array<InventorySpace> Spaces;

    [Export]
    public int Capacity = 5;
    public int OccupiedSpaceCount;
    public int FreeSpaceCount => Capacity - OccupiedSpaceCount;

    private Inventory()
    {
        Spaces = new Godot.Collections.Array<InventorySpace>();
        OccupiedSpaceCount = 0;
        for (int i = 0; i < Capacity; i++)
            Spaces.Add(new());
        GD.Print("Made new Inventory with " + Capacity + " spaces.");
    }

    public InventorySpace GetSpaceAt(int index)
    {
        if (index + 1 > Spaces.Count)
        {
            GD.PushWarning("Not enough Spaces in this Inventory than the index asked for.");
            return null;
        }
        return Spaces[index];
    }

    public bool SetSize(int newSpaceCount)
    {
        if (newSpaceCount == Capacity || newSpaceCount < 0) return true;
        if (newSpaceCount > Capacity)
        {
            for (int i = 0; i < newSpaceCount - Capacity; i++)
                Spaces.Add(new());

            Capacity = newSpaceCount;
            return true;
        }
        if (newSpaceCount < Capacity && FreeSpaceCount >= Capacity - newSpaceCount)
        {
            for (int i = Capacity - newSpaceCount - 1; i >= 0; i--)
            {
                foreach (var space in Spaces)
                {
                    if (space.Item == null)
                    {
                        Spaces.Remove(space);
                        break;
                    }
                }
            }
            Capacity = newSpaceCount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds an Interactable to the Inventory.
    /// </summary>
    /// <param name="item">The Interactable to be stored in the Inventory.</param>
    /// <returns>The position, if succesfull. -1, if no empty spaces left.</returns>
    public bool AddItem(Interactable item)
    {
        //int stackIndex = AddItemToExistingStack(item);
        //if (stackIndex != -1)  return stackIndex;

        if (FreeSpaceCount == 0)
            return false;

        foreach (var space in Spaces)
        {
            if (space.Item == null)
            {
                space.Fill(item);
                OccupiedSpaceCount++;
                return true;
            }
        }
        GD.PushError("Free Space count isnt 0, but there seems to be no space that has no item in " + this.ResourceName);
        return false;
    }

    private bool AddItemToExistingStack(Interactable item)
    {
        // todo
        return false;
    }
}



public partial class InventorySpace : GodotObject
{
    [Export]
    public ItemInstance Item { private set; get; }

    public int Amount = 0;
    public event Action Changed;

    public void Fill(Interactable item)
    {
        //todo: dont fill if full, to avoid errors
        Item = item.ItemInstance;
        Amount = 1;
        Changed?.Invoke();
    }
}
