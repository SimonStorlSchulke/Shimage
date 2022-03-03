using Godot;
using System;

public enum FilterType {
    COLOR,
    DISTORT
}

public abstract class Filter : Node
{
    public static new string Name;
    public FilterType type;
    public Prop[] Props = {};
    public string Code;

    public string GetUniformsCode() {
        string uniformsCode = "";
        foreach (var cProp in Props) {
            uniformsCode += cProp.GetUniformCode();//.Replace(cProp.NameCode, cProp.NameCode + GetInstanceId()) + "\n";
        }
        return uniformsCode;
    }

    public override void _EnterTree() {
        MakePropsUnique();
    }

    //Make GLSL Variable Names Unique
    public void MakePropsUnique() {
        int i = 0;
        foreach (var prop in this.Props) {
            Code = Code.Replace(Props[i].NameCode, Props[i].NameCode + GetInstanceId());
            Props[i].NameCode += GetInstanceId();
            i++;
        }
    }

    public void OnRemove() {
        Apphandler.currentViewer.activeLayer.RemoveFilter(this);
    }

    public void UpdateUI() {
        foreach (Prop cProp in Props)
            cProp.UpdateUIProp();
    }


    public Panel UI;
    public Panel BuildUI() {
        UI = new Panel();
        UI.RectMinSize = new Vector2(300, 48 + Props.Length * 35);
        UI.MarginBottom = 10;

        HBoxContainer btnRow = new HBoxContainer();
        btnRow.RectPosition = new Vector2(160, 0);

        Button btnClose = new Button();
        btnClose.Text = " X ";
        
        Button btnUp = new Button();
        btnUp.Text = " ^ ";

        Button btnDown = new Button();
        btnDown.Text = " v ";

        btnRow.AddChild(btnUp);
        btnRow.AddChild(btnDown);
        btnRow.AddChild(btnClose);

        btnClose.Connect("pressed", this, nameof(OnRemove));
        //btnUp.Connect("pressed", FilterStack.instance, nameof(FilterStack.instance.MoveFilter), new Godot.Collections.Array{this, true});
        //btnDown.Connect("pressed", FilterStack.instance, nameof(FilterStack.instance.MoveFilter), new Godot.Collections.Array{this, false});

        GridContainer UIGrid = new GridContainer();
        UIGrid.Columns = 2;

        Label labelName = new Label();
        labelName.Name = "Labelname";
        labelName.Text = Name;
        UI.AddChild(labelName);
        UI.AddChild(btnRow);

        foreach (var cProp in this.Props) {
            HBoxContainer HBox = new HBoxContainer();
            Label propLabel = new Label();
            propLabel.Text = cProp.NameUI;

            UIGrid.AddChild(propLabel);
            UIGrid.AddChild(cProp.BuildUI());
        }
        UIGrid.RectPosition = new Vector2(0, 48);
        UI.AddChild(UIGrid);
        return UI;
    }
}
