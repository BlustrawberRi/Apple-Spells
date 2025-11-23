using Godot;
using System;

public partial class InventorySlotUI : Control
{
    //[Export]
    private TextureRect itemTextureContainer;
    //private Interactable storedItem;
    private InventorySpace inventorySpace;

    public override void _Ready()
	{
        itemTextureContainer = (TextureRect)this.FindChild("ItemTexture");
    }

    internal void SetSpace(InventorySpace inventorySpace)
    {
        this.inventorySpace = inventorySpace;
        inventorySpace.Changed += ChangeUI;
    }

	/// <summary>
	/// Changes the image of the item in the slot.
	/// </summary>
    private void ChangeUI()
    {
        itemTextureContainer.Texture = inventorySpace.Item.ItemTexture;
    }
}
