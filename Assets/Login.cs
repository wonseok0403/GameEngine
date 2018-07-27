using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    public Text LoginText = null;
    public Text PasswordText = null;
    public Text ConsoleBox = null;
    public Button Gamestart;
    public static string UserMail = null;
    public Button ChatButton;

    // public string url = "http://wonseokdotnet2.azurewebsites.net/Users/Login";
    IEnumerator DownloadSpheres()
    {
        string Input_Id, Input_Pw; // 2 is main server
        string url = "http://wonseokdotnet1.azurewebsites.net/Users/Login";
        Input_Id = LoginText.text;
        UserMail = Input_Id;
        Input_Pw = PasswordText.text;
        Debug.Log(url);
        string json = @"{ 'Email':'" + Input_Id + "','Password':'" + Input_Pw + "'}";
        byte[] data = Encoding.UTF8.GetBytes(json.ToCharArray());
        json = json.Replace("'", "\"");
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json");
        WWW www = new WWW(url, data, header);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    private void Start()
    {
        Debug.Log("Login is started");
        Gamestart.interactable = false;
        ChatButton.interactable = false;
    }
    IEnumerator WaitForRequest(WWW data)
    {
        yield return data; // Wait until the download is done
        if (data.error != null)
        {
            // Login Fail
            Debug.Log("There was an error sending request: " + data.error);
            Gamestart.interactable = false;
            ChatButton.interactable = false;
        }
        else
        {
            // Login Failed
            Debug.Log("WWW Request: " + data.text);
            Gamestart.interactable = true;
            ChatButton.interactable = true;
        }
    }
    public void LoginStart()
    {
        print("Started sphere import...\n");
        StartCoroutine(DownloadSpheres());
    }

}
