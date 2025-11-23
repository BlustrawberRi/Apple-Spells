using Godot;
using System.Collections;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

public partial class InventoryUI : Control
{
    /// <summary>
    /// The scene that holds the the slot.
    /// </summary>
    [Export]
    public PackedScene InventorySlotTemplate;

    [Export(PropertyHint.Range, "0,100")]
    public int slotCount = 5;

    private Inventory Inventory;
    private List<InventorySlotUI> UISlots = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CreateInventory();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        EventBus.InteractableEvents.HoldableReacted += AddInteractable;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        EventBus.InteractableEvents.HoldableReacted -= AddInteractable;
    }

    private void CreateInventory()
    {
        UISlots = new(slotCount);
        Inventory = new Inventory(slotCount);
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlotUI slot = (InventorySlotUI)InventorySlotTemplate.Instantiate<Node>();
            slot.Name = "Slot " + (i + 1);
            slot.SetSpace(Inventory.GetSpaceAt(i));
            this.AddChild(slot);
            UISlots.Add(slot);
        }
    }

    private void AddInteractable (Interactable item) {

        if (!Inventory.AddItem(item))
        {
            GD.PrintErr("No space in inventory for " + item.Name);
            return;
        }
    }

}

public class Inventory
{
    private List<InventorySpace> Spaces;
    public int Capacity => Spaces.Count;
    public int OccupiedSpaceCount;
    public int FreeSpaceCount => Capacity - OccupiedSpaceCount;

    private Inventory() {
        Spaces = new();
    }

    public Inventory(int spaceCount)
    {
        Spaces = new(spaceCount);
        OccupiedSpaceCount = 0;
        for(int i = 0; i<spaceCount; i++)
            Spaces.Add(new());
    }
    
    public InventorySpace GetSpaceAt(int index)
    {
        if (index+1 > Spaces.Count)
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
            for (int i = Capacity-newSpaceCount-1; i >= 0; i--)
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

public class InventorySpace
{
    public Interactable Item { private set; get; }

    public int Amount = 0;
    public event Action Changed;

    public void Fill (Interactable item) {
		//todo: dont fill if full, to avoid errors
        Item = item;
        Amount = 1;
        Changed?.Invoke();
    }
}
