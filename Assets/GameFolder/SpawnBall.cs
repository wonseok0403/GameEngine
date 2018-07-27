using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJson;
using SocketIOClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class VectorValue
{
    public VectorValue(string name, int x, int y, int z)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public VectorValue(string json)
    {
        JObject jobj = JObject.Parse(json);
        this.x = (int)jobj["x"];
        this.y = (int)jobj["y"];
        this.z = (int)jobj["z"];
    }
    public string name { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }
}

public class SpawnBall : MonoBehaviour {
    public Transform Ball;
    public string NAME = "";
    public float dist = 10.0f;
    public float height = 5.0f;
    public float smoothRotate = 5.0f;
    List<VectorValue> ClientLists = new List<VectorValue>(); // client lists
    void Awake()
    {


    }
    private void SocketOpened(object sender, System.EventArgs e)
    {
        Debug.Log("Socket Opened");
    }
    public void DownloadVectors()
    {
        bool flag;
        string url = "http://wonseokdotnet2.azurewebsites.net/GameAccess/GetVector/";
        foreach (VectorValue i in ClientLists)
        {
            string _url = url + i.name + "/";
            using (var client = new WebClient())
            {
                var json = client.DownloadString(_url); 
                VectorValue vv = new VectorValue(json);
                Debug.Log("NAME IS THIS? " + NAME);
                if (i.name == NAME)
                {
                    Ball.position = new Vector3(vv.x, vv.y, vv.z);
                }
            }
        }
    }
    IEnumerator WaitForRequest(WWW data)
    {
        yield return data; // Wait until the download is done
        if (data.error != null)
        {
            // Login Fail
            Debug.Log("There was an error sending request: " + data.error);
        }
        else
        {
            // Login Failed
            Debug.Log("WWW Request: " + data.text);
        }
    }
    public void Start()
    {
        int x, y, z;
            GameSocketManager.Socket.Emit("sMsg", ONLOAD.mail);
        if (GameSocketManager.Socket.IsConnected == false)
        {
            GameSocketManager.Socket = new Client("http://localhost:52724/");
            GameSocketManager.Socket.Opened += SocketOpened;
            GameSocketManager.Socket.Connect();
            //GameSocketManager.Socket.Emit("connections", "hi");
        }
        if (GameSocketManager.Socket.IsConnected == true)
        {
            Debug.Log("Socket is connected, I will add some listeners!");
            GameSocketManager.Socket.On("connectionssuccess", (data) =>
            {
                Debug.Log("connectionssucess");
                //GameSocketManager.Socket.Emit("connections", "hi");
            });
            GameSocketManager.Socket.On("VectorChanged", (data) =>
            {
                DownloadVectors();
            });
            GameSocketManager.Socket.On("NewBee", (data) =>
            {
                int _x, _y, _z;
                Debug.Log("NewBee is called!");
                JObject jobj = JObject.Parse((string)data.Json.args[0]);

                NAME = jobj["name"].ToString();
                _x = (int)jobj["x"];
                _y = (int)jobj["y"];
                _z = (int)jobj["z"];
                Debug.Log(_x.ToString() + " " + NAME + " is parsed");
                ClientLists.Add(new VectorValue(NAME, _x, _y, _z));
                //GameSocketManager.Socket.Emit("connections", "hi hello ahnnyung nae ga nu gun ji ah ni?");
            });
        }
        else
        {
            Debug.Log("aisjfioajoweijfoiawjeofijaweoifjaoiwef");
        }
        Debug.Log("Let's emit!");
        if (GameSocketManager.Socket.IsConnected == true)
        {

            Debug.Log("YES!!!!!!!!!!!!IS!!!!!!!!!!!!TRUE!!!!!!!!!!!!!!!!!!!!!!!!!");
            GameSocketManager.Socket.Send("HI server!");
        }

    }
    private void LateUpdate()
    {
        float currYangle = Mathf.LerpAngle(Camera.main.transform.eulerAngles.y, Ball.eulerAngles.y, smoothRotate * Time.deltaTime);
        Quaternion rot = Quaternion.Euler(0, currYangle, 0);
        Camera.main.transform.position = Ball.position - (Vector3.forward*dist)+(Vector3.up * height);
        Camera.main.transform.LookAt(Ball);
    }
    private void Update()
    {
    }
    /*
    public Text ballName = null;
    public int x;
    public int y;
    public int z;

    
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
        if (!SocketManager.Socket.IsConnected)
        {
            chatWindow.text += chatWindow.text + "\n\nPlease Check Status";
        }
        else
        {
            chatWindow.text += chatWindow.text + "\n" + chatText.text;
        }
        chatText.text = "";
    }*/
}
