using Godot;
using System;

public partial class InventorySlotUI : Control
{
    //[Export]
    private TextureRect itemTextureContainer;
    //private Interactable storedItem;
    [Export]
    private InventorySpace inventorySpace;

    public override void _Ready()
	{
        itemTextureContainer = (TextureRect)this.FindChild("ItemTexture");
    }

    internal void SetSpace(InventorySpace inventorySpace)
    {
        if (inventorySpace == null)
        {
            GD.PrintErr("InventorySpace was null and cant create an Inventory Slot.");
            return;
        }
        this.inventorySpace = inventorySpace;
        inventorySpace.Changed += ChangeUI;
    }

	/// <summary>
	/// Changes the image of the item in the slot.
	/// </summary>
    private void ChangeUI()
    {
        itemTextureContainer.Texture = inventorySpace?.Item.Texture;
    }
}
