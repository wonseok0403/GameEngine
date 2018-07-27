using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadSQL : MonoBehaviour
{

    public Text mytex2t = null;
    public string url = "http://wonseok.azurewebsites.net/Users/GetMembers";
    IEnumerator DownloadSpheres()
    {
        WWW www = new WWW(url);
        yield return www;
        yield return new WaitForSeconds(2f);
        ExtractSpheres(www.text);
        if (www.error == null)
        {
            string json = JsonUtility.ToJson(www);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }
    }
    public void Starts()
    {
        print("Started sphere import...\n");
        StartCoroutine(DownloadSpheres());
    }
    void ExtractSpheres(string json)
    {
        // Create a JSON object from the text stream 
        mytex2t.text = json;
    }
}
