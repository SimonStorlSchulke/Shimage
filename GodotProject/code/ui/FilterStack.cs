using Godot;
using System;
using System.Collections.Generic;

public class FilterStack : VBoxContainer {
    [Export]

    public static List<Filter> filterList = new List<Filter>();

    public void BuildStack() {
        foreach (Filter filter in filterList) {
            AddChild(filter.UI);
        }
    }
    
    public override void _Ready() {
        filterList.Add(Filters.Exposure);
        filterList.Add(Filters.MultiplyColor);
        filterList.Add(Filters.Vignette);

        //TODO move to Shaderer
        ShaderMaterial m = new ShaderMaterial();
        Shader shader = new Shader();
        shader.Code = Shaderer.instance.GenerateShader(filterList);

        m.Shader = shader;
        Shaderer.instance.Material = m;
        BuildStack();
    }

}
