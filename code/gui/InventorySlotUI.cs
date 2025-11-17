using Godot;
using System;

public partial class InventorySlotUI : Control
{
    //[Export]
    private TextureRect itemTextureContainer;
    private Interactable storedItem;

    public override void _Ready()
	{
        itemTextureContainer = (TextureRect)this.FindChild("ItemTexture");
    }

	public void StoreItem (Interactable item) {
        storedItem = item;
        ChangeItemTexture(item.Text);
    }

	/// <summary>
	/// Changes the image of the item in the slot.
	/// </summary>
	/// <param name="itemTexture">The texture that will be applied.</param>
	public void ChangeItemTexture(Texture2D itemTexture){
        itemTextureContainer.Texture = itemTexture;
    }
}
