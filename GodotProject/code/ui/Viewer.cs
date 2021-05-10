using Godot;
using System;

public class Viewer : Sprite {

    [Export]
    float zoomSpeed = 0.1f;

    ImageTexture tex = new ImageTexture();
    Image img = new Image();

    string imgPath = "res://TestImages/lola.png";

    bool mouseHover;

    public static Viewer instance = null;

    public override void _Ready() {
        if (instance == null) {
            instance = this;
        } else {
            GD.PushError("Only one Viewer instance is supported currently");
        }

        img.Load(imgPath);
        tex.CreateFromImage(img);
        Texture = tex;

        this.GetParent<CenterContainer>().Connect("mouse_entered", this, nameof(OnMouseEntered));
        this.GetParent<CenterContainer>().Connect("mouse_exited", this, nameof(OnMouseExited));
    }

    public void OnMouseEntered() {
        mouseHover = true;
    }

    public void OnMouseExited() {
        mouseHover = false;
    }

    public void OnLoadImage(string path) {
        imgPath = path;
        img.Load(path);
        if (img == null)
            GD.Print("NULL!");
        tex.CreateFromImage(img);
        this.Texture = tex;
    }


    public void OnApplyFilters() {
        img.Load(imgPath); //TODO - remove redundant loading
        foreach (PointFilter filter in FilterStack.filterList)
        {
            GD.Print("Apply " + filter.filterName + " to " + img);
            filter.Apply(img);
        }

        tex.CreateFromImage(img);
        Texture = tex;
    }

    public override void _Input(InputEvent e) {
        if (e.IsAction("zoom_in") && this.mouseHover)
            Scale *= (1 + zoomSpeed);
        
        if (e.IsAction("zoom_out") && this.mouseHover)
            Scale *= (1 - zoomSpeed);
    }

}