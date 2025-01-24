using Godot;
using System.Collections.Generic;


public partial class LayerRect : Node2D, ILayer {

    ColorRect rect;

    [Export]
    Color color;

    [Export]
    Vector2 size;
    [Export]
    ShaderUtil.BlendMode blendmode = ShaderUtil.BlendMode.Normal;
    [Export]
    float blendFactor = 1.0f;
    public List<Filter> Filters { get; set; }
    string shaderCode = ShaderUtil.shaderCodeBoilerPlate;
    [Export]
    bool editorUpdateShader {
        get => true; set { UpdateLayer(); SetBlendFactor(blendFactor); }
    }

    
    public override void _EnterTree() {
        rect = new ColorRect();
        rect.MouseFilter = Control.MouseFilterEnum.Ignore;
        AddChild(rect); 
        UpdateLayer();
    }

    public void ApplyProp(Variant value, string name) {
        ((ShaderMaterial)rect.Material).SetShaderParameter(name, value);
    }

    public void OnChangeColor(Color cl) {
        color = cl;
        UpdateLayer(); // TODO optimize - use uniform instead
    }

    public void UpdateLayer() {
        shaderCode = ShaderUtil.generateShaderCode(
    $@"vec3 fg = {ShaderUtil.ColorToVec4(color)}.rgb;
    layerAlpha = texture(TEXTURE, uv).a;", blendmode, Filters);
        ShaderMaterial mat = new ShaderMaterial();
        Shader sh = new Shader();
        sh.Code = shaderCode;
        mat.Shader = sh;
        rect.Material = mat;
        (rect.Material as ShaderMaterial).SetShaderParameter("blendFactor", blendFactor);
    }

    public void UpdateMaterial() {
        ShaderMaterial mat = new ShaderMaterial();
        Shader sh = new Shader();
        sh.Code = shaderCode;
        mat.Shader = sh;
        rect.Material = mat;
    }

    public override void _Ready() {
        rect = GetChild<ColorRect>(0);
    }

    public void SetBlendFactor(float fac) {
        blendFactor = fac;
        (rect.Material as ShaderMaterial).SetShaderParameter("blendFactor", blendFactor);
    }

    public float GetBlendFactor() {
        return blendFactor;
    }

    public ShaderUtil.BlendMode GetBlendmode() {
        return blendmode;
    }

    public void SetBlendmode(ShaderUtil.BlendMode mode) {
        blendmode = mode;
    }

    public static LayerRect New(string name, Color color = new Color()) {
        LayerRect l = new LayerRect();
        l.Name = name;
        ColorRect rect = new ColorRect();
        l.AddChild(rect);
        l.rect = rect;
        l.color = color;
        l.UpdateLayer();
        return l;
    }


    public Vector2 GlobalToPixelCoord(Vector2 globalCoords) {
        //TODO not working
        Vector2 coordVp = (globalCoords - Apphandler.currentViewer.GlobalPosition) / Apphandler.currentViewer.Scale;
        Vector2 coord = coordVp - GlobalPosition - rect.Size;
        return coord;
    }

    public Vector2 UVToGlobalCoord(Vector2 pixelCoords) {
        return Vector2.Zero;
    }


    public Vector2 GlobalToUVCoord(Vector2 globalCoords) {
        //TODO not working
        Vector2 coordVp = (globalCoords - Apphandler.currentViewer.GlobalPosition) / Apphandler.currentViewer.Scale;
        Vector2 coord = coordVp - GlobalPosition - rect.Size;
        coord /= rect.Scale;
        return coord;
    }
}
