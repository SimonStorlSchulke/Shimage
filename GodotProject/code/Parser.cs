using Godot;
using System;
using System.Collections.Generic;

public class Parser : Node {

    public static Output activeOutput;

    Connections connections;

    public override void _Ready() {
        connections = GetParent().GetNode<Connections>("Connections");
    }

    void OnEvaluateTreePressed() {
         connections.generateConnectionList();
         GD.Print(connections.get());
    }
}