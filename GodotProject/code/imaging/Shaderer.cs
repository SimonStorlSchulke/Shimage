using Godot;
using System;
using System.Collections.Generic;

public class Shaderer : Sprite {

    public static Shaderer instance = null;

    [Export]
    float ZoomSpeed = .4f;

    public override void _Ready() {
        if (instance == null) {
            instance = this;
        } else {
            GD.PushError("Only one Viewer instance is supported currently");
        }
    }

    public void OnApplyFilters() {
        foreach (Filter filter in FilterStack.filterList) {
            filter.PropsFromUI();
        }
    }

    public void OnApplyParam(object value, string name) {
        Shaderer.instance.SetProp(name, value);
    }

    public void SetProp(string _name, object _value) {
        GD.Print(_name + "Color: " + _value);
        ((ShaderMaterial)Material).SetShaderParam(_name, _value);
    }

    public string GenerateShader(List<Filter> shaders) {

        string uniformsToAdd = "";
        string codeToAdd = "";
        foreach (Filter cShader in shaders) {
            uniformsToAdd += cShader.UniformsCode;
            codeToAdd += cShader.Code;
        }
        //TODO shader code nicer maybe
        string Code = "shader_type canvas_item;" +
            uniformsToAdd + @"
    void fragment(){
        float f_1 = .0;
        float f_2 = .0;
        float f_3 = .0;
        float f_4 = .0;
        float f_5 = .0;
        COLOR = texture(TEXTURE, UV);" +
                codeToAdd +
                "}";

        GD.Print(Code);
        return Code;
    }

    bool mouseHover = true;
    public void OnMouseEntered() {
        mouseHover = true;
    }

    public void OnMouseExited() {
        mouseHover = false;
    }

    public override void _Input(InputEvent e) {

        if (e.IsAction("zoom_in") && this.mouseHover) {
            Scale *= (1 + ZoomSpeed);
        }

        if (e.IsAction("zoom_out") && this.mouseHover) {
            Scale *= (1 - ZoomSpeed);
        }
    }
}