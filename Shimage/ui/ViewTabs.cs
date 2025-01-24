using Godot;
using System;

public partial class ViewTabs : TabBar {
    public void RedrawTabs() {

        for (int iTab = 0; iTab < GetTabCount(); iTab++) {
            RemoveTab(iTab);
        }

        foreach (Viewer view in Apphandler.openViewers) {
            AddTab(view.projectName);
        }
    }

}
