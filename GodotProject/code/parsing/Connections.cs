using Godot;
using System;
using System.Collections.Generic;

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

public class Connections : Node {
    GraphEdit ge;
    List<Connection> connections = new List<Connection>();

    public override void _Ready() {
        ge = GetParent<GraphEdit>();
    }

    public List<Connection> get() => connections;

    public void generateConnectionList() {

        foreach (Godot.Collections.Dictionary connection in ge.GetConnectionList()) {
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
            //TODO Exception handling
            connections.Add(c);
        }
    }
}