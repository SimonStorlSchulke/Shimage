using Godot;
using System;
using System.Globalization;

public abstract class Prop : Node {
    public string NameCode;
    public string NameUI;
    public object Value;
    public Control UI;
    public abstract Control BuildUI();
    public abstract void UpdateUIProp();
    public abstract string GetUniformCode();

    public void Apply(object value) {
        if (Apphandler.currentViewer.activeLayer == null)
            return;
        Apphandler.currentViewer.activeLayer.ApplyProp(value, NameCode);
        Value = value;
    }

    /// <summary> The fact that this is necessary is stupid.</summary>
    public static string toNotStupidString(float val) {
        return val.ToString(CultureInfo.InvariantCulture);
    }
}


public class PropFloat : Prop {

    bool slider;
    float Min;
    float Max;

    public PropFloat(string _nameCode, string _nameUI, float _value, bool _slider = true, float _min = 0, float _max = 1) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
        this.slider = _slider;
        this.Min = _min;
        this.Max = _max;
    }

    public override void UpdateUIProp() {
        (UI as Range).Value = (float)Value;
    }

    public override Control BuildUI() {
        Range inputField;

        if (slider) {
            inputField = new HSlider();
            inputField.RectMinSize = new Vector2(120, 0);
        } else {
            inputField = new SpinBox();
        }
        inputField.MinValue = this.Min;
        inputField.MaxValue = this.Max;
        inputField.Step = 0.01;
        inputField.Value = (float)this.Value;
        inputField.Connect(
            "value_changed",
            this,
            nameof(Apply));
        UI = inputField;
        return inputField;
    }

    public override string GetUniformCode() {
        return "uniform float " + this.NameCode + " = " + toNotStupidString((float)this.Value) + ";";
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

    public override void UpdateUIProp() {
        (UI as SpinBox).Value = (int)Value;
    }

    public override Control BuildUI() {
        SpinBox spinBox = new SpinBox();
        //spinBox.RectMinSize = new Vector2(120, 0);
        spinBox.MinValue = 0;
        spinBox.MaxValue = this.max;
        spinBox.Value = (int)this.Value;
        spinBox.Connect(
            "value_changed",
            this,
            nameof(Apply));
        UI = spinBox;
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

    public override void UpdateUIProp() {
        (UI as CheckBox).Pressed = (bool)Value;
    }


    public override Control BuildUI() {
        CheckBox checkBox = new CheckBox();
        checkBox.Pressed = (bool)this.Value;
        checkBox.Connect(
            "toggled",
            this,
            nameof(Apply));
        UI = checkBox;
        return checkBox;
    }

    public override string GetUniformCode() {
        string v = (bool)Value ? "true" : "false";
        return "uniform bool " + this.NameCode + " = " + v + ";";
    }
}


public class PropRGBA : Prop {

    public PropRGBA(string _nameCode, string _nameUI, Color _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override void UpdateUIProp() {
        (UI as ColorPickerButton).Color = (Color)Value;
    }


    public override Control BuildUI() {
        ColorPickerButton picker = new ColorPickerButton();
        picker.RectMinSize = new Vector2(32, 0);
        picker.Color = (Color)this.Value;

        picker.Connect(
            "color_changed",
            this,
            nameof(Apply));
        UI = picker;
        return picker;
    }

    public override string GetUniformCode() {

        string r = toNotStupidString(((Color)Value).r);
        string g = toNotStupidString(((Color)Value).g);
        string b = toNotStupidString(((Color)Value).b);
        string a = toNotStupidString(((Color)Value).a);

        return $"uniform vec4 {this.NameCode} : hint_color = vec4({r}, {g}, {b}, {a});";
    }
}


public class PropVector2 : Prop {

    public PropVector2(string _nameCode, string _nameUI, Vector2 _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override void UpdateUIProp() {
        (UI as HBoxContainer).GetNode<SpinBox>("spX").Value = ((Vector2)Value).x;
        (UI as HBoxContainer).GetNode<SpinBox>("spY").Value = ((Vector2)Value).y;
    }

    public override Control BuildUI() {
        HBoxContainer VecUI = new HBoxContainer();
        SpinBox spinboxX = new SpinBox();
        spinboxX.Name = "spX";
        SpinBox spinboxY = new SpinBox();
        spinboxY.Name = "spY";
        spinboxX.MaxValue = 10000;
        spinboxY.MaxValue = 10000;
        spinboxX.Step = 0.01;
        spinboxY.Step = 0.01;
        spinboxX.Value = ((Vector2)this.Value).x;
        spinboxY.Value = ((Vector2)this.Value).x;
        VecUI.AddChild(spinboxX);
        VecUI.AddChild(spinboxY);
        spinboxX.Connect(
            "value_changed",
            this,
            nameof(Apply));
        spinboxY.Connect(
            "value_changed",
            this,
            nameof(Apply));
        UI = VecUI;
        return VecUI;
    }

    public override string GetUniformCode() {
        return $"uniform vec2 {this.NameCode} = vec2({toNotStupidString(((Vector2)Value).x)}, {toNotStupidString(((Vector2)Value).y)});";
    }
}


public class PropPosition : Prop {

    public PropPosition(string _nameCode, string _nameUI, Vector2 _value) {
        this.NameCode = _nameCode;
        this.NameUI = _nameUI;
        this.Value = _value;
    }

    public override void UpdateUIProp() {
        //(UI as HBoxContainer).GetNode<SpinBox>("spX").Value = ((Vector2)Value).x;
        //(UI as HBoxContainer).GetNode<SpinBox>("spY").Value = ((Vector2)Value).y;
    }

    void ShowMover() {
        ToolsLayer.instance.GetNode<Mover>("Mover").ActivateTool(this);
    }

    public void OnMoverMoved(Vector2 to) {
        Vector2 pos = Apphandler.currentViewer.activeLayer.GlobalCoordsToUVCoord(to);
        StatusBarInfo.Display($"Position: x={pos.x} y={pos.y}");
        Apply(pos);
    }

    public override Control BuildUI() {
        HBoxContainer VecUI = new HBoxContainer();
        Button btnSetPosition = new Button();
        btnSetPosition.Text = "Set Position";
        VecUI.AddChild(btnSetPosition);
        btnSetPosition.Connect(
            "pressed",
            this,
            nameof(ShowMover));
        UI = VecUI;
        return VecUI;
    }

    public override string GetUniformCode() {
        return $"uniform vec2 {this.NameCode} = vec2({toNotStupidString(((Vector2)Value).x)}, {toNotStupidString(((Vector2)Value).y)});";
    }
}