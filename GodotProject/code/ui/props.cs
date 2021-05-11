using Godot;
using System;

public abstract class Prop {
    public string NameCode;
    public string NameUI;
    public object Value;
    public abstract Control GetUI();
    public abstract string GetUniformCode();
}

public class PropInt : Prop {

    public PropInt(string _nameCode, string _nameUI, int _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control GetUI() {
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

    public override Control GetUI() {
        HSlider slider = new HSlider();
        slider.RectMinSize = new Vector2(120, 0);
        slider.MinValue = 0;
        slider.MaxValue = 1;
        slider.Step = 0.01;
        slider.Value = (float)this.Value;
        return slider;
    }

    public override string GetUniformCode() {
        return "uniform float " + this.NameCode + " = " + Value + ";";
    }
}

public class PropFloatInf : Prop {

    public PropFloatInf(string _nameCode, string _nameUI, float _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control GetUI() {
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
        return "uniform float " + this.NameCode + " = " + Value + ";";
    }
}

public class PropRGBA : Prop {

    public PropRGBA(string _nameCode, string _nameUI, Color _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override Control GetUI() {
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

