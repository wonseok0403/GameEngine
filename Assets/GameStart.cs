using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour {
    private string mail = Login.UserMail;
    public Text LoginText = null;
    GameObject go = null;

    IEnumerator DownloadSpheres()
    {
        string Input_Id, Input_Pw;
        // 2 is main server
        string url = "http://wonseokdotnet1.azurewebsites.net/GameAccess/AccessServer";
        Debug.Log(url);
        string json = @"{ 'Email':'" + mail+ "'}";
        byte[] data = Encoding.UTF8.GetBytes(json.ToCharArray());
        json = json.Replace("'", "\"");
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json");
        WWW www = new WWW(url, data, header);
        StartCoroutine(WaitForRequest(www));
        return www;
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
            // Login success
            Debug.Log("WWW Request: " + data.text);
            ChangeScene();
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GameStartButtonClicked()
    {
        StartCoroutine(DownloadSpheres());
    }
    public void ChangeScene()
    {
        UserConfigDetails.mail = LoginText.text;
        go = GameObject.Find("UserConfigs");
        DontDestroyOnLoad(go);
        SceneManager.LoadScene("GameStart");
    }
}
