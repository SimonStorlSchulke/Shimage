using Godot;
using System.Collections.Generic;

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
        this.UI.RectMinSize = new Vector2(200, 32 + Props.Length * 25);
        this.UI.MarginBottom = 10;

        Button btnClose = new Button();
        btnClose.Text = "X";
        btnClose.RectPosition = new Vector2(180, 0);

        btnClose.Connect("pressed", this, nameof(OnRemove));

        GridContainer UIGrid = new GridContainer();
        UIGrid.Columns = 2;

        Label labelName = new Label();
        labelName.Name = "Labelname";
        labelName.Text = this.Name;
        this.UI.AddChild(labelName);
        this.UI.AddChild(btnClose);

        foreach (var cProp in this.Props) {
            HBoxContainer HBox = new HBoxContainer();
            Label propLabel = new Label();
            propLabel.Text = cProp.NameUI;

            UIGrid.AddChild(propLabel);
            UIGrid.AddChild(cProp.BuildUI());
        }
        UIGrid.RectPosition = new Vector2(0, 32);
        this.UI.AddChild(UIGrid);

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