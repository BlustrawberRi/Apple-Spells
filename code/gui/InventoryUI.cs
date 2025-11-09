using Godot;
using System.Collections;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CreateInventory();
    }

    private void CreateInventory()
    {
        Inventory = new Inventory(slotCount);
        for (int i = 0; i < slotCount; i++)
        {
            Node slot = InventorySlotTemplate.Instantiate<Node>();
            slot.Name = "Slot " + (i + 1);
            this.AddChild(slot);
        }
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
	/// <returns>"True", if succesfull. "False", if no empty spaces left.</returns>
    public bool AddItem(Interactable item)
    {
        if (AddItemToExistingStack(item)) 
			return true;
        var emptySpace = Spaces.Find(s => s == null);
        if (emptySpace == null) return false;

        emptySpace.Fill(item);
        return true;
    }

    private bool AddItemToExistingStack(Interactable item)
    {
		// Todo
        return false;
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
