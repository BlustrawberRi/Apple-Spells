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
    }
}
