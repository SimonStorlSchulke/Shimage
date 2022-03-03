using Godot;
using System.Collections.Generic;


public class LayerImage : Sprite, ILayer {

    [Export]
    string path;
    [Export]
    ShaderUtil.BlendMode blendmode = ShaderUtil.BlendMode.Normal;
    [Export]
    float blendFactor = 1.0f;
    public List<Filter> Filters {get; set;}
    string shaderCode = ShaderUtil.shaderCodeBoilerPlate;
    [Export]
    bool editorUpdateShader {
        get => true; set { UpdateLayer(); SetBlendFactor(blendFactor); }
    }

    
    Image img = new Image();
    ImageTexture tex = new ImageTexture();

    public static LayerImage New(string name, string path) {

        LayerImage l = new LayerImage();
        l.Name = name;
        l.path = path;
        l.SetTexture(path);
        l.UpdateLayer();
        return l;
    }

    public void UpdateMaterial() {
        ShaderMaterial mat = new ShaderMaterial();
        Shader sh = new Shader();
        sh.Code = shaderCode;
        mat.Shader = sh;
        Material = mat;
    }

    public void ApplyProp(object value, string name) {
        (Material as ShaderMaterial).SetShaderParam(name, value);
    }

    public void UpdateLayer() {
        shaderCode = ShaderUtil.generateShaderCode(
    @"vec3 fg = texture(TEXTURE, UV).rgb;
    layerAlpha = texture(TEXTURE, UV).a;", blendmode, Filters);
        UpdateMaterial();
        (Material as ShaderMaterial).SetShaderParam("blendFactor", blendFactor);
        Apphandler.instance.ShowCode(shaderCode);
    }
    public override void _Ready() {
        FilterExposure exp = new FilterExposure();
        FilterExposure exp2 = new FilterExposure();
        this.AddFilter(exp);
        this.AddFilter(exp2);
    }

    public void SetTexture(string path) {
        img.Load(path);
        if (img == null)
            GD.Print("Image is Null");
        tex.CreateFromImage(img);
        Texture = tex;
    }

    public float GetBlendFactor() {
        return blendFactor;
    }

    public virtual void SetBlendFactor(float fac) {
        blendFactor = fac;
        (Material as ShaderMaterial).SetShaderParam("blendFactor", blendFactor);
    }

    public ShaderUtil.BlendMode GetBlendmode() {
        return blendmode;
    }

    public void SetBlendmode(ShaderUtil.BlendMode mode) {
        blendmode = mode;
    }
}
