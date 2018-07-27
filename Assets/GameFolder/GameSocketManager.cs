using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSocketManager : MonoBehaviour {
    public static Client Socket { get; set; }
    void Awake()
    {
        //string url = "http://23.101.116.87:52724/";
        string url = "http://localhost:52724/";
        DontDestroyOnLoad(this.gameObject);

        Debug.Log("Awake! " + url);
            Socket = new Client(url);
            Socket.Opened += SocketOpened;
            Socket.Message += SocketMessage;
        
        Socket.Connect();
        string mail = ONLOAD.mail;
        Socket.Emit("connection", @"{'mail':'wonsoek'}");
        Socket.On("NewBeeIsHere", (data) =>
        {
            JObject jobj = JObject.Parse((string)data.Json.args[0]);
            string _name = jobj["name"].ToString();
            Debug.Log(_name + "is online now!");

        });
        //Socket.Emit("connections", "wonsoek@naver.com");
        Socket.Send("HEllo........");
    }
    private void SocketOpened(object sender, System.EventArgs e)
    {

        Debug.Log("Socket Opened");
    }
    private void SocketMessage(object sender, MessageEventArgs e)
    {
        if (e != null && e.Message.Event == "message")
        {
            string msg = e.Message.MessageText;
            Debug.Log("MSSSSSSSSSSSG : " + msg);
        }
    }
    void OnDisable()
    {
    }
}
