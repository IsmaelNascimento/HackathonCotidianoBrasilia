using AWSSDK.Examples;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WebManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image textPanel;
    [SerializeField] private Image videoPanel;
    private bool contentIsText = true;

    [SerializeField] private Text contentText;
    [SerializeField] private Text contentVideo;

    [Space(10)]
    [Header("VWS")]
    [SerializeField] private Transform LogPanelContent;
    [SerializeField] private GameObject LogMessagePrefab;
    [SerializeField] private Scrollbar LogPanelScroll;
    [SerializeField] private Image TargetImage;


    #endregion

    #region Methods MonoBehaviour

    private void Start()
    {
        S3Example.Instance.OnAddTargetAndContent += OnAddTargetAndContent;
        ConnectToDatabase();
    }

    #endregion

    #region Methods VWS

    public void ConnectToDatabase()
    {
        PlayerPrefs.SetString("accessKey", VWS.Instance.accessKey);
        PlayerPrefs.SetString("secretKey", VWS.Instance.secretKey);
        PlayerPrefs.Save();

        LogMessage(Constants.SERVER_REQUEST_LOGIN);
        VWS.Instance.RetrieveDatabaseSummary(response =>
            {
                if (response.result_code == "Success")
                {
                    string log = "Name: " + response.name + "\n";
                    log += "Active images: " + response.active_images + "\n";
                    log += "Failed images: " + response.failed_images + "\n";
                    log += "Inactive images: " + response.inactive_images;

                    LogMessage(Constants.SERVER_LOGIN_SUCCESS);
                }
                else
                {
                    LogMessage(Constants.SERVER_LOGIN_FAILED);
                    ConnectToDatabase();
                }
            }
        );
    }

    public void OnButtonAddTargetClicked()
    {
        S3Example.Instance.GetCountObjects();
    }

    #endregion

    #region Methods Created

    void LogMessage(string message)
    {
        GameObject btnObject = Instantiate(LogMessagePrefab);
        btnObject.GetComponentInChildren<Text>().text = message;
        btnObject.transform.SetParent(LogPanelContent);
        btnObject.transform.localScale = Vector3.one;
        LogPanelScroll.value = 0f;
    }

    public void OnButtonPickImageClicked()
    {
        TargetImage.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
    }

    private void OnAddTargetAndContent(int count)
    {
        var fileName = ("NameTarget" + count);
        print("fileName:: " + fileName);
        LogMessage(Constants.SERVER_REQUEST_NEW_TARGET);
        VWS.Instance.AddTarget(fileName,
            1,
            TargetImage.sprite.texture,
            true,
            fileName,
            response =>
            {
                if (response.result_code == "Success" || response.result_code == "TargetCreated")
                {
                    LogMessage(Constants.SERVER_UPLOAD_TARGET_SUCCESS);
                }
                else
                {
                    LogMessage(Constants.SERVER_UPLOAD_TARGET_FAILED);
                }
            }
        );

        try
        {
            if (contentIsText)
            {
                var content = "text)" + contentText.text;
                LogMessage(Constants.SERVER_REQUEST_CONTENT_TARGET);
                S3Example.Instance.PostObjectNew(content, (fileName + ".txt"));
                LogMessage("ACABOU UPLOAD DE CONTENT TARGET");
            }
            else
            {
                var content = "video)" + contentVideo.text;
                LogMessage(Constants.SERVER_REQUEST_CONTENT_TARGET);
                S3Example.Instance.PostObjectNew(content, (fileName + ".txt"));
                LogMessage("ACABOU UPLOAD DE CONTENT TARGET");
            }
            
        }
        catch (Exception ex)
        {
            print(ex);
        }
    }

    public void OnButtonSelectTypeContenClicked(Image image)
    {
        textPanel.color = Color.grey;
        videoPanel.color = Color.grey;
        image.color = Color.green;
        contentIsText = !contentIsText;
    }

    #endregion

}