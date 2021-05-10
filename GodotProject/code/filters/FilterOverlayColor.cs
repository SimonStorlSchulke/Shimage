using Godot;
using System;

public class FilterOverlayColor : PointFilter {

    public FilterOverlayColor(Color overlayColor) {
        this.filterName = "Overlay Color";
        this.properties.Add("overlayColor", overlayColor);
        this.BuildUI();
    }

    protected override Color Operation(Color col) {
        col *= (Color)this.properties["overlayColor"];
        return col;
    }
}