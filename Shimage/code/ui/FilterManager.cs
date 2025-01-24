using Godot;
using System;

public partial class FilterManager : Node {
    public static FilterManager instance;

    public override void _Ready() {
        instance = this;
    }

    public static void BuildFiltersUI() {
        foreach (Node filterUI in instance.GetChildren()) {
            filterUI.Free();
        }
        if (Apphandler.currentViewer.activeLayer == null)
            return;
        if (Apphandler.currentViewer.activeLayer.Filters == null)
            return;
        foreach (Filter cFilter in Apphandler.currentViewer.activeLayer.Filters) {
            instance.AddChild(cFilter.BuildUI());
            GD.Print(cFilter.Props[0].Value);
            cFilter.UpdateUI();
        }
    }
}
