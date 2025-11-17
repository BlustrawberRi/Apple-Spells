using Godot;
using System;


public partial class HoldableComponent : ItemComponent
{
    [Export]
    public bool IsStackable;

    [Export(hint: PropertyHint.Range, hintString: "1,999")]
    public int MaxStackCount = 1;

    public override void _Ready()
    {
        
    }

    public override void React(Interactable item)
    {
        EventBus.InteractableEvents.InvokeHoldableReacted(item/*, this*/);
    }
}
