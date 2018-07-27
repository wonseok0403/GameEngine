using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterface : MonoBehaviour {
    public string txtFieldString = "TextField";
    public string ChatString = " ";
	// Use this for initialization
	void Start () {
        GameSocketManager.Socket.On("RMSG", data =>
        {
            Debug.Log("Recieve Mssg");
            JObject jobj = JObject.Parse((string)data.Json.args[0]);
            Debug.Log("1Line"); 
            ChatString = ChatString + jobj["name"].ToString() + " : " + jobj["msg"].ToString() + "\n";
            Debug.Log("eLine");
            Debug.Log("MSg is " + ChatString);
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        txtFieldString = GUI.TextField(new Rect(135, 25, 800, 30), txtFieldString);
        GUI.TextField(new Rect(25, Screen.height-235, 910, 200), ChatString);
        if (GUI.Button(new Rect(25, 25, 100, 30), "Button"))
        {
            Debug.Log("Button is cliked");
            string txt = @"{'name':'"+ONLOAD.username + "','msg' : '" + txtFieldString+"'}";
            GameSocketManager.Socket.Emit("SMSG", txt);
            txtFieldString = "";
        }
    }
}
