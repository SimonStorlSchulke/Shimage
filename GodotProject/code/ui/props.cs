using Godot;
using System;

public abstract class Prop {
    public string Name;
    public object Value;
    public abstract Control GetUI();
    public abstract string GetUniformCode();
}

public class PropInt : Prop {

    public PropInt(string _name, int _value) {
        this.Name = _name;
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
        return "uniform int " + this.Name + " = " + Value + ";";
    }
}

public class PropFloat : Prop {

    public PropFloat(string _name, float _value) {
        this.Name = _name;
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
        return "uniform float " + this.Name + " = " + Value + ";";
    }
}

public class PropFloatInf : Prop {

    public PropFloatInf(string _name, float _value) {
        this.Name = _name;
        this.Value = _value;
    }

    public override Control GetUI() {
        SpinBox spinbox = new SpinBox();
        spinbox.MaxValue = 10000;
        spinbox.Step = 0.01;
        spinbox.Value = (float)this.Value;
        return spinbox;
    }

    public override string GetUniformCode() {
        return "uniform float " + this.Name + " = " + Value + ";";
    }
}

public class PropRGB : Prop {

    public PropRGB(string _name, Color _value) {
        this.Name = _name;
        this.Value = _value;
    }

    public override Control GetUI() {
        ColorPickerButton picker = new ColorPickerButton();
        picker.RectMinSize = new Vector2(32, 0);
        picker.Color = (Color)this.Value;
        return picker;
    }

    public override string GetUniformCode() {
        return $"uniform vec3 {this.Name} = vec3({((Color)Value).r}, {((Color)Value).g}, {((Color)Value).b});";
    }
}

