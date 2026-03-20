using System.ComponentModel;
using Godot;
using Godot.Collections;
[GlobalClass][Tool]
public partial class Inventory : Resource
{
    [Export] public int Capacity = 0;
    [Export] private Godot.Collections.Array<InventorySpace> Spaces = new();

    public int OccupiedSpaceCount;
    public int FreeSpaceCount => Capacity - OccupiedSpaceCount;


    public Inventory() : this(0) {  }
    public Inventory (int capacity)
    {
        Spaces = new Array<InventorySpace>();
        OccupiedSpaceCount = 0;
        for (int i = 0; i < capacity; i++)
            Spaces.Add(new());
        GD.Print("Made new Inventory with " + Capacity + " spaces.");
    }

    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].AsStringName() == PropertyName.Capacity)
        {
            GD.Print("Change");
            SetSize(Capacity);
        }
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
        if (newSpaceCount == Spaces.Count || newSpaceCount < 0) return true;
        if (newSpaceCount > Spaces.Count)
        {
            for (int i = 0; i < newSpaceCount - Spaces.Count; i++)
                Spaces.Add(new());

            return true;
        }
        if (newSpaceCount < Spaces.Count && FreeSpaceCount >= Spaces.Count - newSpaceCount)
        {
            for (int i = Spaces.Count - newSpaceCount - 1; i >= 0; i--)
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
