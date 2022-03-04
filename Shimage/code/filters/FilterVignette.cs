using Godot;
using System;

public class FilterVignette : Filter
{
    public FilterVignette() {

        Name = "Exposure";

        type = FilterType.COLOR;
        
        Props = new Prop[]{
            new PropPosition("vignettePos", "Position", new Vector2(0.5f, 0.5f)),
            new PropFloat("vignettePower", "Power", 0.5f),
            new PropFloat("vignetteWidth", "Width", 0.4f),
            new PropFloat("vignetteBlur", "Blur", 0.3f),
            new PropFloat("spotlight", "Spotlight", 0.0f),
            new PropRGBA("vignetteColor", "Color", new Color(0,0,0,1)),
            new PropBool("vignetteUseDistortion", "Use Distortion", false)
        };

        Code = @"
    //Vignette
    if (vignetteUseDistortion) {
        f_1 = 1.0 - distance(fract(uv), vec2(vignettePos.x, vignettePos.y));
    } 
    else {
        f_1 = 1.0 - distance(UV, vec2(vignettePos.x, vignettePos.y));
    }
    f_2 = map_range(f_1, 1.0 - (vignetteWidth + (vignetteBlur + 0.001) / 2.0) * 1.5, 1.0 - vignetteWidth * 1.5 + vignetteBlur + 0.001, 0.0, 1.0, true);
    f_2 = smoothstep(0.0, 1.0, f_2);
    fg = mix(vignetteColor.rgb, fg, 1.0-(1.0-f_2)*vignettePower) * vec3(1.0 + f_2 * spotlight);
    ";
    }
}
