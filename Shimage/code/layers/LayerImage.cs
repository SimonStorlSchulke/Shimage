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

    public static LayerImage New(string path) {

        LayerImage l = new LayerImage();
        l.Name = path.GetFile();
        l.path = path;
        l.SetTexture(path);
        l.UpdateLayer();
        return l;
    }

    public void UpdatePath(string path) {
        img.Load(path);
        if (img == null) {
            GD.Print("Image is Null");
            return;
        }
        tex.CreateFromImage(img);
        this.Texture = tex;
    }


    public override void _EnterTree() {
        UpdateLayer();
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
    @"vec3 fg = texture(TEXTURE, uv).rgb;
    layerAlpha = texture(TEXTURE, uv).a;", blendmode, Filters);
        UpdateMaterial();
        (Material as ShaderMaterial).SetShaderParam("blendFactor", blendFactor);
        //Apphandler.instance.ShowCode(shaderCode);
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
    
    
    public Vector2 PixelCoordToGlobalCoord(Vector2 coord) {
        Vector2 c = coord + this.GlobalPosition + this.GetRect().Position;
        c = this.GlobalPosition + (c - this.GlobalPosition).Rotated(this.Rotation);
        c *= Apphandler.currentViewer.RectScale;
        c += Apphandler.currentViewer.RectGlobalPosition;
        return c;
    }


    public Vector2 GlobalToPixelCoord(Vector2 globalCoords) {
        Vector2 coordVp = (globalCoords - Apphandler.currentViewer.RectGlobalPosition) / Apphandler.currentViewer.RectScale;
        // Rotate Mouse Pos around Sprites Center by SPrites Rotation ammount. No idea why negative rotation has to be used
        coordVp = this.GlobalPosition + (coordVp - this.GlobalPosition).Rotated(-this.Rotation);
        coordVp = coordVp - this.GlobalPosition - this.GetRect().Position;
        
        // My brain hurts
        return coordVp;
    }

    public Vector2 UVToGlobalCoord(Vector2 coord) {
        Vector2 c = (coord * GetRect().Size) + this.GlobalPosition + this.GetRect().Position;
        c = this.GlobalPosition + (c - this.GlobalPosition).Rotated(this.Rotation);
        c *= Apphandler.currentViewer.RectScale;
        c += Apphandler.currentViewer.RectGlobalPosition;

        // My brain hurts even more now :)
        return c;
    }


    public Vector2 GlobalToUVCoord(Vector2 globalCoords) {
        return GlobalToPixelCoord(globalCoords) / GetRect().Size;
    }
}
