using Godot;
using System;

public class Connection {
    public NodeBase from;
    public int fromSlot;

    public NodeBase to;
    public int toSlot;

    public Connection(NodeBase from, int fromSlot, NodeBase to, int toSlot) {
        this.from = from;
        this.fromSlot = fromSlot;
        this.to = to;
        this.toSlot = toSlot;
    }
    public Connection() { }
}

public class Parser : Node {
    GraphEdit ge;
    Godot.Collections.Array tree;
    public override void _Ready() {
        ge = GetParent<GraphEdit>();
    }


    void OnEvaluateTreePressed() {

        tree = ge.GetConnectionList();
        foreach (Godot.Collections.Dictionary connection in tree) {

            var en = connection.GetEnumerator();
            Connection c = new Connection();
            while (en.MoveNext()) {

                switch ((string)en.Key) {
                    case ("from"):
                        c.from = ge.GetNode<NodeBase>((string)en.Value);
                        break;
                    case ("from_port"):
                        c.fromSlot = (int)en.Value;
                        break;
                    case ("to"):
                        c.to = ge.GetNode<NodeBase>((string)en.Value);
                        break;
                    case ("to_port"):
                        c.fromSlot = (int)en.Value;
                        break;
                    default:
                        break;
                }
            }
                GD.Print("Connection from " + c.from.Name + " to " + c.to.Name);
        }
    }
}