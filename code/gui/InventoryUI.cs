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
        Inventory = new Inventory(slotCount);
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlotUI slot = (InventorySlotUI)InventorySlotTemplate.Instantiate<Node>();
            slot.Name = "Slot " + (i + 1);
            this.AddChild(slot);
            UISlots.Add(slot);
        }
    }

    private void AddInteractable (Interactable item) {
        int itemIndex = Inventory.AddItem(item);
        if (itemIndex < 0 || itemIndex>UISlots.Count) {
            GD.PrintErr("No space in inventory for "+ item.Name);
            return;
        }
        UISlots.ElementAt(0).StoreItem(item);
        
    }

}

public class Inventory
{
    private List<InventorySpace> Spaces;
    public int SpaceCount => Spaces.Count;

    private Inventory() {
        Spaces = new();
    }

	public Inventory ( int spaceCount) {
        Spaces = new(spaceCount);
    }

    public bool SetSize(int spaceNumber)
    {
        if (spaceNumber == Spaces.Count || spaceNumber < 0) return true;
        if (spaceNumber > Spaces.Count)
        {
            while (spaceNumber > Spaces.Count)
                Spaces.Add(null);
            return true;
        }
        if (spaceNumber < Spaces.Count)
        {
            int emptySpaceCount = 0;
            Spaces.ForEach(space => emptySpaceCount += (space == null) ? 1 : 0);
            if (emptySpaceCount >= spaceNumber-Spaces.Count) {
				while (spaceNumber < Spaces.Count) {
                    var emptySpace = Spaces.Find(s => s == null);
                    Spaces.Remove(emptySpace);
                }
                return true;
            }
        }
        return false;
    }

	/// <summary>
	/// Adds an Interactable to the Inventory.
	/// </summary>
	/// <param name="item">The Interactable to be stored in the Inventory.</param>
	/// <returns>The position, if succesfull. -1, if no empty spaces left.</returns>
    public int AddItem(Interactable item)
    {
        //int stackIndex = AddItemToExistingStack(item);
        //if (stackIndex != -1)  return stackIndex;

        int index = 0;
        InventorySpace emptySpace = null;
        foreach(var space in Spaces) {
            if (space == null) {
                emptySpace = space;
                break;
            }
            index++;
        }
        if (emptySpace == null) return -1;

        emptySpace.Fill(item);
        return index;
    }

    private int AddItemToExistingStack(Interactable item)
    {
		// todo
        return -1;
    }
}

public class InventorySpace
{
    public Interactable Item = null; 
    public int Amount = 0;

    public void Fill(Interactable item) {
		//todo: dont fill if full, to avoid errors
        Item = item;
        Amount = 1;
    }
}
