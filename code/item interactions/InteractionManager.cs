using Godot;
using System;

public partial class InteractionManager : Area2D
{

    [Export]
    private Interactable ActiveItem;
    [Signal]
    public delegate void ItemCollisionEventHandler(StaticBody2D item);
    [Signal]
    public delegate void ItemInteractionEventHandler(Interactable item);

    private String ItemText;
    private Texture ItemTexture = null;
    private bool listenToInput = false;

    public override void _Ready() 
    {

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

    private void OnBodyEntered(Node2D body)
    {
        if(body is StaticBody2D)
        {
            EmitSignal(SignalName.ItemCollision, body as StaticBody2D);
            if (body is not Interactable) return;

            //GetInteractableFeatures((StaticBody2D)body);
            ActiveItem = body as Interactable;
            listenToInput = true;
            GD.PrintRich("Collided with: [img]"+ActiveItem.ItemTexture?.ResourcePath+"[/img] "+ body.Name); 
        }
    }
    private void OnBodyExit(Node2D body)
    {
        listenToInput = false;
    }


    private void GetInteractableFeatures(Interactable body)
    {
        //Variant variant = body.GetMeta("text");
        //ItemText = variant.AsString();
        //ItemText = (body as Interactable).ItemDescription;

        GD.PrintRich("Collided with: [img]"+ActiveItem.ItemTexture.ResourcePath+"[/img] "+ body.Name); 
        listenToInput = true;
        

    }

}
