using Godot;
using System;
using System.Collections.Generic;

public class FilterStack : VBoxContainer {

    public static List<Filter> filterList = new List<Filter>();

    public void BuildStack() {
        foreach (Filter filter in filterList) {
            //AddChild(filter.ui);
            AddChild(filter.GetUI());
        }
    }
    
    public override void _Ready() {
        filterList.Add(Filters.Exposure);
        filterList.Add(Filters.MultiplyColor);
        filterList.Add(Filters.Vignette);
        filterList.Add(Filters.Vignette);

        BuildStack();
    }

}
