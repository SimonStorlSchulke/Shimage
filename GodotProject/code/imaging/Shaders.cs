using Godot;
using System;

public class Filter {
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
            HBox.AddChild(cProp.GetUI());
            uiList.AddChild(HBox);
        }
        this.UI.AddChild(uiList);
    }

    public void PropsFromUI() {
        Godot.Collections.Array propsUi = this.UI.GetNode("UIList").GetChildren();
        propsUi.RemoveAt(0); //Ignore Label
        
        int i = 0;
        foreach (Prop cProp in this.Props) {
            Control valueUI = ((Control)propsUi[i]).GetChild<Control>(1);
            
            if (cProp.GetType() == typeof(PropFloatInf)) {
               Shaderer.instance.SetProp(cProp.NameCode, ((SpinBox)valueUI).Value);
            }
            
            if (cProp.GetType() == typeof(PropFloat)) {
               Shaderer.instance.SetProp(cProp.NameCode, ((HSlider)valueUI).Value);
            }
            
            if (cProp.GetType() == typeof(PropRGBA)) {
                GD.Print(((ColorPickerButton)valueUI).Color);
               Shaderer.instance.SetProp(cProp.NameCode, ((ColorPickerButton)valueUI).Color);
            }
        }
    }

    //Make GLSL Variable Names Unique. Kinda dirty.
    public Filter NewInstance() {
        string NewCode = Code;
        Prop[] NewProps = new Prop[this.Props.Length];
        int i = 0;
        foreach (var prop in this.Props) {
            NewCode = NewCode.Replace(prop.NameCode, prop.NameCode+"_");
            NewProps[i] = this.Props[i];
            NewProps[i].NameCode += "_";
            i++;
        }
        Code = NewCode;
        return new Filter(this.Name, NewProps, NewCode);
    }
}

/* Filters may not define new Variables (as this would result of them not being reusable in the same shader)
use the pre-defined variables instead (f_1 - f_5, v3_1-v3_5...)
Prop NameCodes CANNOT have similar lettercombinations in them (TODO: fix - Regex?)
*/
public class Filters {
    public static Filter Vignette = new Filter(
           "Vignette", new Prop[] {
            new PropFloatInf("vignettePower", "Power", 2.0f)
           },
           @"
        f_1 = distance(UV, vec2(0.5, 0.5));
        f_1 = 1.0 - pow(f_1, vignettePower);
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