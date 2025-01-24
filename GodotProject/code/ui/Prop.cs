using Godot;
using System;
using System.Globalization;
using Range = Godot.Range;

public abstract class Prop {
    public string NameCode;
    public string NameUI;
    public object Value;
    public abstract Control BuildUI();
    public abstract string GetUniformCode();

    /// <summary> The fact that this is necessary is stupid.</summary>
    public string toNotStupidString(float val) {
        return val.ToString(CultureInfo.InvariantCulture);
    }
}

public class PropInt : Prop {

    public int max;

    public PropInt(string _nameCode, string _nameUI, int _value, int _max) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
        this.max = _max;
    }

    public override Control BuildUI() {
        SpinBox spinBox = new SpinBox();
        //spinBox.RectMinSize = new Vector2(120, 0);
        spinBox.MinValue = 0;
        spinBox.MaxValue = this.max;
        spinBox.Value = (int)this.Value;
        
        spinBox.ValueChanged += (v) => Shaderer.instance.OnApplyParam(v, this.NameCode);
        
        return spinBox;
    }

    public override string GetUniformCode() {
        return "uniform int " + this.NameCode + " = " + Value + ";";
    }
}

public class PropBool : Prop {

    public PropBool(string _nameCode, string _nameUI, bool _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control BuildUI() {
        CheckBox checkBox = new CheckBox();
        //spinBox.RectMinSize = new Vector2(120, 0);
        checkBox.SetPressed((bool)this.Value);
        
        checkBox.Toggled += (v) => Shaderer.instance.OnApplyParam(v, this.NameCode);
        
        return checkBox;
    }

    public override string GetUniformCode() {
        string v = (bool)Value ? "true" : "false";
        return "uniform bool " + this.NameCode + " = " + v + ";";
    }
}

public class PropFloat : Prop {

    bool slider;
    float Min;
    float Max;

    public PropFloat(string _nameCode, string _nameUI, float _value, bool _slider = true, float _min = 0, float _max = 1)  {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
        this.slider = _slider;
        this.Min = _min;
        this.Max = _max;
    }

    public override Control BuildUI() {
        Range inputField;
        
        if (slider) {
            inputField = new HSlider();
            inputField.SetCustomMinimumSize(new Vector2(120, 0)); //from RectMinSize = ... correct?
        } else {
            inputField = new SpinBox();
        }
            inputField.MinValue = this.Min;
            inputField.MaxValue = this.Max;
            inputField.Step = 0.01;
            inputField.Value = (float)this.Value;
            
            inputField.ValueChanged += (v) => Shaderer.instance.OnApplyParam(v, this.NameCode);
            
        return inputField;
    }

    public override string GetUniformCode() {
        return "uniform float " + this.NameCode + " = " + toNotStupidString((float)this.Value) + ";";
    }
}

public class PropRGBA : Prop {

    public PropRGBA(string _nameCode, string _nameUI, Color _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control BuildUI() {
        ColorPickerButton picker = new ColorPickerButton();
        picker.SetCustomMinimumSize(new Vector2(32, 0)); //from RectMinSize = ... correct?
        picker.Color = (Color)this.Value;
        
        picker.ColorChanged += (v) => Shaderer.instance.OnApplyParam(v, this.NameCode);

        return picker;
    }

    public override string GetUniformCode() {

        string r = toNotStupidString(((Color)Value).R);
        string g = toNotStupidString(((Color)Value).G);
        string b = toNotStupidString(((Color)Value).B);
        string a = toNotStupidString(((Color)Value).A);

        return $"uniform vec4 {this.NameCode} : source_color = vec4({r}, {g}, {b}, {a});";
    }
}

public class PropVector2 : Prop {

    public PropVector2(string _nameCode, string _nameUI, Vector2 _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control BuildUI() {
        HBoxContainer VecUI = new HBoxContainer();
        SpinBox spinboxX = new SpinBox();
        SpinBox spinboxY = new SpinBox();
        spinboxX.MaxValue = 10000;
        spinboxY.MaxValue = 10000;
        spinboxX.Step = 0.01;
        spinboxY.Step = 0.01;
        spinboxX.Value = ((Vector2)this.Value).X;
        spinboxY.Value = ((Vector2)this.Value).X;
        VecUI.AddChild(spinboxX);
        VecUI.AddChild(spinboxY);

        /* TODO Vec2Props currently not working because here I pass the individual spin box
        values instead of a vector2 */
        
        spinboxX.ValueChanged += (v) => Shaderer.instance.OnApplyParam(v, this.NameCode);
        spinboxY.ValueChanged += (v) => Shaderer.instance.OnApplyParam(v, this.NameCode);

        return VecUI;
    }

    public override string GetUniformCode() {
        return $"uniform vec2 {this.NameCode} = vec2({toNotStupidString(((Vector2)Value).X)}, {toNotStupidString(((Vector2)Value).Y)});";
    }
}