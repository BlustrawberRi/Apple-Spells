using Godot;
using System;

[Tool]
public partial class SceneItemLoader : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
    public override void _EnterTree()
	{
		EventBus.UIEvents.InventoryAccepted += RemoveItemFromScene;
    }


	public void LoadItem(Interactable item)
	{
		GD.PrintErr(this, " Load Item not implemented.");
	}

	public void RemoveItemFromScene(Interactable item)
	{
		var children = GetChildren();

		foreach (var child in children)
		{
			if (child is Interactable && child == item)
			{
				(child as Interactable).Disable();
				return ;
			}

		}
	}
}
