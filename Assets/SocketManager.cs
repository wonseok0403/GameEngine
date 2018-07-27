using UnityEngine;
using System.Collections;
using SocketIOClient;

public class SocketManager : MonoBehaviour
{
    string url = "http://localhost:52723/";
    public static Client Socket { get; set; }

    void Awake()
    {
        Debug.Log("Awake! " + url);
        Socket = new Client(url);
        Socket.Opened += SocketOpened;
        Socket.Connect();


        if (Socket.IsConnected)
        {
            Debug.Log("YES!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
        else
        {
            Debug.Log("No.");
        }
    }

    private void SocketOpened(object sender, System.EventArgs e)
    {
        Debug.Log("Socket Opened");
    }

    void OnDisable()
    {
        Socket.Close();
    }
}
