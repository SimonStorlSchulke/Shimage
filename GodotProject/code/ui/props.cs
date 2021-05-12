using Godot;
using System;
using System.Globalization;

public abstract class Prop {
    public string NameCode;
    public string NameUI;
    public object Value;
    public abstract Control BuildUI();
    public abstract string GetUniformCode();

    public string toNotStupidString(float val) {
        return val.ToString(CultureInfo.InvariantCulture);
    }

    //public abstract object ValueFromUI();
}

public class PropInt : Prop {

    public PropInt(string _nameCode, string _nameUI, int _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control BuildUI() {
        HSlider slider = new HSlider();
        slider.RectMinSize = new Vector2(120, 0);
        slider.MinValue = 0;
        slider.MaxValue = 1;
        slider.Step = 0.01;
        slider.Value = (float)this.Value;
        slider.Connect("value_changed", Shaderer.instance, nameof(Shaderer.instance.OnApplyParam));
        return slider;
    }

    public override string GetUniformCode() {
        return "uniform int " + this.NameCode + " = " + Value + ";";
    }
}

public class PropFloat : Prop {

    public PropFloat(string _nameCode, string _nameUI, float _value)  {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control BuildUI() {
        HSlider slider = new HSlider();
        slider.RectMinSize = new Vector2(120, 0);
        slider.MinValue = 0;
        slider.MaxValue = 1;
        slider.Step = 0.01;
        slider.Value = (float)this.Value;
        slider.Connect(
            "value_changed", 
            Shaderer.instance, 
            nameof(Shaderer.instance.OnApplyParam),
            new Godot.Collections.Array {this.NameCode});
        return slider;
    }

    public override string GetUniformCode() {
        return "uniform float " + this.NameCode + " = " + toNotStupidString((float)this.Value) + ";";
    }
}

public class PropFloatInf : Prop {

    public PropFloatInf(string _nameCode, string _nameUI, float _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control BuildUI() {
        SpinBox spinbox = new SpinBox();
        spinbox.MaxValue = 10000;
        spinbox.Step = 0.01;
        spinbox.Value = (float)this.Value;
        spinbox.Connect(
            "value_changed", 
            Shaderer.instance, 
            nameof(Shaderer.instance.OnApplyParam),
            new Godot.Collections.Array {this.NameCode});
        return spinbox;
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
        picker.RectMinSize = new Vector2(32, 0);
        picker.Color = (Color)this.Value;
        picker.Connect(
            "color_changed",
            Shaderer.instance, 
            nameof(Shaderer.instance.OnApplyParam),
            new Godot.Collections.Array {this.NameCode});
        return picker;
    }

    public override string GetUniformCode() {
        return $"uniform vec4 {this.NameCode} : hint_color = vec4({((Color)Value).r}, {((Color)Value).g}, {((Color)Value).b}, {((Color)Value).a});";
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
        spinboxX.Value = ((Vector2)this.Value).x;
        spinboxY.Value = ((Vector2)this.Value).x;
        VecUI.AddChild(spinboxX);
        VecUI.AddChild(spinboxY);

        /* TODO Vec2Props currently not working because here I pass the individual spin box
        values instead of a vector2 */
        spinboxX.Connect(
            "value_changed", 
            Shaderer.instance, 
            nameof(Shaderer.instance.OnApplyParam),
            new Godot.Collections.Array {this.NameCode});
        spinboxY.Connect(
            "value_changed", 
            Shaderer.instance, 
            nameof(Shaderer.instance.OnApplyParam),
            new Godot.Collections.Array {this.NameCode});
        return VecUI;
    }

    public override string GetUniformCode() {
        return $"uniform vec2 {this.NameCode} = vec2({toNotStupidString(((Vector2)Value).x)}, {toNotStupidString(((Vector2)Value).y)});";
    }
}

