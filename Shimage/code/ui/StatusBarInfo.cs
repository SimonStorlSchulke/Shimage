using Godot;
using System;

public class StatusBarInfo : Label {
    public static StatusBarInfo instance;


    public override void _Ready() {
        instance = this;
    }

    public static void Display(string text) {
        instance.Text = text;
    }
}
