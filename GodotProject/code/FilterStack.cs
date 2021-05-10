using Godot;
using System;
using System.Collections.Generic;

public class FilterStack : VBoxContainer {

    public static List<PointFilter> filterList = new List<PointFilter>();

    public void BuildStack() {
        foreach (PointFilter filter in filterList) {
            AddChild(filter.ui);
        }
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        filterList.Add(new FilterShiftHue(0.9f));
        filterList.Add(new FilterOverlayColor(new Color(0.8f, 0.5f, 1f)));
        filterList.Add(new FilterExposure(1.7f));
        BuildStack();
    }

}
