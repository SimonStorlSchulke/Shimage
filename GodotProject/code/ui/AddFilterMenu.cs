using Godot;
using System;

public partial class AddFilterMenu : MenuButton {
    
    public override void _Ready() {
        GetPopup().Connect("id_pressed", new Callable(this, nameof(OnAddFilter)));

        foreach (Filter cFilter in Filters.List) {
            GetPopup().AddItem(cFilter.Name);
        }
    }

    public void OnAddFilter(int id) {
        FilterStack.AddFilter(Filters.List[id]);
        FilterStack.instance.BuildStack();
    }
}