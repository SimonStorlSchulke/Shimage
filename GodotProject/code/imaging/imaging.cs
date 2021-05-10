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

public struct floatInf {
    public float val;

    public floatInf(float v) {
        val = v;
    }
}

public abstract class PointFilter {

    //TODO: restrict possible types (float, int, Color...)
    public Dictionary<string, object> properties = new Dictionary<string, object>();

    public string filterName;

    //Runs on every Pixel;
    protected abstract Color Operation(Color col);

    public void Apply(Image img, Image mask = null) {
        this.PropsFromUI();
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
        } else {
            for (int y = 0; y < img.GetHeight(); y++) {
                for (int x = 0; x < img.GetWidth(); x++) {
                    img.SetPixel(x, y, Operation(img.GetPixel(x, y)));
                }
            }
            img.Unlock();
        }
    }

    public FilterNode ui = new FilterNode();

    public void BuildUI() {
        //TODO Extract some of this to FilterNode constructor

        ui.RectMinSize = new Vector2(200, 200);

        VBoxContainer uiList = new VBoxContainer();
        uiList.Name = "UIList";
        Label labelName = new Label();
        labelName.Name = "Labelname";
        labelName.Text = this.filterName;
        uiList.AddChild(labelName);

        foreach (var prop in this.properties) {
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

            if (propType == typeof(floatInf)) {
                SpinBox spinbox = new SpinBox();
                spinbox.MaxValue = 10000;
                spinbox.Step = 0.01;
                spinbox.Value = ((floatInf)prop.Value).val;
                HBox.AddChild(spinbox);
            }

            if (propType == typeof(Color)) {
                ColorPickerButton picker = new ColorPickerButton();
                picker.RectMinSize = new Vector2(32, 0);
                picker.Color = (Color)prop.Value;
                HBox.AddChild(picker);
            }

            uiList.AddChild(HBox);
        }

        ui.AddChild(uiList);
        ui.filter = this;
    }

    public static PointFilter GetFilterOfType(string filtertype) {
        if (filtertype == "HSL") {
            return new FilterHSL(0.5f, 0.5f, 0.5f);
        }
        //TODO ERROR when Filtertype is not known
        return new FilterHSL(0.5f, 0.5f, 0.5f);
    }

    void PropsFromUI() {
        Godot.Collections.Array propsUi = ui.GetNode("UIList").GetChildren();
        propsUi.RemoveAt(0); //Ignore Label

        foreach (HBoxContainer cPropUi in propsUi) {
            string key = cPropUi.GetChild<Label>(0).Text;
            object value = null;

            var valueUi = cPropUi.GetChild(1);
            Type tProp = valueUi.GetType();

            if (tProp == typeof(HSlider)) {
                value = (float)(valueUi as HSlider).Value;
            }

            if (tProp == typeof(ColorPickerButton)) {
                value = (valueUi as ColorPickerButton).Color;
            }

            if (tProp == typeof(SpinBox)) {
                value = new floatInf((float)(valueUi as SpinBox).Value);
            }

            this.properties[key] = value;
        }

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
            this.BuildUI();
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
            this.BuildUI();
        }

        protected override Color Operation(Color col) {
            col *= (float)this.properties["H"];
            return col;
        }
    }

    //NOT WORKING
    public class FilterExposure : PointFilter {

        public FilterExposure(float value) {
            this.filterName = "Exposure";
            this.properties.Add("Exposure", new floatInf(value));
            this.BuildUI();
        }

        protected override Color Operation(Color col) {
            col *= ((floatInf)this.properties["Exposure"]).val;
            return col;
        }
    }

