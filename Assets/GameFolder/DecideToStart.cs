using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SocketIOClient;

public class DecideToStart : MonoBehaviour {
    public Text BallName;
    public GameObject go = null;

    List<string> buffer = new List<string>();

    // Use this for initialization
    void Start()
    {
        Debug.Log("Gamesockmanager is starting...");
        GameObject go = GameObject.Find("GSMObject");
        if( go == null)
        {
            Debug.Log("Something is wrong");
        }
        else
        {
            Debug.Log("Okay found it");
        }
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
    public void Decide()
    {
        Debug.Log("Sending! : " + BallName.text);

        ChangeScene();
    }

    public void ChangeScene()
    {
        ONLOAD.username = BallName.text;
        ONLOAD.mail = UserConfigDetails.mail;
        if( go == null)
        {
            go = GameObject.Find("GSMObject");
        }
        DontDestroyOnLoad(go);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
