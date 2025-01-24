using Godot;

public enum FilterType {
    COLOR,
    DISTORT
}


public partial class Filter : Node {
    public string Code;
    public string UniformsCode;
    public Prop[] Props;
    public Panel UI;
    public FilterType filterType;

    //TODO save uniforms as individual variables and generate code from them.
    public Filter(string _name, Prop[] _props, string _code, FilterType _filterType) {
        this.Name = _name;
        this.Code = _code;
        this.Props = _props;
        this.filterType = _filterType;
        this.UniformsCode = "";
        foreach (var cProp in Props) {
            this.UniformsCode += cProp.GetUniformCode() + "\n";
        }

        BuildUI();
    }

    public void BuildUI() {
        this.UI = new Panel();
        this.UI.CustomMinimumSize = new Vector2(240, 32 + Props.Length * 27);
        this.UI.OffsetBottom = 10;

        HBoxContainer btnRow = new HBoxContainer();
        btnRow.Position = new Vector2(141, 0);

        Button btnClose = new Button();
        btnClose.Text = " X ";
        
        Button btnUp = new Button();
        btnUp.Text = " ^ ";

        Button btnDown = new Button();
        btnDown.Text = " v ";

        btnRow.AddChild(btnUp);
        btnRow.AddChild(btnDown);
        btnRow.AddChild(btnClose);

        btnClose.Connect("pressed", new Callable(this, nameof(OnRemove)));
        
        btnClose.Pressed += () => OnRemove();
        btnUp.Pressed += () => FilterStack.instance.MoveFilter(this, true);
        btnDown.Pressed += () => FilterStack.instance.MoveFilter(this, false);

        GridContainer UIGrid = new GridContainer();
        UIGrid.Columns = 2;

        Label labelName = new Label();
        labelName.Name = "Labelname";
        labelName.Text = this.Name;
        this.UI.AddChild(labelName);
        this.UI.AddChild(btnRow);

        foreach (var cProp in this.Props) {
            HBoxContainer HBox = new HBoxContainer();
            Label propLabel = new Label();
            propLabel.Text = cProp.NameUI;

            UIGrid.AddChild(propLabel);
            UIGrid.AddChild(cProp.BuildUI());
        }
        UIGrid.Position = new Vector2(0, 32);
        this.UI.AddChild(UIGrid);
    }

    public void OnRemove() {
        FilterStack.instance.Remove(this);
        this.QueueFree();
    }

    public void OnMoveUp() {
        FilterStack.instance.MoveFilter(this, true);
    }

    public void OnMoveDown() {
        FilterStack.instance.MoveFilter(this, false);
    }


    //Make GLSL Variable Names Unique
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
        return new Filter(this.Name, NewProps, NewCode, this.filterType);
    }
}