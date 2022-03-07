using Godot;
using System;

public class ViewTabs : Tabs {
    public void RedrawTabs() {

        for (int iTab = 0; iTab < GetTabCount(); iTab++) {
            RemoveTab(iTab);
        }

        foreach (Viewer view in Apphandler.openViewers) {
            AddTab(view.projectName);
        }
    }

}
