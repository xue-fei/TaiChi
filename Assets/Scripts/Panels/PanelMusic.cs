using System;
using System.Collections;
using System.Collections.Generic;
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
    string apiUrl = "https://api.uomg.com/api/rand.music?sort=%E7%83%AD%E6%AD%8C%E6%A6%9C&format=json";
    string musicPath;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        ApiData apiData = new ApiData();
        apiData.code = 1;
        apiData.data.name = "aaaa";
        apiData.data.url = "bbbb";
        apiData.data.picurl = "cccc";
        apiData.data.artistsname = "dddd";

      string json =  JsonUtility.ToJson(apiData);
        Log.Warning(json);

        musicPath = Application.persistentDataPath + "/Music";
        if (!Directory.Exists(musicPath))
        {
            Directory.CreateDirectory(musicPath);
        }
        image = transform.Find("Image").GetComponent<Image>();
        buttonRefresh = transform.Find("ButtonRefresh").GetComponent<Button>();
        textName = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(RequestMusicUrl());
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
                ApiData apiData = new ApiData();
                JsonUtility.FromJsonOverwrite(uwr.downloadHandler.text, apiData);
                Log.Warning(uwr.downloadHandler.text);
                musicUrl = apiData.data.url;
                Log.Warning(apiData.data.url);
                Log.Warning(apiData.data.name);
                Log.Warning(apiData.data.picurl);
                ImgLoader.Instance.DownLoad(image, apiData.data.picurl, musicPath, apiData.data.name + ".jpg");
                Loom.RunAsync(async () =>
                {
                    using (var web = new WebClient())
                    {
                        //web.DownloadProgressChanged += (s, e) =>
                        //{
                        //    Debug.LogWarning(e.ProgressPercentage + "%");
                        //};
                        await web.DownloadFileTaskAsync(apiData.data.url, musicPath + "/" + apiData.data.name + ".mp3");
                        Loom.QueueOnMainThread(() =>
                        {
                            StartCoroutine(PlayMusic(musicPath + "/" + apiData.data.name + ".mp3"));
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
            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public class ApiData
    {
        public int code;
        public Data data = new Data();
    }

    public class Data
    {
        public string name;
        public string url;
        public string picurl;
        public string artistsname;
    }
}