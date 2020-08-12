using Godot;
using System;

public abstract class PointFilter {

    //Runs on every Pixel;
    protected abstract Color Operation(Color col);

    public PointFilter() {

    }

    public void Apply(Image img, Image mask) {
        Color colA = new Color();
        Color colB = new Color();
        img.Lock();

        if (mask != null) {
            mask.Lock();

            for (int y = 0; y < img.GetHeight(); y++) {
                for (int x = 0; x < img.GetWidth(); x++) {
                    colA = img.GetPixel(x, y);
                    colB = Operation(colA);

                    colA = Blend.NORMAL(colA, colB, mask.GetPixel(x, y).r);

                    img.SetPixel(x, y, colA);
                }
            }
            img.Unlock();
            mask.Unlock();
        } else {
            for (int y = 0; y < img.GetHeight(); y++) {
                for (int x = 0; x < img.GetWidth(); x++) {
                    img.SetPixel(x, y, Operation(img.GetPixel(x, y)));
                }
            }
            img.Unlock();
        }
    }
}

public delegate Color Blendmode(Color a, Color b, float opacity);



public static class Blend {

    public static Blendmode NORMAL = delegate (Color a, Color b, float opacity) {
        return opacity * b + (1 - opacity) * a;
    };
}

public class ExampleFilter : PointFilter {
    private readonly Color overlayColor;

    public ExampleFilter(Color overlayColor) {
        this.overlayColor = overlayColor;
    }

    Color col = new Color();
    protected override Color Operation(Color col) {
        col *= overlayColor;
        return col;
    }
}