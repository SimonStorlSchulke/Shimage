using Godot;
using System;

public class Output : NodeBase
{
    public override void _Ready()
    {
        base._Ready();
        SetSlot(0, true, SLOTTYPE_RGBA, SLOTCOLOR_RGBA, false, 0, COLORBLACK);

        //TODO select Output to be viewed in viewport
        Parser.activeOutput = this;
    }

    public override void Execute() {

    }
}