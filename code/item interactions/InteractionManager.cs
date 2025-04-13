using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;

[Tool]
public partial class InteractionManager : Area2D
{

    [Export]
    private Node2D ActiveItem;

    [Export]
    private Godot.Collections.Array<Node2D> InteractablesInRange;

    //[Signal]
    //public delegate void ItemCollisionEventHandler(StaticBody2D item);
    [Signal]
    public delegate void ItemInteractionEventHandler(Interactable item);

    private String ItemText;
    private Texture ItemTexture = null;
    private bool listenToInput = false;

    public override void _Ready() 
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExit;

        //BodyEntered += OnBodyEntered; // I dont need this if I connected it in editor
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!listenToInput) return;
        
        if (@event.IsActionReleased("Interact")){
            EmitSignal(SignalName.ItemInteraction, ActiveItem);
            this.GetViewport().SetInputAsHandled();
        }
    }

    public void UpdateActiveInteractable()
    {
        if (InteractablesInRange.Count == 0) return;

        ActiveItem = GetInteractableLookedAt();
        listenToInput = (ActiveItem is null)? false : true;

        GD.PrintRich("Active: [img]"+ (ActiveItem as Interactable).ItemTexture?.ResourcePath +"[/img] "+ ActiveItem.Name);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Interactable) return;

        InteractablesInRange.Add(body);
        UpdateActiveInteractable();
        //ActiveItem = GetBodyLookedAt();

        /*if (body is Plant) 
        {
            listenToInput = true;
            GD.PrintRich("Collided with: [img]"+ (body as Plant).PlantSprite?.Texture?.ResourcePath +"[/img] "+ (body as Plant).PlantType + " | Level " + (body as Plant).CurrentGrowthPhase); 
        }

        //if (body is Interactable)
        //{
            //EmitSignal(SignalName.ItemCollision, body as StaticBody2D);
            //if (body is not Interactable) return;

            //GetInteractableFeatures((StaticBody2D)body);
            listenToInput = true;
            GD.PrintRich("Collided with: [img]"+ (body as Interactable).ItemTexture?.ResourcePath+"[/img] "+ body.Name); 
        //}*/
    }

    private void OnBodyExit(Node2D body)
    {
        if (!InteractablesInRange.Contains(body))
            return;

        InteractablesInRange.Remove(body);
        UpdateActiveInteractable();
        /*if (ActiveItem == body) {
            ActiveItem = null;
            listenToInput = false;
        }*/

    }

    /// <summary>
    /// The Character moved.
    /// </summary>
    private void OnMcMoved(Vector2 velocity)
    {
        //todo: actually just update every 5 frames or so...
        UpdateActiveInteractable();
    }

    /// <summary>
    /// Returns the collided interactable with the smallest distance to the center of the InteractionManager area out of all collided interactables.
    /// </summary>
    /// <returns></returns>
    private Node2D GetInteractableLookedAt()
    {
        string interactableList = "[";
        foreach(var item in InteractablesInRange)
            {
                interactableList +="[img]"+(item as Interactable).TexturePath+"[/img]" 
                    + item.Name + " ";
            };
        GD.PrintRich(interactableList + "]");

        if (InteractablesInRange.Count == 0) return null;

        Node2D closestThing = InteractablesInRange[0];
        float distanceToThing = closestThing.GlobalPosition.DistanceSquaredTo(this.GlobalPosition);
        foreach(Node2D i in InteractablesInRange) {
            float distanceToI = i.GlobalPosition.DistanceSquaredTo(this.GlobalPosition);
            if (distanceToI < distanceToThing) {
                closestThing = i;
                distanceToThing = distanceToI;
            }
        }
        return closestThing;
    }


}
