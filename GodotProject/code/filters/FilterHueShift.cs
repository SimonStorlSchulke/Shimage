using Godot;
using System;

public class FilterHueShift : PointFilter {

    public FilterHueShift(float hue) {
        this.filterName = "Shift Hue";
        this.properties.Add("Hue", hue);
        this.BuildUI();
    }

    protected override Color Operation(Color col) {
        float U = Mathf.Cos((float)properties["Hue"] * Mathf.Pi * 2);
        float W = Mathf.Sin((float)properties["Hue"] * Mathf.Pi * 2);

        Color ret = new Color();
        ret.r = (.299f + .701f * U + .168f * W) * col.r
            + (.587f - .587f * U + .330f * W) * col.g
            + (.114f - .114f * U - .497f * W) * col.b;

        ret.g = (.299f - .299f * U - .328f * W) * col.r
            + (.587f + .413f * U + .035f * W) * col.g
            + (.114f - .114f * U + .292f * W) * col.b;

        ret.b = (.299f - .3f * U + 1.25f * W) * col.r
            + (.587f - .588f * U - 1.05f * W) * col.g
            + (.114f + .886f * U - .203f * W) * col.b;
        return ret;
    }
}