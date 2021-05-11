using Godot;
using System;

public abstract class Prop {
    public string Name;
    public object Value;
    public abstract void BuildUI();
    public abstract string GetUniformCode();
}

public class PropInt : Prop {

    public PropInt(string _name, int _value) {
        this.Name = _name;
        this.Value = _value;
    }

    public override void BuildUI() {
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

    public override void BuildUI() {
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

    public override void BuildUI() {
    }

    public override string GetUniformCode() {
        return $"uniform vec3 {this.Name} = vec3({((Color)Value).r}, {((Color)Value).g}, {((Color)Value).b});";    
        }
}

