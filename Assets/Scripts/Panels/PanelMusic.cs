using DG.Tweening;
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
    /// https://api.aa1.cn/doc/wyy_music.html
    /// </summary>
    string apiUrl = "https://api.wqwlkj.cn/wqwlapi/wyy_random.php?type=json";
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
                if (apiData.code != 1)
                {
                    Log.Error("apiData.code:" + apiData.code);
                    yield break;
                }
                textName.text = apiData.data.name;
                textSinger.text = "";
                musicUrl = apiData.data.url;
                string coverName = apiData.data.name + ".jpg";
                ImgLoader.Instance.DownLoad(image, apiData.data.picurl, musicPath, coverName);
                string musicFilePath = musicPath + "/" + apiData.data.name + ".mp3";

                if (File.Exists(musicFilePath))
                {
                    StartCoroutine(PlayMusic(musicFilePath));
                }
                else
                {
                    Loom.RunAsync(async () =>
                    {
                        using (var web = new WebClient())
                        {
                            await web.DownloadFileTaskAsync(apiData.data.url, musicFilePath);
                            Loom.QueueOnMainThread(() =>
                            {
                                StartCoroutine(PlayMusic(musicFilePath));
                            });
                        }
                    });
                }
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
            if (audioSource.clip != null)
            {
                audioSource.Stop();
                audioSource.clip.UnloadAudioData();
            }
            AudioClip clip = null;
            clip = DownloadHandlerAudioClip.GetContent(uwr);
            if (!clip.LoadAudioData())
            {
                Log.Error("加载失败");
                StartCoroutine(RequestMusicUrl());
                yield break;
            }
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void ChangeStyle()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (GlobalData.uStyle == UStyle.White)
        {
            textName.DOColor(GlobalData.blackColor, 0.5f);
            textSinger.DOColor(GlobalData.blackColor, 0.5f);
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            textName.DOColor(Color.white, 0.5f);
            textSinger.DOColor(Color.white, 0.5f);
        }
    }

    public class ApiData
    {
        public int code;
        public string name;
        public string coverImgUrl;
        public string tags;
        public Data data = new Data();
    }

    public class Data
    {
        public string name;
        public string alname;
        public int id;
        public string url;
        public string picurl;
        public string artistsname;
    }
}