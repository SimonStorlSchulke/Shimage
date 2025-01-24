using Godot;
using System;

public partial class MbAddFilter : MenuButton {
    Tuple<string, string, Type>[] filterMenuList = new Tuple<string, string, Type>[] {
        new Tuple<string, string, Type>("Exposure", "Color", typeof(FilterExposure)),
        new Tuple<string, string, Type>("Vignette", "Color", typeof(FilterVignette)),
        new Tuple<string, string, Type>("Vibrancy", "Color", typeof(FilterVibrancy)),
        new Tuple<string, string, Type>("Hue Shift", "Color", typeof(FilterHueShift)),
        new Tuple<string, string, Type>("Filmic Tonemap", "Color", typeof(FilterFilmicTonemap)),
        new Tuple<string, string, Type>("Saturation", "Color", typeof(FilterSaturation)),
        new Tuple<string, string, Type>("Levels", "Color", typeof(FilterLevelsoffset)),
        new Tuple<string, string, Type>("Noise Distort", "Color", typeof(FilterDistortNoise)),
    };

    public override void _Ready() {
        int filterIdx = 0;
        foreach (var cFilterEntry in filterMenuList) {
            GetPopup().AddItem(cFilterEntry.Item1);
            filterIdx++;
        }
        GetPopup().Connect("id_pressed", new Callable(this, nameof(AddFilterFromUI)));
    }

    public void AddFilterFromUI(int filterIdx) {
        if(Apphandler.currentViewer.activeLayer != null) {
            Filter filterInstance = (Filter)Activator.CreateInstance(filterMenuList[filterIdx].Item3);
            Apphandler.currentViewer.activeLayer.AddFilter(filterInstance);
            FilterManager.BuildFiltersUI();
        }
    }

}
