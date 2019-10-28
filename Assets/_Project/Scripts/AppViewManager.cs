using AWSSDK.Examples;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AppViewManager : MonoBehaviour
{
    public static AppViewManager Instance;

    [SerializeField] private GameObject goContentText;
    [SerializeField] private GameObject goContentVideo;

    [SerializeField] private Text textContent;
    [SerializeField] private VideoPlayer videoContent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        goContentText.SetActive(false);
        goContentVideo.SetActive(false);
        AppViewS3Manager.Instance.OnGetContentNow += GetContent;
    }

    private void GetContent(string contentReceived)
    {
        print("GetContent:: " + contentReceived);
        var requestContent = contentReceived.Split(Constants.CHAR_DIVISION_CONTENT);

        var typeContent = requestContent[0];
        var content = requestContent[1];

        if (typeContent.Equals(Constants.TYPE_CONTENT_TEXT))
        {
            goContentText.SetActive(true);
            textContent.text = content;
        }
        else
        {
            videoContent.url = content;
            goContentVideo.SetActive(true);
            videoContent.Play();
        }
    }

    public void DisableContents()
    {
        goContentText.SetActive(false);
        goContentVideo.SetActive(false);
        videoContent.Stop();
    }
}