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
            new PropRGBA("vignetteColor", "Color", new Color(0,0,0,1)),
            new PropBool("vignetteUseDistortion", "Use Distortion", false)
           },
           @"
    //Vignette
    if (vignetteUseDistortion) {
        f_1 = 1.0 - distance(fract(uv), vec2(vignettePosX, 1.0 - vignettePosY));
    } 
    else {
        f_1 = 1.0 - distance(UV, vec2(vignettePosX, 1.0 - vignettePosY));
    }
    f_2 = map(f_1, 1.0 - (vignetteWidth + (vignetteBlur + 0.001) / 2.0) * 1.5, 1.0 - vignetteWidth * 1.5 + vignetteBlur + 0.001, 0.0, 1.0, true);
    f_2 = f_2 * f_2;
    COLOR = mix(vignetteColor, COLOR, 1.0-(1.0-f_2)*vignettePower) * vec4(1.0 + f_2 * spotlight);
    ", FilterType.COLOR),
    
    new Filter(
        "Exposure",
         new Prop[] {
            new PropFloat("exposure", "Fac", 1.0f, _slider: false, _max: 1000),
        },
        @"
        // Exposure
        COLOR *= vec4(exposure,exposure,exposure,1);
    ", FilterType.COLOR),

    new Filter(
        "Filmic Tonemap",
         new Prop[] {
            new PropFloat("fm_brighthess", "Brightness", 0.8f, _slider: false, _max: 1000),
            new PropFloat("fm_toe", "Toe", 5f, _slider: false, _max: 1000),
            new PropFloat("fm_shoulder", "Shoulder", 1.8f, _slider: false, _max: 1000),
            new PropFloat("fm_add", "Add", 0.15f, _slider: false, _max: 1000),
        },
        @"
        // Filmic Tonemap
        COLOR *= vec4(fm_brighthess,fm_brighthess,fm_brighthess,1);

        v3_1 = max(vec3(0.0), COLOR.rgb - 0.004);
        v3_2 = (v3_1 * (6.2 * v3_1 + fm_brighthess)) / (v3_1 * (fm_toe * v3_1 + fm_shoulder) + fm_add);
        v3_2 = pow(v3_2, vec3(2.2));
        COLOR = vec4(v3_2.rgb, 1.0);

    ", FilterType.COLOR),

    new Filter(
        "Saturation",
         new Prop[] {
            new PropFloat("sat", "Fac", 0.5f, _max: 2.0f),
        },
        @"
    // Saturation
    v3_1 = vec3(0.2125, 0.7154, 0.0721);
    v3_2 = vec3(dot(COLOR.rgb, v3_1));
    COLOR = mix(vec4(v3_2.rgb, 1.0), COLOR, sat * 2.0);
    ", FilterType.COLOR),

    new Filter(
        "Vibrancy",
         new Prop[] {
            new PropFloat("vib_fac", "Fac", 0.5f, _max: 2.0f),
            new PropFloat("vib_pow", "Raise Low Sat", 2.0f, _slider: false, _max: 10),
        },
        @"
    // Vibrancy

    f_1 = COLOR.r*0.299 + COLOR.g*0.587 + COLOR.b*0.114; //lum
    f_2 = min(min(COLOR.r, COLOR.g), COLOR.b); //mn
    f_3 = max(max(COLOR.r, COLOR.g), COLOR.b); //mx
    f_4 = (1.0-(f_3 - f_2)) * (1.0-f_3) * f_1 * 5.0; //sat

    v3_1 = vec3(0.2125, 0.7154, 0.0721);
    v3_2 = vec3(dot(COLOR.rgb, v3_1));
    f_5 = pow(f_4, vib_pow) * vib_fac;
    COLOR = mix(vec4(v3_2.rgb, 1.0), COLOR, 1.0 + clamp(f_5, 0.0, 1.0));

    ", FilterType.COLOR),

    new Filter(
        "Levels Offset",
         new Prop[] {
            new PropRGBA("levels_high", "White Level", new Color(0.5f, 0.5f, 0.5f ,1)),
            new PropRGBA("levels_low", "Black Level ", new Color(0.5f,0.5f,0.5f,1)),
        },
        @"
    // Levels
    v3_1 = (levels_low.rgb - vec3(0.5)) * 2.0;
    COLOR = (COLOR - vec4(v3_1, 0.0)) / (vec4(levels_high.rgb * 2.0, 1.0) - vec4(v3_1, 0.0));
    ", FilterType.COLOR),

    new Filter(

        "Overlay Color",
        new Prop[] {
            new PropRGBA("mcolor", "Color", new Color(1,1,1)),
            new PropFloat("mVal", "Fac", 1.0f),
        },
        @"
        COLOR *= mcolor * mVal;
    ", FilterType.COLOR),

    new Filter(
        "Hue Shift",
        new Prop[] {
            new PropFloat("hueshift", "Fac", 0.0f),
        },
    @"
    // Hue Shift
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
    ", FilterType.COLOR),

    new Filter(
        "Tile",
        new Prop[] {
            new PropFloat("tilesX", "Tiles X", 2, _slider: false, _min: 0, _max: 10000),
            new PropFloat("tilesY", "Tiles Y", 2, _slider: false, _min: 0, _max: 10000),
            new PropFloat("offsetX", "Offset X", 0, _slider: true, _min: -1, _max: 1),
            new PropFloat("offsetY", "Offset Y", 0, _slider: true, _min: -1, _max: 1),
        },
        @"
    // Tile
    uv *= vec2(tilesX, tilesY);
    uv.x += floor(uv.y) * offsetX;
	uv.y += floor(uv.x) * offsetY;
    ", FilterType.DISTORT),

    new Filter(
        "Lens Distort",
        new Prop[] {
            new PropFloat("lensDistort", "Factor", 0, _slider: true, _min: -1, _max: 1),
            new PropFloat("lensZoom", "Zoom", 1, _slider: true, _min: 0.5f, _max: 1.5f),
        },
        @"
    // Lens Distortion
    f_1 = map(lensZoom, 0.5, 1.5, 1.5, 0.5, false);
    uv = (uv - vec2(0.5)) * ( pow(length(uv - vec2(0.5)), lensDistort) * f_1 )  + vec2(0.5);
    ", FilterType.DISTORT),

    new Filter(
        "Flip",
        new Prop[] {
            new PropBool("flipHorizontal", "Horizontal", true),
            new PropBool("flipVertical", "Vertical", false),
        },
    @"
    // Flip
    uv = fract(uv * vec2(float(flipHorizontal) * -2.0 + 1.0, float(flipVertical) * -2.0 + 1.0));
    ", FilterType.DISTORT),

    new Filter(
        "Noise Distort",
        new Prop[] {
            new PropFloat("noiseDistortAmmount", "Distort", 0.2f),
            new PropFloat("noiseDistortOffset", "Offset", 0.15f, _slider: false, _min: -10000, _max: 10000),
            new PropFloat("noiseDistortScale", "Scale", 10, _slider: false, _max: 10000),
            new PropInt("noiseDistortOctaves", "Octaves", 3, 12),
        },
    @"
    // Noise Distort
    uv = mix(uv, vec2(fbm(uv * noiseDistortScale + noiseDistortOffset, noiseDistortOctaves)), noiseDistortAmmount*noiseDistortAmmount);
    ", FilterType.DISTORT),
    
    };
}