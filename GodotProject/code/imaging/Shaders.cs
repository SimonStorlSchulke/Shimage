using Godot;
using System;


public class Filter {
    public string Name;
    public string Code;
    public string UniformsCode;
    public Prop[] Props;

    //TODO save uniforms as individual variables and generate code from them.
    public Filter(string _name, Prop[] _props, string _code) {
        this.Name = _name;
        this.Code = _code;
        this.Props = _props;

        this.UniformsCode = "";
        foreach (var cProp in Props) {
            this.UniformsCode += cProp.GetUniformCode() + "\n";
        }
    }

    public Panel GetUI() {
        GD.Print(this.Name + "UI");
        Panel ui = new Panel();
        ui.RectMinSize = new Vector2(200, 30 + Props.Length * 30);

        VBoxContainer uiList = new VBoxContainer();
        uiList.Name = "UIList";
        Label labelName = new Label();
        labelName.Name = "Labelname";
        labelName.Text = this.Name;
        uiList.AddChild(labelName);

        foreach (var cProp in this.Props) {
            HBoxContainer HBox = new HBoxContainer();
            Label propLabel = new Label();
            propLabel.Text = cProp.Name;

            HBox.AddChild(propLabel);
            HBox.AddChild(cProp.GetUI());
            uiList.AddChild(HBox);
        }
        ui.AddChild(uiList);

        return ui;
    }
}

public class Filters : Node {
    public static Filter Vignette = new Filter(
           "Vignette", new Prop[] {
            new PropFloatInf("vignettePower", 2.0f)
           },
           @"
        float d = distance(UV, vec2(0.5, 0.5));
        d = 1.0 - pow(d, vignettePower);
        COLOR *= vec4(d,d,d,1);
    ");

    public static Filter Exposure = new Filter(
        "Exposure",
         new Prop[] {
            new PropFloatInf("exposure", 1.0f)
        },
        @"
        COLOR *= vec4(exposure,exposure,exposure,1);
    ");

    public static Filter MultiplyColor = new Filter(
        "Overlay Color",
        new Prop[] {
            new PropRGB("multiplycolor", new Color(1,1,1))
        },
        @"
        COLOR *= vec4(multiplycolor.rgb, 1);
    ");
}