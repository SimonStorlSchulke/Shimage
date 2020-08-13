using Godot;

using System.Collections.Generic;

/*
public struct Connection {
    public GraphNode from;
    public int fromSlot;

    public GraphNode to;
    public int toSlot;

    public Connection(GraphNode from, int fromSlot, GraphNode to, int toSlot) {
        this.from = from;
        this.fromSlot = fromSlot;
        this.to = to;
        this.toSlot = toSlot;
    }
}*/

public class GraphEdit : Godot.GraphEdit {

    void onConnectionRequest(string from, int fromSlot, string to, int toSlot) {
        if (from != to)
            ConnectNode(from, fromSlot, to, toSlot);
    }

    void onDisconnectionRequest(string from, int fromSlot, string to, int toSlot) {
        DisconnectNode(from, fromSlot, to, toSlot);
    }

    public void EvaluateTree() {
        GD.Print("Evaluate Tree");
    }
}
