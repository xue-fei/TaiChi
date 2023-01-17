using RenderHeads.Media.AVProVideo;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PanelVideo : MonoBehaviour
{
    public MediaPlayer mediaPlayer;

    VideoType videoType = VideoType.Beauty;
    string sceneryUrl = "https://v.api.aa1.cn/api/api-fj/index.php?aa1=suf7y58th48u935";
    string beautyUrl = "https://tucdn.wpon.cn/api-girl/index.php?wpon=json";
    string url = "";

    // Start is called before the first frame update
    void Start()
    {
        PlayVideo();
    }
      
    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode er)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.FinishedPlaying:
                {
                    Debug.LogWarning("OnPlayEnd");
                    PlayVideo();
                }
                break;
            default:
                break;
        }
        if(er!= ErrorCode.None)
        {
            PlayVideo();
        } 
    }

    private void PlayVideo()
    {
        if (videoType == VideoType.Scenery)
        {
            mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, sceneryUrl, true);
        }
        if (videoType == VideoType.Beauty)
        {
            StartCoroutine(RequestVideoUrl());
        }
    }
     
    private IEnumerator RequestVideoUrl()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(beautyUrl);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("请求失败");
            yield break;
        }
        else
        {
            BeautyData data = new BeautyData();
            try
            {
                JsonUtility.FromJsonOverwrite(uwr.downloadHandler.text, data);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                yield break;
            } 
            mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, "https:" + data.mp4, true);
        }
    }

    enum VideoType
    {
        /// <summary>
        /// 风景
        /// </summary>
        Scenery,
        /// <summary>
        /// 美女
        /// </summary>
        Beauty,
    }

    public class BeautyData
    {
        public int error;
        public int result;
        public string mp4;
    }
}