using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetText : MonoBehaviour
{
    public Text text; 

    void Start()
    {
        StartCoroutine(GetText_Coroutine());
    }

    IEnumerator GetText_Coroutine()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://s3.amazonaws.com/hackathoncotidiano/textExample.txt"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                text.text = www.downloadHandler.text;

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }
}