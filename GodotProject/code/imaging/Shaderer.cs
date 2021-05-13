using Godot;
using System;
using System.Collections.Generic;

public class Shaderer : Sprite {

    public static Shaderer instance = null;
    Image img = new Image();
    string imgPath = "";
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
        imgPath = path;
        img.Load(path);
        if (img == null)
            GD.Print("Image is Null");
        tex.CreateFromImage(img);
        this.Texture = tex;
        GetNode(ShaderToImage).Call("_on_load_image");
    }

    public void OnApplyParam(object value, string name) {
        Shaderer.instance.SetProp(name, value);
    }

    public void SetProp(string _name, object _value) {
        if (_value.GetType() == typeof(Vector2)) {
            GD.Print("Vector");
        }
        ((ShaderMaterial)Material).SetShaderParam(_name, _value);
    }

    public void GenerateShader(List<Filter> shaders) {

        string uniformsToAdd = "";
        string codeToAdd = "";
        foreach (Filter cShader in shaders) {
            uniformsToAdd += cShader.UniformsCode;
            codeToAdd += cShader.Code;
        }
        //TODO shader code nicer maybe
        string Code = "shader_type canvas_item;\n" +
            uniformsToAdd + @"
float map(float value, float min1, float max1, float min2, float max2, bool clamp_result) {
    float res = min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    if (clamp_result) {
        res = clamp(res, min2, max2);
    }
    return res;
}

void fragment(){
    const float PI = 3.14159265358979323846;
    float f_1;
    float f_2;
    float f_3;
    float f_4;
    float f_5;
    vec3 v3_1;
    vec3 v3_2;
    vec3 v3_3;
    COLOR = texture(TEXTURE, UV);" +
            codeToAdd +
            "}";

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

    Vector2 startPos = new Vector2();
    Vector2 startPosMouse = new Vector2();
    bool dragging = false;
    public override void _Input(InputEvent e) {
        if (e.IsAction("zoom_in") && this.mouseHover) {
            Scale *= (1 + ZoomSpeed);
        }

        if (e.IsAction("zoom_out") && this.mouseHover) {
            Scale *= (1 - ZoomSpeed);
        }

        if (e.IsAction("MouseLeft") && this.mouseHover) {
            bool btnDown = ((InputEventMouseButton)e).IsPressed();
            if (btnDown) {
                startPos = Position;
                startPosMouse = GetNode<CenterContainer>(ViewportArea).GetLocalMousePosition();
                GD.Print("Startpos :" + startPos);
            } else {
                //startPos = Position;
                GD.Print("Startpos :" + startPos);
            }
            dragging = btnDown;
            GD.Print(dragging);
        }
    }

    public override void _Process(float delta) {
        if(dragging) {
            Vector2 mPos = GetNode<CenterContainer>(ViewportArea).GetLocalMousePosition();
            Position = startPos + mPos - startPosMouse;
        }
    }
}