using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ChatTest : MonoBehaviour
{
    public Text chatText;
    public Text chatWindow;

    List<string> buffer = new List<string>();

    // Use this for initialization
    void Start()
    {
        Debug.Log("ChatTest is starting...");
        SocketManager.Socket.On("rMsg", (data) =>
        {
            Debug.Log(data.Json.args[0]);
            buffer.Add(data.Json.args[0].ToString());
        });
    }

    void Update()
    {
        // update is needed. maybe singloeton is good choice.
        chatWindow.text = "";
        if (buffer.Count <= 0) return;
        foreach (var b in buffer)
        {
            chatWindow.text += b + "\n";
        }
    }
    public void Disconnect()
    {
        try
        {
            SocketManager.Socket.Dispose();
        }
        catch
        {
            Debug.Log("Dispose failed");
        }
        try
        {
            SocketManager.Socket.Close();
        }
        catch
        {
            Debug.Log("Close Failed");
        }
    }
    public void SendChat()
    {
        Debug.Log("Sending! : " + chatText.text);
        SocketManager.Socket.Emit("sMsg", chatText.text);
        if( !SocketManager.Socket.IsConnected ) {
            chatWindow.text += chatWindow.text + "\n\nPlease Check Status";
        }
        else
        {
            chatWindow.text += chatWindow.text + "\n" + chatText.text;
        }
        chatText.text = "";
    }
}
