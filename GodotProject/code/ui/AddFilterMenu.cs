using Godot;
using System;

public class AddFilterMenu : MenuButton
{
    public override void _Ready()
    {
        GetPopup().AddItem("Exposure");
        GetPopup().AddItem("Vignette");
        GetPopup().AddItem("Multiply Color");
        GetPopup().AddItem("Hue Shift");
        GetPopup().AddItem("Levels");

        GetPopup().Connect("id_pressed", this, nameof(OnAddFilter));
    }

    public void OnAddFilter(int id) {
        string itemName = GetPopup().GetItemText(id);
        switch (itemName)
        {
            case ("Exposure"):
                FilterStack.AddFilter(Filters.Exposure);
                break;
            case ("Vignette"):
                FilterStack.AddFilter(Filters.Vignette);
                break;
            case ("Multiply Color"):
                FilterStack.AddFilter(Filters.MultiplyColor);
                break;
            case ("Hue Shift"):
                FilterStack.AddFilter(Filters.HueShift);
                break;
            case ("Levels"):
                FilterStack.AddFilter(Filters.Levels);
                break;
        }
        FilterStack.instance.BuildStack();
    }
}