using Godot;

public delegate Color Blendmode(Color a, Color b, float opacity);

public static class Blend {

    public static Blendmode NORMAL = delegate (Color a, Color b, float opacity) {
        return opacity * b + (1 - opacity) * a;
    };
}

