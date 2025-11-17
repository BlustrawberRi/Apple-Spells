using Godot;
using System;
using System.Dynamic;

public static class EventBus
{
    public struct InteractableEvents
    {
        public static Action<Interactable> ItemInteracted;
        public static void InvokeItemInteractedEvent(Interactable item)
        {
            ItemInteracted?.Invoke(item);
        }

        public static Action<Interactable> ItemUsed;

        public static void InvokeItemUsedEvent(Interactable item)
        {
            ItemUsed?.Invoke(item);
        }

        public static Action<Interactable> HoldableReacted; 
        public static void InvokeHoldableReacted(Interactable holdableItem) {

            HoldableReacted?.Invoke(holdableItem);
        }
    }
}
