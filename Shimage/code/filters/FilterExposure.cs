using Godot;
using System;


public partial class FilterExposure : Filter
{
    public FilterExposure() {

        Name = "Exposure";

        type = FilterType.COLOR;
        
        Props = new Prop[]{
            new PropFloat("exposure", "Factor", 1.0f, false, 0, 1000),
        };

        Code = @"
        // Exposure
        fg *= vec3(exposure, exposure, exposure);
        ";
    }
}
