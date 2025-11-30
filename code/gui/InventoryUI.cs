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

    [Export]
    private Inventory Inventory;

    [Export]
    private AudioStreamPlayer audioPlayer;
    private List<InventorySlotUI> UISlots = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BuildInventory();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        if (Inventory != null) 
            EventBus.InteractableEvents.HoldableReacted += AddInteractable;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (Inventory != null) 
            EventBus.InteractableEvents.HoldableReacted -= AddInteractable;
    }

    private void BuildInventory()
    {
        if (Inventory == null)
        {
            GD.PushWarning("Inventory " + this.Name + " can't be built: No Inventory ressource connected.");
            return;
        }

        UISlots = new(Inventory.Capacity);
        for (int i = 0; i < Inventory.Capacity; i++)
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
        EventBus.UIEvents.InvokeInventoryAccepted(item);
        PlaySound();
    }

    private void PlaySound()
    {
        audioPlayer?.Play();
    }
}


