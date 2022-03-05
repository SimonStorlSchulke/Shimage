using Godot;
using System;

public class MbAddFilter : MenuButton {
    Tuple<string, string, Type>[] filterMenuList = new Tuple<string, string, Type>[] {
        new Tuple<string, string, Type>("Exposure", "Color", typeof(FilterExposure)),
        new Tuple<string, string, Type>("Vignette", "Color", typeof(FilterVignette)),
    };

    public override void _Ready() {
        int filterIdx = 0;
        foreach (var cFilterEntry in filterMenuList) {
            GetPopup().AddItem(cFilterEntry.Item1);
            filterIdx++;
        }
        GetPopup().Connect("id_pressed", this, nameof(AddFilterFromUI));
    }

    public void AddFilterFromUI(int filterIdx) {
        if(Apphandler.currentViewer.activeLayer != null) {
            Filter filterInstance = (Filter)Activator.CreateInstance(filterMenuList[filterIdx].Item3);
            Apphandler.currentViewer.activeLayer.AddFilter(filterInstance);
            FilterManager.BuildFiltersUI();
        }
    }

}
