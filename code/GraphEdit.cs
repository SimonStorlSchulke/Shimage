using Godot;

public class GraphEdit : Godot.GraphEdit
{

    void onConnectionRequest(string from, int fromSlot, string to, int toSlot) {
        if (from != to) {
            ConnectNode(from, fromSlot, to, toSlot);
        }
    }

    void onDisconnectionRequest(string from, int fromSlot, string to, int toSlot) {
        DisconnectNode(from, fromSlot, to, toSlot);
    }
}
