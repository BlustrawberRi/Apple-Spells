using Godot;
using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

public partial class ItemDialogue : MarginContainer
{

	[Export]
	public String Text 
	{
		get
		{
			return _text;
		} 
		set
		{ 
			_text = value;
			if (TextLabel == null) return;
			TextLabel.Text = value;
		}
	}

    private String _text;

	[Export]
	public Texture Texture 
	{
		get
		{
			return _texture;
		} 
		set
		{ 
			_texture = value;
			if (TextureContainer == null) return;
			TextureContainer.Set("texture", value);
		}
	}
	private Texture _texture; //todo: show the name of the object somewhere

	[Export]
    public StringName InteractableName;

	private RichTextLabel TextLabel {get; set;}
	private TextureRect TextureContainer {get; set;}
	private TextureRect TextIndicatorContainer {get;set;}
	private int currentLine = 0;

	private const bool CONTINUE_INDICATOR = true;
	private const bool CLOSE_INDICATOR = false;


    public override void _EnterTree()
    {
        Visible = true;
    }
    public override void _Ready()
    {
		Text = "";
		Texture = null;
		Visible = false;

		TextLabel = (RichTextLabel)GetNode<RichTextLabel>("ItemContainer/HBoxContainer/MarginContainer/ItemText");
		TextureContainer = (TextureRect)GetNode<TextureRect>("ItemContainer/HBoxContainer/ItemTexture");
		TextIndicatorContainer = GetNode<TextureRect>("ItemContainer/HBoxContainer/TextIndicator/TextIndicatorTexture");
    }
	

    private async Task OpenDialogue()
    {
		if (Visible) return;

        bool hasText = (Text is not "") && (Text is not null);
        if (!hasText)
        {
            GD.PrintRich("[color=#bb9922]No interaction, because Item Description is missing[/color] for " + InteractableName);
            return;
        }

        Visible = true;

        //waiting for dialogue to be drawn and processed, so the visible line count is correct
        await ToSignal(TextLabel, Godot.RichTextLabel.SignalName.Draw);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        GetNode<AudioStreamPlayer>("DialogueOpenAudio")?.Play();

        int visibleLineCount = TextLabel.GetVisibleLineCount();
        int totalLineCount = TextLabel.GetLineCount();

        if (visibleLineCount == totalLineCount)
        {
            SetIndicatorIcon(CLOSE_INDICATOR);
        }
        else
        {
            SetIndicatorIcon(CONTINUE_INDICATOR);
            AddEmptyLinesToEnd(visibleLineCount, totalLineCount);
        }
    }

    public void Close()
    {
		if (!Visible) return;

        GetNode<AudioStreamPlayer>("DialogueCloseAudio")?.Play();
        Visible = false;
    }

    private async void OnItemInteraction(Interactable item)
    {
        Texture = item.ItemTexture;
        Text = item.ItemDescription;
        InteractableName = item.Name;

        await OpenDialogue();

        _PrintDialogueText();
    }


    private void SetIndicatorIcon(bool continueIndicator)
    {
        AnimationPlayer animationPlayer = TextIndicatorContainer.GetNode<AnimationPlayer>("AnimationPlayer");
		if (continueIndicator)
		{
			animationPlayer.Play("text_continue");
		} else
		{
			animationPlayer.Play("text_stop");
		}
    }

    /// <summary>
    /// Scoll the <ref>TextLabel</ref> to see the next page of lines.
    /// </summary>
    /// <param name="visibleLineCount">The number of lines visible in the dialogue at a time.</param>
    /// <param name="totalLineCount">The total number of lines that are to be shown in the dialogue.</param>
    /// <returns></returns>
    private bool ScrollToNextLines(int visibleLineCount, int lineCount)
    {
        currentLine += visibleLineCount;
        if (currentLine < lineCount)
        {
            TextLabel.ScrollToLine(currentLine);
			if (currentLine+visibleLineCount >= lineCount)
				SetIndicatorIcon(CLOSE_INDICATOR);
			return true;
        }
        else
        {
            currentLine = 0;
            TextLabel.ScrollToLine(currentLine);
			return false;
        }
    }


	/// <summary>
	/// Fills up the Text with empty lines, so when scrolling to the last part you will not see lines that were previously shown before. The amount of total lines will be evenly divided by the amount of visible lines.
	/// </summary>
	/// <param name="visibleLineCount">The number of lines visible in the dialogue at a time.</param>
	/// <param name="totalLineCount">The total number of lines that are to be shown in the dialogue.</param>
    private void AddEmptyLinesToEnd(int visibleLineCount, int totalLineCount)
    {
        int linesTooMany = totalLineCount % visibleLineCount;

        Text += new string('\n', (visibleLineCount - linesTooMany) % visibleLineCount);
    }


    //this is for UI Input (before _unhandled_input())
    public override void _Input(InputEvent @event)
    {
        if (!Visible) return;

        if (@event.IsActionReleased("Interact"))
        {
            int visibleLineCount = TextLabel.GetVisibleLineCount();
            int lineCount = TextLabel.GetLineCount();

            if (!ScrollToNextLines(visibleLineCount, lineCount))
            {
                Close();
            }

            this.GetViewport().SetInputAsHandled();
        }
    }

    private void _PrintDialogueText()
    {
        GD.Print("There are " + TextLabel.GetVisibleLineCount() + "/" + TextLabel.GetLineCount() + " Lines visible in the dialogue");
        GD.PrintRich("[img]" + Texture?.ResourcePath + "[/img] " + Text); // show this on interaction
    }

	
}
