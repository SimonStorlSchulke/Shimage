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

    public virtual void AddCustomPropUI(Control customUI, Prop forProp) {
        //TODO
    }

    public Panel UI;
    public Panel BuildUI() {
        UI = new Panel();
        UI.MarginBottom = 10;

        HBoxContainer TopRow = new HBoxContainer();
        TopRow.RectMinSize = new Vector2(285, 24);
        TopRow.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;

        Label labelName = new Label();
        labelName.Name = "Labelname";
        labelName.Text = Name;
        TopRow.AddChild(labelName);

        Control spacer = new Control();
        spacer.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
        TopRow.AddChild(spacer);

        Button btnUp = new Button();
        btnUp.Text = " ^ ";
        TopRow.AddChild(btnUp);

        Button btnDown = new Button();
        btnDown.Text = " v ";
        TopRow.AddChild(btnDown);

        Button btnClose = new Button();
        btnClose.Text = " X ";
        TopRow.AddChild(btnClose);

        btnClose.Connect("pressed", this, nameof(OnRemove));
        //btnUp.Connect("pressed", FilterStack.instance, nameof(FilterStack.instance.MoveFilter), new Godot.Collections.Array{this, true});
        //btnDown.Connect("pressed", FilterStack.instance, nameof(FilterStack.instance.MoveFilter), new Godot.Collections.Array{this, false});

        GridContainer UIGrid = new GridContainer();
        UIGrid.Columns = 2;

        
        UI.AddChild(TopRow);

        foreach (var cProp in this.Props) {
            Label propLabel = new Label();
            propLabel.Text = cProp.NameUI;

            UIGrid.AddChild(propLabel);
            // TODO add custom PropUI here
            UIGrid.AddChild(cProp.BuildUI());
        }
        UIGrid.RectPosition = new Vector2(0, 48);
        UI.AddChild(UIGrid);
        UI.RectMinSize = UIGrid.RectSize + new Vector2(0, UIGrid.RectPosition.y + 5);
        return UI;
    }
}
