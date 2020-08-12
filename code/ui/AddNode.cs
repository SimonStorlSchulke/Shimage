using System.Collections.Generic;
using Godot;
using System;

public class AddNode : MenuButton {
    static PopupMenu popup;
    static GraphNode[] Nodes;

    static List<PackedScene> graphNodes = new List<PackedScene>();
    static GraphEdit ge;

    public override void _Ready() {
        popup = GetPopup();
        ge = GetNode<GraphEdit>("../../../GraphEdit");
        CollectGraphNodes();
    }

    void AddGraphNode(int ID) {
        var graphNode = (GraphNode)graphNodes[ID].Instance();
        ge.AddChild(graphNode);
    }

    //Collect Nodes from nodes directory and add them to the List
    void CollectGraphNodes() {
        Directory nodeDir = new Directory();

        //Iterate through nodes folder
        if (nodeDir.Open("res://nodes") == Error.Ok) {
            nodeDir.ListDirBegin();
            string filename = nodeDir.GetNext();
            while (filename != "") {
                if (!nodeDir.CurrentIsDir()) {

                    //Add to nodes List
                    graphNodes.Add(GD.Load<PackedScene>("res://nodes/" + filename));

                    //add to Menu
                    filename = filename.Remove(filename.Length - 5); //Remove .tscn extension
                    popup.AddItem(filename);
                }
                filename = nodeDir.GetNext();
            }
        }
        popup.Connect("id_pressed", this, nameof(AddGraphNode));
    }
}
