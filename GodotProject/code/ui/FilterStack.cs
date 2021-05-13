using Godot;
using System;
using System.Collections.Generic;

public class FilterStack : VBoxContainer {

    public static List<Filter> filterList = new List<Filter>();

    public void BuildStack() {
        if (Shaderer.instance.Material == null) {
            ShaderMaterial m = new ShaderMaterial();
            Shader shader = new Shader();
            m.Shader = shader;
            Shaderer.instance.Material = m;
        }

        foreach (Filter filter in filterList) {
            AddChild(filter.UI);
            /*TODO - this throws an error when multiple instances of the same filter are added: 
            / Can't add child to FilterStackContainer, already has parent FilterStackContainer
            It doesn't seem like an issue though... possible memory leak?*/
        }

        Shaderer.instance.GenerateShader(filterList);
    }

    public void Remove(Filter filter) {
        filterList.Remove(filter);
        this.RemoveChild(filter.UI);
        Shaderer.instance.GenerateShader(filterList);
    }

    public static FilterStack instance;

    public static void AddFilter(Filter filter) {
        filterList.Add(filter.NewInstance());
    }
    
    public override void _Ready() {

        if (FilterStack.instance == null) {
            FilterStack.instance = this;
        } else {
            GD.PrintErr("Just One Instance of Filterstack is allowed");
        }

        BuildStack();
    }

}
