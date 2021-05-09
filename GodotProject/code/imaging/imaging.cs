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

public abstract class PointFilter {

    //TODO: restrict possible types (float, int, Color...)
    public Dictionary<string, object> properties = new Dictionary<string, object>();

    public string filterName;

    //Runs on every Pixel;
    protected abstract Color Operation(Color col);

    public void Apply(Image img, Image mask = null) {
        Color colA = new Color();
        img.Lock();

        if (mask != null) {
            mask.Lock();

            for (int y = 0; y < img.GetHeight(); y++) {
                for (int x = 0; x < img.GetWidth(); x++) {
                    colA = img.GetPixel(x, y);
                    colA = Blend.NORMAL(colA, Operation(colA), mask.GetPixel(x, y).r);
                    img.SetPixel(x, y, colA);
                }
            }
            mask.Unlock();
        } 
        
        else {
            for (int y = 0; y < img.GetHeight(); y++) {
                for (int x = 0; x < img.GetWidth(); x++) {
                    img.SetPixel(x, y, Operation(img.GetPixel(x, y)));
                }
            }
            img.Unlock();
        }
    }

    public FilterNode BuildUI() {
        //TODO Extract some of this to FilterNode constructor
        FilterNode filterUI = new FilterNode();

        filterUI.RectMinSize = new Vector2(200, 200);

        VBoxContainer uiList = new VBoxContainer();
        uiList.Name = "UIList";
        Label labelName = new Label();
        labelName.Name = "Labelname";
        labelName.Text = this.filterName;
        uiList.AddChild(labelName);

        foreach (var prop in this.properties)
        {
            HBoxContainer HBox = new HBoxContainer();
            Label propLabel = new Label();
            propLabel.Text = prop.Key;

            HBox.AddChild(propLabel);

            var propType = prop.Value.GetType();
            
            if (propType == typeof(float)) {
                HSlider slider = new HSlider();
                slider.RectMinSize = new Vector2(120, 0);
                slider.MinValue = 0;
                slider.MaxValue = 1;
                slider.Step = 0.01;
                slider.Value = (float)prop.Value;
                HBox.AddChild(slider);
            }

            if (propType == typeof(Color)) {
                ColorPickerButton picker = new ColorPickerButton();
                picker.RectMinSize = new Vector2(32, 0);
                picker.Color = (Color)prop.Value;
                HBox.AddChild(picker);
            }

            uiList.AddChild(HBox);
        }

        filterUI.AddChild(uiList);
        filterUI.filter = this;
        return filterUI;
    }

    public static PointFilter GetFilterOfType(string filtertype) {
        if (filtertype == "HSL") {
            return new FilterHSL(0.5f ,0.5f ,0.5f);
        }
        //TODO ERROR when Filtertype is not known
        return new FilterHSL(0.5f ,0.5f ,0.5f);
    }
}

public delegate Color Blendmode(Color a, Color b, float opacity);



public static class Blend {

    public static Blendmode NORMAL = delegate (Color a, Color b, float opacity) {
        return opacity * b + (1 - opacity) * a;
    };
}

public class FilterOverlayColor : PointFilter {

    public FilterOverlayColor(Color overlayColor) {
        this.filterName = "Overlay Color";
        this.properties.Add("overlayColor", overlayColor);
    }

    protected override Color Operation(Color col) {
        col *= (Color)this.properties["overlayColor"];
        return col;
    }
}

public class FilterHSL : PointFilter {

    public FilterHSL(float H, float S, float L) {
        this.filterName = "HSL";
        this.properties.Add("H", H);
        this.properties.Add("S", S);
        this.properties.Add("L", L);
    }

    protected override Color Operation(Color col) {
        col *= (float)this.properties["H"];
        return col;
    }
}
