using Godot;
using System;
using System.Collections.Generic;

/*
public enum PropertyType {
    FLOAT,
    FLOAT01,
    COLOR,
    INT
}
*/

public delegate Color Blendmode(Color a, Color b, float opacity);

public static class Blend {

    public static Blendmode NORMAL = delegate (Color a, Color b, float opacity) {
        return opacity * b + (1 - opacity) * a;
    };
}

