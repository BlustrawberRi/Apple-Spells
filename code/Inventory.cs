using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Inventory : Resource
{
    private List<InventorySpace> Spaces;
    [Export]
    public int Capacity = 5;
    public int OccupiedSpaceCount;
    public int FreeSpaceCount => Capacity - OccupiedSpaceCount;

    private Inventory()
    {
        Spaces = new(Capacity);
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
            return true;
        }
        if (newSpaceCount < Capacity && FreeSpaceCount >= Capacity - newSpaceCount)
        {
            for (int i = Capacity - newSpaceCount - 1; i >= 0; i--)
            {
                int emptySpaceIndex = Spaces.FindLastIndex(space => space == null);
                Spaces.RemoveAt(emptySpaceIndex);
            }
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

        Spaces.Find(s => s.Item == null).Fill(item);
        OccupiedSpaceCount++;
        return true;
    }

    private bool AddItemToExistingStack(Interactable item)
    {
        // todo
        return false;
    }
}



public partial class InventorySpace : GodotObject
{
    public Interactable Item { private set; get; }

    public int Amount = 0;
    public event Action Changed;

    public void Fill(Interactable item)
    {
        //todo: dont fill if full, to avoid errors
        Item = item;
        Amount = 1;
        Changed?.Invoke();
    }
}
