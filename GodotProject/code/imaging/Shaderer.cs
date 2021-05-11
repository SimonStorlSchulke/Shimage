using Godot;
using System;
public class Shaderer : Sprite {


    public string GenerateShader(Filter[] shaders) {

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
                COLOR = texture(TEXTURE, UV);" +
                codeToAdd +
                "}";

        GD.Print(Code);
        return Code;
    }

    public override void _Ready() {
        ShaderMaterial m = new ShaderMaterial();
        Shader shader = new Shader();
        shader.Code = GenerateShader(new Filter[] { 
            Filters.Vignette, 
            Filters.Exposure,
            Filters.MultiplyColor,
            });
        m.Shader = shader;
        Material = m;
        m.SetShaderParam("vignettePower", 0.4f);
        m.SetShaderParam("exposure", 2f);
        m.SetShaderParam("multiplycolor", new Vector3(1,0.6f,0.7f));
        GD.Print(Material);
    }
}