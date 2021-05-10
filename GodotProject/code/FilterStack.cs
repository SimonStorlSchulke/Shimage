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
        filterList.Add(new FilterHSL(0.5f, 0.5f, 0.5f));
        filterList.Add(new FilterOverlayColor(new Color(1, 1, 0)));

        BuildStack();
    }

}
