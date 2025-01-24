using Godot;
using System;


public partial class FilterHueShift : Filter {
    public FilterHueShift() {

        Name = "Exposure";

        type = FilterType.COLOR;

        Props = new Prop[] {
            new PropFloat("hueshift", "Fac", 0.0f),
        };

        Code = @"
    // Hue Shift
    f_1 = cos(hueshift * PI * 2.0);
    f_2 = sin(hueshift * PI * 2.0);
    
    v3_1.r = (.299f + .701f * f_1 + .168f * f_2) * fg.r
        + (.587f - .587f * f_1 + .330f * f_2) * fg.g
        + (.114f - .114f * f_1 - .497f * f_2) * fg.b;
    v3_1.g = (.299f - .299f * f_1 - .328f * f_2) * fg.r
        + (.587f + .413f * f_1 + .035f * f_2) * fg.g
        + (.114f - .114f * f_1 + .292f * f_2) * fg.b;
    v3_1.b = (.299f - .3f * f_1 + 1.25f * f_2) * fg.r
        + (.587f - .588f * f_1 - 1.05f * f_2) * fg.g
        + (.114f + .886f * f_1 - .203f * f_2) * fg.b;
    fg = v3_1;
    ";
    }
}
