using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PanelMusic : MonoBehaviour
{
    public Image image;
    public Button buttonRefresh;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textSinger;
    /// <summary>
    /// https://api.wya6.cn/doc/NetEase_random.html
    /// </summary>
    string apiUrl = "https://api.wya6.cn/api/NetEase_random?return=json";
    string musicPath;
    AudioSource audioSource;
    float pauseTime;

    private void Awake()
    {
        musicPath = Application.persistentDataPath + "/Music";
        if (!Directory.Exists(musicPath))
        {
            Directory.CreateDirectory(musicPath);
        }
        image = transform.Find("Image").GetComponent<Image>();
        buttonRefresh = transform.Find("ButtonRefresh").GetComponent<Button>();
        textName = transform.Find("TextName").GetComponent<TextMeshProUGUI>();
        textSinger = transform.Find("TextSinger").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonRefresh.onClick.AddListener(() => StartCoroutine(RequestMusicUrl()));
        StartCoroutine(RequestMusicUrl());
    }

    private void OnEnable()
    {
        Log.Warning("OnEnable");
        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.time = pauseTime;
        }
    }

    private void OnDisable()
    {
        Log.Warning("OnDisable");
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            pauseTime = audioSource.time;
        }
    }

    private IEnumerator RequestMusicUrl()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(apiUrl);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("请求失败");
            yield break;
        }
        else
        {
            string musicUrl = "";
            try
            {
                Log.Warning(uwr.downloadHandler.text);
                ApiData apiData = JsonConvert.DeserializeObject<ApiData>(uwr.downloadHandler.text);
                if (apiData.code != 200)
                {
                    Log.Error("apiData.code:" + apiData.code);
                    yield break;
                }
                textName.text = apiData.data.name;
                textSinger.text = apiData.data.singer;
                musicUrl = apiData.data.url;
                string coverName = apiData.data.singer + "-" + apiData.data.name;
                ImgLoader.Instance.DownLoad(image, apiData.data.cover, musicPath, coverName);
                string musicFilePath = musicPath + "/" + apiData.data.singer + "-" + apiData.data.name + ".mp3";
                string lyricPath = musicPath + "/" + apiData.data.singer + "-" + apiData.data.name + ".lrc";
                Loom.RunAsync(async () =>
                {
                    File.WriteAllText(lyricPath, apiData.data.lyric);
                    using (var web = new WebClient())
                    {
                        //web.DownloadProgressChanged += (s, e) =>
                        //{
                        //    Debug.LogWarning(e.ProgressPercentage + "%");
                        //};
                        await web.DownloadFileTaskAsync(apiData.data.url, musicFilePath);
                        Loom.QueueOnMainThread(() =>
                        {
                            StartCoroutine(PlayMusic(musicFilePath));
                        });
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                yield break;
            }
        }
    }

    IEnumerator PlayMusic(string musicPath)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(musicPath, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
                yield break;
            }
            AudioClip clip = null;
            try
            {
                clip = DownloadHandlerAudioClip.GetContent(uwr);
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                StartCoroutine(RequestMusicUrl());
                yield break;
            }
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public class ApiData
    {
        public int code;
        public string msg;
        public string desc;
        public string content;
        public string updateTime;
        public Data data = new Data();
    }

    public class Data
    {
        public int id;
        public string name;
        public string singer;
        public string duration;
        public string cover;
        public string url;
        public string lyric;
    }
}