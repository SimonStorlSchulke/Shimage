using Godot;

public class ImageIO : Node {
    public static ImageIO instance;

    public override void _Ready() {
        instance = this;
    }

    public static void LoadImage(string path) {

    }
}
