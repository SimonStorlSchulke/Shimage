using Godot;
using System;

public class Shaderer : Sprite {

    public struct ShaderFilter {
        public string Code;
        public string UniformsCode;

        //TODO save uniforms as individual variables and generate code from them.
        public ShaderFilter(string _uniforms, string _code) {
            Code = _code;
            UniformsCode = _uniforms;
        }
    }

    ShaderFilter shaderVignette = new ShaderFilter(
        "uniform float vignettePower = 2.0;", @"
        //ShaderVignette
        float d = distance(UV, vec2(0.5, 0.5));
        d = 1.0 - pow(d, vignettePower);
        COLOR *= vec4(d,d,d,1);
    ");

    public string GenerateShader(ShaderFilter[] shaders) {

        string uniformsToAdd = "";
        string codeToAdd = "";
        foreach (ShaderFilter cShader in shaders) {
            uniformsToAdd += cShader.UniformsCode;
            codeToAdd += cShader.Code;
        }
        //TODO shader code nicer maybe
        string Code = "shader_type canvas_item;" +
            uniformsToAdd + @"
            void fragment(){
                COLOR = texture(TEXTURE, UV);" +
                codeToAdd +
                "}";

        GD.Print(Code);
        return Code;
    }

    public override void _Ready() {
        ShaderMaterial m = new ShaderMaterial();
        Shader shader = new Shader();
        shader.Code = GenerateShader(new ShaderFilter[] { shaderVignette });
        m.Shader = shader;
        Material = m;
        m.SetShaderParam("vignettePower", 0.4f);
        GD.Print(Material);
    }
}