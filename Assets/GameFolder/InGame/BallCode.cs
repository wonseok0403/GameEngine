using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;


class LoginMembers
{
    public LoginMembers(string _name, int _x, int _y, int _z, GameObject _GO)
    {
        name = _name; x = _x; y = _y; z = _z;
        nowPosition = new Vector3(_x, _y, _z);
        nowMade = true;
    }
    public string name;
    public bool isName(string _name)
    {
        return name == _name;
    }
    public int x, y, z;
    public Vector3 nowPosition;
    public GameObject sphere;
    public bool nowMade;
}
// This is my ball.
public class BallCode : MonoBehaviour {
    public string myName = "";
    public int nx, ny, nz; // now location
    public int num;
    private Rigidbody rigidbody;
    List<LoginMembers> DataMembers;
    GameObject tmpObj;
	// Use this for initialization
	void Start () {
        num = 0;
        tmpObj = new GameObject();
        // Start needs to be query to set fisrt location.
        // after that, it just needs to communicate with server.
        //------------------------------
        myName = ONLOAD.mail;                // my lovely item.
        DataMembers = new List<LoginMembers>();
        //------------------------------
        rigidbody = GetComponent<Rigidbody>();
        DownloadVectors();
        Debug.Log("New ball is here!");
        GameSocketManager.Socket.On(myName, (data) =>
        {
            JObject jobj =   JObject.Parse((string)data.Json.args[0]);
            Debug.Log("My ball : " + jobj["x"].ToString());
        });


    }
    public void DownloadVectors()
    {
        bool flag;
        string url = "http://wonseokdotnet2.azurewebsites.net/GameAccess/GetVector/";
        
        string _url = url + myName + "/";
        using (var client = new WebClient())
        {
            var json = client.DownloadString(_url);
            VectorValue vv = new VectorValue(json);
            Debug.Log("NAME IS THIS? " + myName);
            transform.position = new Vector3(vv.x, vv.y, vv.z);
            
        }
        num = 0;
        GameSocketManager.Socket.On("Locations", (data) =>
        {
            Debug.Log("Socket is online at locations");
            JArray array = JArray.Parse((string)data.Json.args[0]);
            for(int i=0; i<array.Count; i++)
            {
                if (array[i][0].ToString() == ONLOAD.mail) continue;
                Debug.Log(array[i][0].ToString() + " " + array[i][1].ToString() + " " + array[i][2] + " " + array[i][3]);
                for(int j = 0; j<array.Count; j++)
                {
                    Debug.Log("I'm here");
                    if (j > DataMembers.Count-1) // 1 > -1
                    {
                        Debug.Log("This is null!");
                        LoginMembers tmp = new LoginMembers(array[i][0].ToString(), (int)array[i][1], (int)array[i][2], (int)array[i][3], tmpObj);
                        
                        DataMembers.Add(tmp);
                        break;
                    }
                    var m = DataMembers[j];
                    Debug.Log("Let's find it!");
                    if ( m.isName(array[i][0].ToString()) && array[i][0].ToString() != ONLOAD.mail)
                    {
                        Debug.Log("Found it!");
                        //m.sphere.transform.position = new Vector3(m.x, m.y, m.z);
                        m.x = (int)array[i][1]; m.y = (int)array[i][2]; m.z = (int)array[i][3];
                        break;
                    }
                    Debug.Log("Why I am here~?");
                }
            }
        });
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rigidbody.AddForce(movement);
        float _x, _y, _z;
        _x = transform.position.x;
        _y = transform.position.y;
        _z = transform.position.z;
        string str = @"{'name':'" + myName + "', 'x': " + _x.ToString() + ", 'y':" + _y.ToString() + ", 'z':" + _z.ToString() + "}";
        str = str.Replace("'", "\"");
        GameSocketManager.Socket.Emit("connections", str);
        GameSocketManager.Socket.Emit("RequestLocation", str);
        for (int i=0; i<DataMembers.Count; i++)
        {
            if (DataMembers[i].nowMade == true)
            {
                DataMembers[i].sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                DataMembers[i].nowMade = false;
            }
            else
            {
                //---------------------------------------------------------------------------------------------------------------------------------
                //Vector3 letsposition = Vector3.Lerp(DataMembers[i].x, DataMembers[i].y, DataMembers[i].z);
                // DataMembers[i].sphere.transform.position = Vector3.MoveTowards(DataMembers[i].nowPosition, letsposition, Time.deltaTime*100);
                //DataMembers[i].nowPosition = letsposition;
                //-------------------------------------------============== Lerp is added. (jul 27)
                float _radius = DataMembers[i].sphere.transform.GetComponent<SphereCollider>().radius;
                DataMembers[i].sphere.transform.position = Vector3.Lerp(DataMembers[i].sphere.transform.position, new Vector3(_radius+DataMembers[i].x, _radius+DataMembers[i].y, _radius+DataMembers[i].z), Time.deltaTime);
            }
        }
    }
    void Update () {
        //GameSocketManager.Socket.Emit("connections", @"{}");
    }
}
