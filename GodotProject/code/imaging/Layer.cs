using Godot;
using System.Collections.Generic;

public partial class Layer : Node
{
    public string path;
    public Image image;
    public List<Filter> filterList = new List<Filter>();

    public Layer() {

    }

    public void AddFilter(Filter filter) {
        filterList.Add(filter.NewInstance());
    }

    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

}
