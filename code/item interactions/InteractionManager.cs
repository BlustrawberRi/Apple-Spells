using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;

// todo: rename to InteractionSensor?
/// <summary>
/// It figures out what the entity touches.
/// </summary>
[Tool]
public partial class InteractionManager : Area2D
{

    [Export]
    private Interactable ActiveItem;

    [Export]
    private Godot.Collections.Array<Node2D> InteractablesInRange;

    //[Signal]
    //public delegate void ItemInteractionEventHandler(Interactable item);

    private String ItemText;
    private Texture ItemTexture = null;
    private bool listenToInput = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExit;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!listenToInput) return;


        if (@event.IsActionReleased("Interact"))
        {
            EventBus.InteractableEvents.InvokeItemInteractedEvent(ActiveItem);
            //EmitSignal(SignalName.Interacted, ActiveItem);
            this.GetViewport().SetInputAsHandled();
        }
        if (@event.IsActionReleased("Use"))
        {
            GD.Print("Use " + ActiveItem.Name);
            ActiveItem.React(null);
            EventBus.InteractableEvents.InvokeItemUsedEvent(ActiveItem);
            //Hand.React
            this.GetViewport().SetInputAsHandled();
        }
    }

    public bool UpdateActiveInteractable()
    {
        Interactable newActive = null;
        if (InteractablesInRange.Count != 0)
            newActive = GetLookedAtInteractable();

        if (newActive != ActiveItem)
        {
            newActive?.Highlight(true);
            ActiveItem?.Highlight(false);
            ActiveItem = newActive;
            listenToInput = ActiveItem is not null;

            _PrintInteractables();
            return true;
        }
        return false;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Interactable ) return;

        InteractablesInRange.Add(body);
        bool changed = UpdateActiveInteractable();
        if (!changed) _PrintInteractables();
    }

    private void OnBodyExit(Node2D body)
    {
        if (!InteractablesInRange.Contains(body))
            return;
        InteractablesInRange.Remove(body);
        bool changed = UpdateActiveInteractable();
        if (!changed) _PrintInteractables();
    }

    /// <summary>
    /// The Character moved.
    /// </summary>
    private void OnMcMoved(Vector2 velocity)
    {
        Rotation = -(velocity.Angle() + (float)Math.PI / (2.0f));
        //todo: actually just update every 5 frames or so...
        UpdateActiveInteractable();
    }

    /// <summary>
    /// Returns the collided interactable with the smallest distance to the center of the InteractionManager area out of all collided interactables.
    /// </summary>
    /// <returns></returns>
    private Interactable GetLookedAtInteractable()
    {
        if (InteractablesInRange.Count == 0) return null;

        Interactable closestThing = null;
        float distanceToThing = 0;
        foreach (Node2D i in InteractablesInRange)
        {
            if (i is not Interactable) continue;
            float distanceToI = 1 / i.GlobalPosition.DistanceSquaredTo(this.GlobalPosition);
            if (distanceToI > distanceToThing)
            {
                closestThing = i as Interactable;
                distanceToThing = distanceToI;
            }
        }

        return closestThing;
    }

    private void _PrintInteractables()
    {
        string interactableList = "[";
        foreach (var item in InteractablesInRange)
        {
            if (item == ActiveItem)
                interactableList += "[u]";

            interactableList += "[img]" + (item as Interactable).TexturePath + "[/img]"
                + item.Name + " ";

            if (item == ActiveItem)
                interactableList += "[/u]";
        }
        ;
        GD.PrintRich(interactableList + "]");
    }
}
