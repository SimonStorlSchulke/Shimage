using Godot;

public class FilterExposure : PointFilter {

    public FilterExposure(float value) {
        this.filterName = "Exposure";
        this.properties.Add("Exposure", new floatInf(value));
        this.BuildUI();
    }

    protected override Color Operation(Color col) {
        col *= ((floatInf)this.properties["Exposure"]).val;
        return col;
    }
}