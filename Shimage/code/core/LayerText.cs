using Godot;
using System.Collections.Generic;


public class LayerText : Node2D, ILayer {

    Label lbl;

    [Export]
    Color color;
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

    public static LayerText New(string name, string text, Color color = new Color()) {
        LayerText l = new LayerText();
        l.Name = name;
        Label lbl = new Label();
        lbl.Text = text;
        l.AddChild(lbl);
        l.lbl = lbl;
        l.UpdateLayer();
        return l;
    }

    public void SetText(string text) {
        lbl.Text = text;
    }

    public void SetFont(Font font) {
        lbl.AddFontOverride("font", font);
    }

    public void UpdateMaterial() {
        ShaderMaterial mat = new ShaderMaterial();
        Shader sh = new Shader();
        sh.Code = shaderCode;
        mat.Shader = sh;
        lbl.Material = mat;
    }

    public void ApplyProp(object value, string name) {
        ((ShaderMaterial)lbl.Material).SetShaderParam(name, value);
    }

    public void UpdateLayer() {
        shaderCode = ShaderUtil.generateShaderCode(
    $@"vec3 fg = {ShaderUtil.ColorToVec4(color)}.rgb;
    layerAlpha = texture(TEXTURE, UV).a;", blendmode, Filters);
        UpdateMaterial();
        (lbl.Material as ShaderMaterial).SetShaderParam("blendFactor", blendFactor);
    }
    public override void _Ready() {
        lbl = GetChild<Label>(0);
    }

    public float GetBlendFactor() {
        return blendFactor;
    }

    public void SetBlendFactor(float fac) {
        blendFactor = fac;
        (lbl.Material as ShaderMaterial).SetShaderParam("blendFactor", blendFactor);
    }

    public ShaderUtil.BlendMode GetBlendmode() {
        return blendmode;
    }

    public void SetBlendmode(ShaderUtil.BlendMode mode) {
        blendmode = mode;
    }


    public Vector2 GlobalCoordToPixelCoord(Vector2 globalCoords) {
        // TODO not working
        Vector2 coordVp = (globalCoords - Apphandler.currentViewer.RectGlobalPosition) / Apphandler.currentViewer.RectScale;
        Vector2 coord = coordVp - GlobalPosition - lbl.RectScale;
        return coord;
    }


    public Vector2 GlobalCoordToUVCoord(Vector2 globalCoords) {
        // TODO not working
        Vector2 coordVp = (globalCoords - Apphandler.currentViewer.RectGlobalPosition) / Apphandler.currentViewer.RectScale;
        Vector2 coord = coordVp - GlobalPosition - lbl.RectScale;
        //coord /= this.GetRect().Size;
        return coord;
    }


    public Vector2 UVCoordToGlobalCoord(Vector2 pixelCoords) {
        return Vector2.Zero;
    }
}
