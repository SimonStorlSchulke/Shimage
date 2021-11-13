using Godot;
using System.Collections.Generic;

public class Shaderer : Sprite {

    public static Shaderer instance = null;
    Image img = new Image();
    ImageTexture tex = new ImageTexture();

    [Export]
    NodePath CodeViewer;

    [Export]
    NodePath ShaderToImage;

    [Export]
    NodePath ViewportArea;

    [Export]
    float ZoomSpeed = .4f;

    public override void _Ready() {
        if (instance == null) {
            instance = this;
        } else {
            GD.PushError("Only one Viewer instance is supported currently");
        }
    }

    public void OnLoadImage(string path) {
        AppHandler.imagePath = path;
        OS.SetWindowTitle("GDPhotoEdit - " + path);
        img.Load(path);
        if (img == null)
            GD.Print("Image is Null");
        tex.CreateFromImage(img);
        this.Texture = tex;
        GetNode(ShaderToImage).Call("_on_load_image");
        OnReCenter();
    }

    public void OnApplyParam(object value, string name) {
        Shaderer.instance.SetProp(name, value);
    }

    public void OnEffectSliderChanged(float value) {
        Shaderer.instance.SetProp("effect_slider_val", value);
    }

    public void SetProp(string _name, object _value) {
        ((ShaderMaterial)Material).SetShaderParam(_name, _value);
    }

    public void GenerateShader(List<Filter> shaders) {

        string uniformsToAdd = "";
        string codeToAddDistortFilters = "";
        string codeToAddColorFilters = "";

        foreach (Filter cShader in shaders) {
            uniformsToAdd += cShader.UniformsCode;

            //Add switch statement here in case more filtertypes are added
            if (cShader.filterType == FilterType.DISTORT) {
                codeToAddDistortFilters += cShader.Code; //must be inverted for some reason..
            } else {
                codeToAddColorFilters += cShader.Code;
            }
        }

        string Code = "shader_type canvas_item;\n" +
            uniformsToAdd + @"

uniform float effect_slider_val = 1.0;

float map(float value, float min1, float max1, float min2, float max2, bool clamp_result) {
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

void fragment(){
    const float PI = 3.14159265358979323846;
    
    //variables to use
    float f_1;float f_2;float f_3;float f_4;float f_5;vec2 v2_1;vec2 v2_2;vec3 v3_1;vec3 v3_2;vec3 v3_3;
    vec2 uv = UV;


    // Distortion Filters------------------------
    " + codeToAddDistortFilters +
    @"
    if (UV.x > effect_slider_val) {
        uv = UV;
    }
    COLOR *= texture(TEXTURE, uv);
    vec4 previous = COLOR;
    
    // Color Filters-----------------------------
    " + codeToAddColorFilters +
    @"
    if (UV.x > effect_slider_val) {
        COLOR = previous;
    }
}";

        GetNode<TextEdit>(CodeViewer).Text = Code;
        ((ShaderMaterial)this.Material).Shader.Code = Code;
    }

    bool mouseHover = false;
    public void OnMouseEntered() {
        mouseHover = true;
    }

    public void OnMouseExited() {
        mouseHover = false;
    }

    public void OnReCenter() {
        Vector2 viewportSize = this.GetParent<CenterContainer>().RectSize;
        Position = viewportSize / 2;
        Vector2 facVec = viewportSize / this.Texture.GetSize();
        float fac = Mathf.Min(facVec.x, facVec.y);
        this.Scale = new Vector2(fac, fac);
    }

    Vector2 startPos;
    Vector2 startPosMouse;
    Vector2 mousePos;
    bool dragging = false;
    public override void _Input(InputEvent e) {
        if (e.IsAction("zoom_in") && this.mouseHover) {
            mousePos = GetNode<CenterContainer>(ViewportArea).GetGlobalMousePosition();
            Vector2 d = mousePos - GlobalPosition;
            Position -= d * ZoomSpeed;
            Scale *= (1 + ZoomSpeed);
        }

        if (e.IsAction("zoom_out") && this.mouseHover) {
            mousePos = GetNode<CenterContainer>(ViewportArea).GetGlobalMousePosition();
            Vector2 d = mousePos - GlobalPosition;
            Position += d * ZoomSpeed;
            Scale *= (1 - ZoomSpeed);
        }

        if (e.IsAction("MouseLeft") && this.mouseHover) {
            bool btnDown = ((InputEventMouseButton)e).IsPressed();
            if (btnDown) {
                startPos = Position;
                startPosMouse = GetNode<CenterContainer>(ViewportArea).GetLocalMousePosition();
            } else {
            }
            dragging = btnDown;
        }
    }

    public override void _Process(float delta) {
        if (dragging) {
            Vector2 mPos = GetNode<CenterContainer>(ViewportArea).GetLocalMousePosition();
            Position = startPos + mPos - startPosMouse;
        }
    }
}