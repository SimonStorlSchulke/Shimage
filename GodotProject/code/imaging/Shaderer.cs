using Godot;
using System;

public class Shaderer : Sprite {

    public struct ShaderFilter {
        public string Name;
        public string Code;
        public string UniformsCode;
        public Prop[] props;

        //TODO save uniforms as individual variables and generate code from them.
        public ShaderFilter(string _name, Prop[] _props, string _code) {
            this.Name = _name;
            this.Code = _code;
            this.props = _props;

            this.UniformsCode = "";
            foreach (var cProp in props) {
                this.UniformsCode += cProp.GetUniformCode() + "\n";
            }
        }

        void GenerateUniformsCode() {

        }
    }

    ShaderFilter shaderVignette = new ShaderFilter(
        "Vignette", new Prop[] {
            new PropFloat("vignettePower", 2.0f)
        },
        @"
        //ShaderVignette
        float d = distance(UV, vec2(0.5, 0.5));
        d = 1.0 - pow(d, vignettePower);
        COLOR *= vec4(d,d,d,1);
    ");

    ShaderFilter shaderExposure = new ShaderFilter(
        "Exposure",
         new Prop[] {
            new PropFloat("exposure", 1.0f)
        },
        @"
        COLOR *= vec4(exposure,exposure,exposure,1);
    ");

    ShaderFilter shaderMultiplyColor = new ShaderFilter(
        "Overlay Color",
        new Prop[] {
            new PropRGB("multiplycolor", new Color(1,1,1))
        },
        @"
        COLOR *= vec4(multiplycolor.rgb, 1);
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
        shader.Code = GenerateShader(new ShaderFilter[] { 
            shaderVignette, 
            shaderExposure,
            shaderMultiplyColor,
            });
        m.Shader = shader;
        Material = m;
        m.SetShaderParam("vignettePower", 0.4f);
        m.SetShaderParam("exposure", 2f);
        m.SetShaderParam("multiplycolor", new Vector3(1,0.6f,0.7f));
        GD.Print(Material);
    }
}