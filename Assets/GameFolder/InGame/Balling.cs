using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balling : MonoBehaviour {

    List<string> buffer = new List<string>();

    // Use this for initialization
    void Start()
    {
        Debug.Log("ChatTest is starting...");
        GameSocketManager.Socket.On("rMsg", (data) =>
        {
            Debug.Log(data.Json.args[0]);
            buffer.Add(data.Json.args[0].ToString());
        });
    }

    void Update()
    {
        // update is needed. maybe singloeton is good choice.

    }
    public void Disconnect()
    {
        try
        {
            GameSocketManager.Socket.Dispose();
        }
        catch
        {
            Debug.Log("Dispose failed");
        }
        try
        {
            GameSocketManager.Socket.Close();
        }
        catch
        {
            Debug.Log("Close Failed");
        }
    }
}
