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
    
    public override void _Ready() {
        filterList.Add(new FilterHueShift(0.7f));
        filterList.Add(new FilterGamma(1.5f, new Color(0.2f, 0.1f, 0f)));
        filterList.Add(new FilterExposure(1.7f));
        BuildStack();
    }

}
