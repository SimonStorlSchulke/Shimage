using Godot;
using System.Collections.Generic;

/// <summary> Filters may not define new Variables (as this would result of them not being reusable in the same shader)
/// use the pre-defined variables instead (f_1 - f_5, v3_1-v3_5...)
/// Limitation: Prop NameCodes CANNOT contain the whole name of another Variable (TODO: fix - Regex?)
/// </summary>
public class Filters {

    public static List<Filter> List = new List<Filter>() {
        new Filter(
           "Vignette", new Prop[] {
            new PropFloat("vignettePosX", "X", 0.5f),
            new PropFloat("vignettePosY", "Y", 0.5f),
            new PropFloat("vignettePower", "Power", 0.5f),
            new PropFloat("vignetteWidth", "Width", 0.4f),
            new PropFloat("vignetteBlur", "Blur", 0.3f),
            new PropFloat("spotlight", "Spotlight", 0.0f),
           },
           @"
        f_1 = distance(UV, vec2(vignettePosX, 1.0 - vignettePosY));
        f_1 = 1.0 - f_1;
		f_1 = map(f_1, 1.0 - (vignetteWidth + vignetteBlur / 2.0) * 1.5, 1.0 - vignetteWidth * 1.5 + vignetteBlur, 1.0-vignettePower, 1.0 + spotlight, true);
        f_1 = pow(f_1, 2.0);
        COLOR *= vec4(f_1,f_1,f_1,1);
    "),
    new Filter(
        "Exposure",
         new Prop[] {
            new PropFloatInf("exposure", "Fac", 1.0f),
        },
        @"
        COLOR *= vec4(exposure,exposure,exposure,1);
    "),
    new Filter(
        "Saturation",
         new Prop[] {
            new PropFloat("saturation", "Fac", 1.0f),
        },
        @"
        v3_1 = vec3(0.2125, 0.7154, 0.0721);
        v3_2 = vec3(dot(COLOR.rgb, v3_1));
        COLOR = mix(vec4(v3_2.rgb, 1.0), COLOR, saturation);
    "),

    new Filter(
        "Levels",
         new Prop[] {
            new PropRGBA("levels_low", "Black Level ", new Color(0,0,0,1)),
            new PropRGBA("levels_high", "White Level", new Color(1,1,1,1)),
        },
        @"
        COLOR = (COLOR - vec4(levels_low.rgb, 0.0)) / (vec4(levels_high.rgb, 1.0) - vec4(levels_low.rgb, 0.0));
    "),

    new Filter(

        "Overlay Color",
        new Prop[] {
            new PropRGBA("mcolor", "Color", new Color(1,1,1)),
            new PropFloatInf("mVal", "Fac", 1.0f),
        },
        @"
        COLOR *= mcolor * mVal;
    "),

    new Filter(
        "Hue Shift",
        new Prop[] {
            new PropFloat("hueshift", "Fac", 0.0f),
        },
        @"
        f_1 = cos(hueshift * PI * 2.0);
        f_2 = sin(hueshift * PI * 2.0);
		
    	v3_1.r = (.299f + .701f * f_1 + .168f * f_2) * COLOR.r
            + (.587f - .587f * f_1 + .330f * f_2) * COLOR.g
            + (.114f - .114f * f_1 - .497f * f_2) * COLOR.b;

        v3_1.g = (.299f - .299f * f_1 - .328f * f_2) * COLOR.r
            + (.587f + .413f * f_1 + .035f * f_2) * COLOR.g
            + (.114f - .114f * f_1 + .292f * f_2) * COLOR.b;

        v3_1.b = (.299f - .3f * f_1 + 1.25f * f_2) * COLOR.r
            + (.587f - .588f * f_1 - 1.05f * f_2) * COLOR.g
            + (.114f + .886f * f_1 - .203f * f_2) * COLOR.b;
        COLOR = vec4(v3_1.rgb, 1.0);
    "),
    
    };
}