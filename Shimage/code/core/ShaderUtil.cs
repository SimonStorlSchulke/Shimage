using Godot;
using System.Collections.Generic;

public static class ShaderUtil {

    public static string shaderCodeBoilerPlate =
@"uniform float blendFactor = 1.0;
const float PI = 3.14159265359;

//variables to use

float map_range(float value, float min1, float max1, float min2, float max2, bool clamp_result) {
    float res = min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    if (clamp_result) {
        res = clamp(res, min2, max2);
    }
    return res;
}

// Precision-adjusted variations of https://www.shadertoy.com/view/4djSRW
float hash(float p) { p = fract(p * 0.011); p *= p + 7.5; p *= p + p; return fract(p); }
float hashV2(vec2 p) {vec3 p3 = fract(vec3(p.xyx) * 0.13); p3 += dot(p3, p3.yzx + 3.333); return fract((p3.x + p3.y) * p3.z); }

float noise(vec2 x) {
    vec2 i = floor(x);
    vec2 f = fract(x);
	// Four corners in 2D of a tile
	float a = hashV2(i);
    float b = hashV2(i + vec2(1.0, 0.0));
    float c = hashV2(i + vec2(0.0, 1.0));
    float d = hashV2(i + vec2(1.0, 1.0));
    vec2 u = f * f * (3.0 - 2.0 * f);
	return mix(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

float fbm(vec2 x, int noiseOctaves) {
	float v = 0.0;
	float a = 0.5;
	vec2 shift = vec2(100);
	// Rotate to reduce axial bias
    mat2 rot = mat2(vec2(cos(0.5), sin(0.5)), vec2(-sin(0.5), cos(0.50)));
	for (int i = 0; i < noiseOctaves; ++i) {
		v += a * noise(x);
		x = rot * x * 2.0 + shift;
		a *= 0.5;
	}
	return v;
}

// Hue Shift
vec3 shift_hue(vec3 col, float hueshift) {
    float f_1 = cos(hueshift * PI * 2.0);
    float f_2 = sin(hueshift * PI * 2.0);
    vec3 c_new = vec3(0.0,0.0,0.0);
    c_new.r = (.299f + .701f * f_1 + .168f * f_2) * col.r
        + (.587f - .587f * f_1 + .330f * f_2) * col.g
        + (.114f - .114f * f_1 - .497f * f_2) * col.b;
    c_new.g = (.299f - .299f * f_1 - .328f * f_2) * col.r
        + (.587f + .413f * f_1 + .035f * f_2) * col.g
        + (.114f - .114f * f_1 + .292f * f_2) * col.b;
    c_new.b = (.299f - .3f * f_1 + 1.25f * f_2) * col.r
        + (.587f - .588f * f_1 - 1.05f * f_2) * col.g
        + (.114f + .886f * f_1 - .203f * f_2) * col.b;
    return c_new;
}


float luminance(vec3 col) {
    return dot(col, vec3(0.30,0.59,0.11));
}

void fragment(){
    float f_1;float f_2;float f_3;float f_4;float f_5;vec2 v2_1;vec2 v2_2;vec3 v3_1;vec3 v3_2;vec3 v3_3;
    vec2 uv = UV;
    vec3 bg = texture(SCREEN_TEXTURE, SCREEN_UV).rgb;
    float layerAlpha = 1.0;
    ";

    public static string generateShaderCode(string fragment, BlendMode blendMode, List<Filter> filters) {
        string code = "shader_type canvas_item;\n";

        string uniformsToAdd = "";
        string codeToAddDistortFilters = "";
        string codeToAddColorFilters = "";

        if (filters != null) {

            foreach (Filter cShader in filters) {
                uniformsToAdd += cShader.GetUniformsCode();

                //Add switch statement here in case more filtertypes are added
                if (cShader.type == FilterType.DISTORT) {
                    codeToAddDistortFilters += cShader.Code;
                } else {
                    codeToAddColorFilters += cShader.Code;
                }
            }
        }

        code += uniformsToAdd;
        code += shaderCodeBoilerPlate;
        code += fragment;
        code += codeToAddColorFilters;

        code += "\n    " + ShaderUtil.BlenModeCode[blendMode] + @"
    COLOR.a = layerAlpha * blendFactor;
}";
        return code;
    }

    public enum BlendMode { Normal, Multiply, Add, Subtract, Divide, Overlay, Screen, Difference, MultiplyAdd }

    public static Dictionary<BlendMode, string> BlenModeCode = new Dictionary<BlendMode, string>(){
        {BlendMode.Normal, "COLOR.rgb = fg;"},
        {BlendMode.Multiply, "COLOR.rgb = bg * fg;"},
        {BlendMode.Add, "COLOR.rgb = bg + fg;"},
        {BlendMode.Subtract, "COLOR.rgb = bg - fg;"},
        {BlendMode.Divide, "COLOR.rgb = bg / fg;"},
        {BlendMode.Overlay,
    @"if (luminance(bg) < 0.5) {
        COLOR.rgb = 2.0 * bg * fg;
    } else {
        COLOR.rgb = 1.0 - 2.0 * (1.0 - bg) * (1.0 - fg);
    }"},
        {BlendMode.Screen, "COLOR.rgb = 1.0 - (1.0 - bg) * (1.0 - fg);"},
        {BlendMode.MultiplyAdd, "COLOR.rgb = bg * (fg + 1.0);"},
    };

    /// <summary> Godot Color to vec4 string </summary>
    public static string ColorToVec4(Color col) {
        string r = Prop.toNotStupidString(((Color)col).r);
        string g = Prop.toNotStupidString(((Color)col).g);
        string b = Prop.toNotStupidString(((Color)col).b);
        string a = Prop.toNotStupidString(((Color)col).a);

        return $"vec4({r}, {g}, {b}, {a})";
    }
}
