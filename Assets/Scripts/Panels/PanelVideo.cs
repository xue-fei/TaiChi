using DG.Tweening;
using RenderHeads.Media.AVProVideo;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PanelVideo : MonoBehaviour
{
    public MediaPlayer mediaPlayer;
    public DisplayUGUI displayUGUI;
    public TMP_Dropdown dropdown;
    string nowUrl = "";
    VideoType videoType = VideoType.Girl1;

    /// <summary>
    /// 小姐姐短视频
    /// </summary>
    string girl1Url = "https://tucdn.wpon.cn/api-girl/index.php?wpon=json";
    /// <summary>
    /// PC端风景视频
    /// </summary>
    string sceneryUrl = "https://v.api.aa1.cn/api/api-fj/index.php?aa1=suf7y58th48u935";
    /// <summary>
    /// 极品实时抖音美女主播短视频直链版
    /// </summary>
    string girl2Url = "https://zj.v.api.aa1.cn/api/video_dyv2";
    /// <summary>
    /// 高质量小姐姐秒播线路
    /// </summary>
    string girl3Url = "https://v.api.aa1.cn/api/api-girl-11-02/index.php?type=json";
    /// <summary>
    /// 小姐姐短视频第二版
    /// </summary>
    string girl4Url = "https://v.api.aa1.cn/api/api-dy-girl/index.php?aa1=json";

    private void Awake()
    {
        mediaPlayer = transform.Find("AVPro Media Player").GetComponent<MediaPlayer>();
        mediaPlayer.Events.AddListener(OnVideoEvent);
        displayUGUI = transform.Find("AV Pro Video uGUI").GetComponent<DisplayUGUI>();
        dropdown = transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnSelect);
        List<TMP_Dropdown.OptionData> listOptions = new List<TMP_Dropdown.OptionData>();
        listOptions.Add(new TMP_Dropdown.OptionData("小姐姐短视频"));
        listOptions.Add(new TMP_Dropdown.OptionData("PC端风景视频"));
        listOptions.Add(new TMP_Dropdown.OptionData("实时抖音美女"));
        listOptions.Add(new TMP_Dropdown.OptionData("高质量小姐姐"));
        listOptions.Add(new TMP_Dropdown.OptionData("高质量小姐姐2"));
        dropdown.AddOptions(listOptions);
        nowUrl = girl1Url;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayVideo();
    }

    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode er)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.FirstFrameReady:
                {
                    displayUGUI.DOColor(Color.white, 0.3f);
                }
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                {
                    PlayVideo();
                }
                break;
            default:
                break;
        }
        if (er != ErrorCode.None)
        {
            PlayVideo();
        }
    }

    private void PlayVideo()
    {
        displayUGUI.DOColor(Data.blackColor, 0.3f);
        StartCoroutine(RequestVideoUrl());
    }

    private IEnumerator RequestVideoUrl()
    {
        if (videoType == VideoType.Scenery)
        {
            mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, nowUrl, true);
            yield break;
        }
        UnityWebRequest uwr = UnityWebRequest.Get(nowUrl);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("请求失败");
            yield break;
        }
        else
        {
            string videoUrl = "";
            try
            {
                if (videoType == VideoType.Girl1
                    || videoType == VideoType.Girl3
                    || videoType == VideoType.Girl4)
                {
                    Girl1Data data = new Girl1Data();
                    JsonUtility.FromJsonOverwrite(uwr.downloadHandler.text, data);
                    videoUrl = "https:" + data.mp4;
                }
                if (videoType == VideoType.Girl2)
                {
                    Girl2Data data = new Girl2Data();
                    JsonUtility.FromJsonOverwrite(uwr.downloadHandler.text, data);
                    videoUrl = data.url;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                yield break;
            }
            mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, videoUrl, true);
        }
    }

    private void OnSelect(int index)
    {
        switch (index)
        {
            case 0:
                videoType = VideoType.Girl1;
                nowUrl = girl1Url;
                break;
            case 1:
                videoType = VideoType.Scenery;
                nowUrl = sceneryUrl;
                break;
            case 2:
                videoType = VideoType.Girl2;
                nowUrl = girl2Url;
                break;
            case 3:
                videoType = VideoType.Girl3;
                nowUrl = girl3Url;
                break;
            case 4:
                videoType = VideoType.Girl4;
                nowUrl = girl4Url;
                break;
            default:
                videoType = VideoType.Scenery;
                nowUrl = sceneryUrl;
                break;
        }

        PlayVideo();
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
        Girl1,
        Girl2,
        Girl3,
        Girl4,
    }

    public class Girl1Data
    {
        public int error;
        public int result;
        public string mp4;
    }

    public class Girl2Data
    {
        public int code;
        public string msg;
        public string dsc;
        public string url;
        public string info;
    }
}