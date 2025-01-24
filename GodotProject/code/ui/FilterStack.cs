using Godot;
using System;
using System.Collections.Generic;

public partial class FilterStack : VBoxContainer {

    public static List<Filter> filterList = new List<Filter>();

    //Maybe move to apphandler or something
    public Layer selectedLayer;

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
        filter.UI.QueueFree();
        Shaderer.instance.GenerateShader(filterList);
    }

    public static FilterStack instance;

    public static void AddFilter(Filter filter) {
        filterList.Add(filter.NewInstance());
    }

    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    public void MoveFilter(Filter filter, bool updown) 
    {
        int val = updown? -1 : 1;
        int i = filterList.IndexOf(filter);
        GD.Print(i);
        if (val == 1 && i == filterList.Count-1 || val == -1 && i == 0 ) {return;}

        MoveChild(GetChild(i), i+val);
        
        Filter tmp = filterList[i];
        filterList[i] = filterList[i+val];
        filterList[i+val] = tmp;
        
        Shaderer.instance.GenerateShader(filterList);
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
