using Godot;
using System;

public class FilterLevelsoffset : Filter
{
    public FilterLevelsoffset() {

        Name = "Exposure";

        type = FilterType.COLOR;
        
        Props = new Prop[] {
            new PropRGBA("levels_high", "White Level", new Color(0.5f, 0.5f, 0.5f ,1)),
            new PropRGBA("levels_low", "Black Level ", new Color(0.5f,0.5f,0.5f,1)),
        };

        Code = @"
    // Levels
    v3_1 = (levels_low.rgb - vec3(0.5)) * 2.0;
    fg = (fg - v3_1) / (levels_high.rgb * 2.0 - v3_1);
    ";
    }
}
