using Godot;
using System;

public class NodeMix : NodeBase {
    PointFilter filter = new ExampleFilter(new Color(1, 0, 0));

    public NodeMix() {
        nodename = "Mix";
        SetSlot(0, true, SLOTTYPE_RGBA, SLOTCOLOR_RGBA,
                    true, SLOTTYPE_RGBA, SLOTCOLOR_RGBA);

        SetSlot(1, true, SLOTTYPE_RGBA, SLOTCOLOR_RGBA, false, 0, COLORBLACK);
        SetSlot(2, true, SLOTTYPE_FLOAT, SLOTCOLOR_FLOAT, false, 0, COLORBLACK);
    }

    public override void Execute() {

    }
}
