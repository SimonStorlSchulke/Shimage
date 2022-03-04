using Godot;
using System;

public class FilterManager : Node {
    public static FilterManager instance;

    public override void _Ready() {
        instance = this;
    }

    public void BuildFiltersUI() {
        foreach (Node filterUI in GetChildren()) {
            filterUI.Free();
        }
        if (Apphandler.currentViewer.activeLayer == null)
            return;
        if (Apphandler.currentViewer.activeLayer.Filters == null)
            return;
        foreach (Filter cFilter in Apphandler.currentViewer.activeLayer.Filters) {
            AddChild(cFilter.BuildUI());
            GD.Print(cFilter.Props[0].Value);
            cFilter.UpdateUI();
        }
    }
}
