using Godot;
using System.Collections.Generic;

public class FilterNode : Panel
{

    public PointFilter filter; //TODO Restrict access

    // TODO (currently unused - later call from PointFilter -> BuildUI)
    public void FilterNodeBuild(string filterType, Dictionary<string, object> properties)
    {
        filter = PointFilter.GetFilterOfType(filterType);

        GetNode<Label>("UIList/LabelName").Text = filterType;

        foreach (var prop in filter.properties)
        {
            HBoxContainer HBox = new HBoxContainer();
            Label propLabel = new Label();
            propLabel.Text = prop.Key;

            HBox.AddChild(propLabel);

            var propType = prop.Value.GetType();
            
            if (propType == typeof(float)) {
                HSlider slider = new HSlider();
                slider.RectMinSize = new Vector2(120, 0);
                HBox.AddChild(slider);
            }

            GetNode("UIList").AddChild(HBox);
        }
    }
}
