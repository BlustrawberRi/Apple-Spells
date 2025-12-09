using Godot;
using System;

/// <summary>
/// This Component will allow the item to be held by the player. An Inventory should handle the EventBus.InteractableEvents.HoldableReacted(Interactable item) event this component will invoke. Does need an empty interactionSource to work.
/// </summary>
[GlobalClass]
public partial class HoldableComponent : ItemComponent
{
    [Export]
    public bool IsStackable;

    [Export(hint: PropertyHint.Range, hintString: "1,999")]
    public int MaxStackCount = 1;


    public override bool CanInteract(Interactable interactionSource)
    {
        if (interactionSource != null) return false;

        return true;
    }

    public override void React(Interactable interactionSource, Interactable item)
    {
        EventBus.InteractableEvents.InvokeHoldableReacted(item/*, this*/);
    }
}
