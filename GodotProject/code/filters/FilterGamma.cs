using Godot;
using System;

public class FilterGamma : PointFilter {

    public FilterGamma(float gamma, Color color) {
        this.filterName = "Gamma";
        this.properties.Add("Gamma", new floatInf(gamma));
        this.properties.Add("Color", color);
        this.BuildUI();
    }

    protected override Color Operation(Color col) {
        Color gc = ((Color)this.properties["Color"]).Inverted();
        gc *= ((floatInf)this.properties["Gamma"]).val;
        col.r = Mathf.Pow(col.r, gc.r);
        col.g = Mathf.Pow(col.g, gc.g);
        col.b = Mathf.Pow(col.b, gc.b);
        return col;
    }
}