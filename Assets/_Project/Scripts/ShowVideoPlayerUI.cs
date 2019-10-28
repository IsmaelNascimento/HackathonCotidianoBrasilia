using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
public class ShowVideoPlayerUI : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(loadVideoFromThisURL(videoUrl));
    }

    [SerializeField]
    internal VideoPlayer myVideoPlayer;
    string videoUrl = "https://r6---sn-q4fl6n7y.googlevideo.com/videoplayback?mime=video%2Fmp4&clen=7849956&ipbits=0&fvip=6&ratebypass=yes&requiressl=yes&beids=%5B9466592%5D&pl=19&fexp=9466586,23709359&source=youtube&sparams=clen,dur,ei,expire,gir,id,initcwndbps,ip,ipbits,itag,lmt,mime,mip,mm,mn,ms,mv,pl,ratebypass,requiressl,source&c=WEB&key=cms1&id=o-AE3ermAxei5aSnIAl5P8CKZRiANeQHCQyk0T0_29Uj_a&expire=1529864737&ip=197.250.8.162&lmt=1528740116499545&ei=wY0vW76PGNCH1wag27fwDA&dur=175.682&itag=18&signature=2A158EBF641326A4B3633D31E3566F98BD2DBDDB.2810E999A10DF3C80747EE736280CA4340D0C7C6&gir=yes&video_id=b7SVejDyaJA&title=CAMP+6+Cotidiano+-+Formul%C3%A1rio+aberto&rm=sn-8vq5jvhu1-q5gl7l&req_id=e1318d6863aca3ee&redirect_counter=2&cm2rm=sn-aigeey7d&cms_redirect=yes&mip=186.222.141.69&mm=34&mn=sn-q4fl6n7y&ms=ltu&mt=1529843007&mv=m";
    private IEnumerator loadVideoFromThisURL(string _url)
    {
        UnityWebRequest _videoRequest = UnityWebRequest.Get(_url);

        yield return _videoRequest.SendWebRequest();

        if (_videoRequest.isDone == false || _videoRequest.error != null)
        { Debug.Log("Request = " + _videoRequest.error); }

        Debug.Log("Video Done - " + _videoRequest.isDone);

        byte[] _videoBytes = _videoRequest.downloadHandler.data;

        string _pathToFile = Path.Combine(Application.persistentDataPath, "movie.mp4");
        File.WriteAllBytes(_pathToFile, _videoBytes);
        Debug.Log(_pathToFile);
        StartCoroutine(PlayThisURLInVideo(_pathToFile));
        yield return null;
    }


    private IEnumerator PlayThisURLInVideo(string _url)
    {
        myVideoPlayer.source = VideoSource.Url;
        myVideoPlayer.url = _url;
        myVideoPlayer.Prepare();

        while (myVideoPlayer.isPrepared == false)
        { yield return null; }

        Debug.Log("Video should play");
        myVideoPlayer.Play();
    }
}