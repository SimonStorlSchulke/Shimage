using Godot;
using System;

public class Filter : Node {
    public string Name;
    public string Code;
    public string UniformsCode;
    public Prop[] Props;
    public Panel UI;

    //TODO save uniforms as individual variables and generate code from them.
    public Filter(string _name, Prop[] _props, string _code) {
        this.Name = _name;
        this.Code = _code;
        this.Props = _props;

        this.UniformsCode = "";
        foreach (var cProp in Props) {
            this.UniformsCode += cProp.GetUniformCode() + "\n";
        }

        BuildUI();
    }

    public void BuildUI() {
        GD.Print(this.Name + "UI");
        this.UI = new Panel();
        this.UI.RectMinSize = new Vector2(200, 30 + Props.Length * 30);

        Button btnClose = new Button();
        btnClose.Text = "X";
        btnClose.RectPosition = new Vector2(180, 0);
        this.UI.AddChild(btnClose);

        btnClose.Connect("pressed", this, nameof(OnRemove));

        VBoxContainer uiList = new VBoxContainer();
        uiList.Name = "UIList";
        Label labelName = new Label();
        labelName.Name = "Labelname";
        labelName.Text = this.Name;
        uiList.AddChild(labelName);

        foreach (var cProp in this.Props) {
            HBoxContainer HBox = new HBoxContainer();
            Label propLabel = new Label();
            propLabel.Text = cProp.NameUI;

            HBox.AddChild(propLabel);
            HBox.AddChild(cProp.BuildUI());
            uiList.AddChild(HBox);
        }
        this.UI.AddChild(uiList);

    }

    public void OnRemove() {
        FilterStack.instance.Remove(this);
        this.Free();
    }


    //Make GLSL Variable Names Unique. Kinda dirty.
    public Filter NewInstance() {
        string NewCode = Code;
        Prop[] NewProps = new Prop[this.Props.Length];
        int i = 0;
        foreach (var prop in this.Props) {
            NewCode = NewCode.Replace(prop.NameCode, prop.NameCode + "_");
            NewProps[i] = this.Props[i];
            NewProps[i].NameCode += "_";
            i++;
        }
        Code = NewCode;
        return new Filter(this.Name, NewProps, NewCode);
    }
}

/// <summary> Filters may not define new Variables (as this would result of them not being reusable in the same shader)
/// use the pre-defined variables instead (f_1 - f_5, v3_1-v3_5...)
/// Limitation: Prop NameCodes CANNOT contain the whole name of another Variable (TODO: fix - Regex?)
/// </summary>
public class Filters {


    public static Filter Vignette = new Filter(
           "Vignette", new Prop[] {
            new PropFloat("vignettePosX", "X", 0.5f),
            new PropFloat("vignettePosY", "Y", 0.5f),
            new PropFloat("vignettePower", "Power", 0.5f),
            new PropFloat("vignetteWidth", "Width", 0.4f),
            new PropFloat("vignetteBlur", "Blur", 0.3f),
            new PropFloat("spotlight", "Spotlight", 0.0f),
           },
           @"
        f_1 = distance(UV, vec2(vignettePosX, 1.0 - vignettePosY));
        f_1 = 1.0 - f_1;
		f_1 = map(f_1, 1.0 - vignetteWidth * 1.5, 1.0 - vignetteWidth * 1.5 + vignetteBlur, 1.0-vignettePower, 1.0 + spotlight, true);
        f_1 = pow(f_1, 2.0);
        COLOR *= vec4(f_1,f_1,f_1,1);
    ");

    public static Filter Exposure = new Filter(
        "Exposure",
         new Prop[] {
            new PropFloatInf("exposure", "Fac", 1.0f),
        },
        @"
        COLOR *= vec4(exposure,exposure,exposure,1);
    ");

    public static Filter Levels = new Filter(
        "Levels",
         new Prop[] {
            new PropRGBA("levels_low", "low ", new Color(0,0,0,1)),
            new PropRGBA("levels_high", "high", new Color(1,1,1,1)),
        },
        @"
        COLOR = (COLOR - vec4(levels_low.rgb, 0.0)) / (vec4(levels_high.rgb, 1.0) - vec4(levels_low.rgb, 0.0));
    ");

    public static Filter MultiplyColor = new Filter(

        "Overlay Color",
        new Prop[] {
            new PropRGBA("mcolor", "Color", new Color(1,1,1)),
            new PropFloatInf("mVal", "Fac", 1.0f),
        },
        @"
        COLOR *= mcolor * mVal;
    ");

    public static Filter HueShift = new Filter(
        "Hue Shift",
        new Prop[] {
            new PropFloat("hueshift", "Fac", 0.0f),
        },
        @"
        f_1 = cos(hueshift * PI * 2.0);
        f_2 = sin(hueshift * PI * 2.0);
		
    	v3_1.r = (.299f + .701f * f_1 + .168f * f_2) * COLOR.r
            + (.587f - .587f * f_1 + .330f * f_2) * COLOR.g
            + (.114f - .114f * f_1 - .497f * f_2) * COLOR.b;

        v3_1.g = (.299f - .299f * f_1 - .328f * f_2) * COLOR.r
            + (.587f + .413f * f_1 + .035f * f_2) * COLOR.g
            + (.114f - .114f * f_1 + .292f * f_2) * COLOR.b;

        v3_1.b = (.299f - .3f * f_1 + 1.25f * f_2) * COLOR.r
            + (.587f - .588f * f_1 - 1.05f * f_2) * COLOR.g
            + (.114f + .886f * f_1 - .203f * f_2) * COLOR.b;
        COLOR = vec4(v3_1.rgb, 1.0);
    ");
}