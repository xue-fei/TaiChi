using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PanelMusic : MonoBehaviour
{
    public Image background;
    public Image image;
    public UButton buttonRefresh;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textSinger;
    /// <summary>
    /// https://api.aa1.cn/doc/wyy_music.html
    /// </summary>
    string apiUrl = "https://free.wqwlkj.cn/wqwlapi/wyy_random.php?type=json";
    string infoUrl = "http://music.163.com/api/song/detail/?id={0}&ids=%5B{1}%5D";
    string lyricUrl = "https://music.163.com/api/song/lyric?id={0}&lv=1&kv=1&tv=-1";
    string musicPath;
    AudioSource audioSource;
    float pauseTime;

    Vector3 RotationSpeed = Vector3.forward * -25f;
    public Camera camera3d;
    /// <summary>
    /// 存放频谱数据的数组长度
    /// </summary>
    float[] samples = new float[128];
    /// <summary>
    /// 画线
    /// </summary>
    public LineRenderer linerenderer;
    /// <summary>
    /// cube预制体
    /// </summary>
    public GameObject cube;
    /// <summary>
    /// cube预制体的位置
    /// </summary>
    Transform[] cubeTransform;
    /// <summary>
    /// 中间位置，用以对比cube位置与此帧的频谱数据
    /// </summary>
    Vector3 cubePos;

    public Text textLyric;
    Lyric mLyric = new Lyric();

    private void Awake()
    {
        musicPath = @Application.persistentDataPath + "/Music";
        if (!Directory.Exists(musicPath))
        {
            Directory.CreateDirectory(musicPath);
        }
        background = transform.parent.Find("Background").GetComponent<Image>();
        image = transform.Find("Image").GetComponent<Image>();
        buttonRefresh = transform.Find("ButtonRefresh").GetComponent<UButton>();
        textName = transform.Find("TextName").GetComponent<TextMeshProUGUI>();
        textSinger = transform.Find("TextSinger").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();

        linerenderer.positionCount = samples.Length;//设定线段的片段数量
        cubeTransform = new Transform[samples.Length];//设定数组长度
        //将脚本所挂载的gameobject向左移动，使得生成的物体中心正对摄像机
        camera3d.transform.position = new Vector3(samples.Length * 0.5f, 47, 0);
        GameObject tempCube;

        //生成cube，将其位置信息传入cubeTransform数组，并将其设置为脚本所挂载的gameobject的子物体
        for (int i = 0; i < samples.Length; i++)
        {
            tempCube = Instantiate(cube, new Vector3(linerenderer.transform.position.x + i,
                linerenderer.transform.position.y,
                linerenderer.transform.position.z), Quaternion.identity);
            cubeTransform[i] = tempCube.transform;
            cubeTransform[i].parent = linerenderer.transform;
            tempCube.name = i.ToString();
        }
        textLyric = transform.Find("TextLyric").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonRefresh.onClick.AddListener(() => StartCoroutine(RequestMusicUrl()));
        StartCoroutine(RequestMusicUrl());
    }

    void Update()
    {
        image.transform.Rotate(RotationSpeed * Time.deltaTime, Space.Self);
        if (audioSource.clip == null)
        {
            return;
        }
        //获取频谱
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        //循环
        for (int i = 0; i < samples.Length; i++)
        {
            //根据频谱数据设置中间位置的的y的值，根据对应的cubeTransform的位置设置x、z的值
            //使用Mathf.Clamp将中间位置的的y限制在一定范围，避免过大
            //频谱时越向后越小的，为避免后面的数据变化不明显，故在扩大samples[i]时，乘以50+i * i*0.5f
            cubePos.Set(cubeTransform[i].position.x, Mathf.Clamp(samples[i] * (50 + i * i * 0.5f), 0, 100), cubeTransform[i].position.z);
            //画线，为使线不会与cube重合，故高度减一
            linerenderer.SetPosition(i, cubePos - Vector3.up);
            //当cube的y值小于中间位置cubePos的y值时，cube的位置变为cubePos的位置
            if (cubeTransform[i].position.y < cubePos.y)
            {
                cubeTransform[i].position = cubePos;
            }
            //当cube的y值大于中间位置cubePos的y值时，cube的位置慢慢向下降落
            else if (cubeTransform[i].position.y > cubePos.y)
            {
                cubeTransform[i].position -= new Vector3(0, 0.5f, 0);
            }
        }
        UpdateLyric();
        if (audioSource.time > audioSource.clip.length - 0.05f)
        {
            StartCoroutine(RequestMusicUrl());
        }
    }

    string text;
    void UpdateLyric()
    {
        // test code
        // get current music play timestamp
        Int64 timestamp = GetCurrentTimestamp();
        // search current lyric
        LyricItem currentItem = mLyric.SearchCurrentItem(timestamp);
        text = "";
        if (null != textLyric)
        {
            // show lyrics from index (currentItem.mIndex - showLyricSize) to (currentItem.mIndex + showLyricSize)
            List<LyricItem> items = mLyric.GetItems();
            int showLyricSize = 3;
            foreach (LyricItem item in items)
            {
                if (item == currentItem)
                {
                    // if current lyric, highlight text with color (R, G, B)
                    text += Lyric.WrapStringWithColorTag(item.mText, 255, 0, 0) + System.Environment.NewLine;
                }
                else if ((null == currentItem && item.mIndex < showLyricSize)
                    || (null != currentItem && item.mIndex >= currentItem.mIndex - showLyricSize
                    && item.mIndex <= currentItem.mIndex + showLyricSize))
                {
                    if (GlobalData.uStyle == UStyle.Black)
                    {
                        text += Lyric.WrapStringWithColorTag(item.mText, 255, 255, 255) + System.Environment.NewLine;
                    }
                    else
                    {
                        text += Lyric.WrapStringWithColorTag(item.mText, (int)(GlobalData.blackColor.r * 255)
                            , (int)(GlobalData.blackColor.g * 255), (int)(GlobalData.blackColor.b * 255)) + System.Environment.NewLine;
                    }
                }
            }
            textLyric.text = text;
        }
    }

    Int64 GetCurrentTimestamp()
    {
        return (Int64)(audioSource.time * 1000.0f);
    }

    private void OnEnable()
    {
        Log.Warning("OnEnable");
        background.gameObject.SetActive(false);
        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.time = pauseTime;
        }
        camera3d.gameObject.SetActive(true);
        ChangeStyle(0f);
    }

    private void OnDisable()
    {
        Log.Warning("OnDisable");
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            pauseTime = audioSource.time;
        }
        camera3d.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
    }

    string invalidChars = @"[\\/:*?""<>|]";
    private IEnumerator RequestMusicUrl()
    {
        pauseTime = 0;
        SongData songData;
        UnityWebRequest uwr0 = UnityWebRequest.Get(apiUrl);
        uwr0.certificateHandler = new WebReqSkipCert();
        yield return uwr0.SendWebRequest();
        if (uwr0.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("请求失败 " + uwr0.error);
            yield break;
        }
        else
        {
            try
            {
                Log.Warning(uwr0.downloadHandler.text);
                songData = JsonConvert.DeserializeObject<SongData>(uwr0.downloadHandler.text);
                songData.name = Regex.Replace(songData.name, invalidChars, "");
                if (songData.code != 1)
                {
                    Log.Error("apiData.code:" + songData.code);
                    yield break;
                }
                textName.text = songData.data.name;
                textSinger.text = "";
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                yield break;
            }
        }
        string singer;
        SongInfo songInfo = null;
        UnityWebRequest uwr1 = UnityWebRequest.Get(string.Format(infoUrl, songData.data.id, songData.data.id));
        uwr1.certificateHandler = new WebReqSkipCert();
        yield return uwr1.SendWebRequest();
        if (uwr1.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("请求失败 " + uwr1.error);
            yield break;
        }
        else
        {
            try
            {
                Log.Warning(uwr1.downloadHandler.text);
                songInfo = JsonConvert.DeserializeObject<SongInfo>(uwr1.downloadHandler.text);
                if (songInfo.code != 200)
                {
                    Log.Error("songInfo.code:" + songInfo.code);
                    yield break;
                }
                singer = songInfo.songs[0].artists[0].name;
                textSinger.text = singer;
                string musicUrl = songData.data.url;
                string coverName = singer + "-" + songData.data.name + ".jpg";
                ImgLoader.Instance.DownLoad(image, songData.data.picurl, musicPath, coverName);
                string musicFilePath = musicPath + "/" + singer + "-" + songData.data.name + ".mp3";

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
                            await web.DownloadFileTaskAsync(songData.data.url, musicFilePath);
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

        LyricData lyricData;
        UnityWebRequest uwr2 = UnityWebRequest.Get(string.Format(lyricUrl, songData.data.id));
        uwr2.certificateHandler = new WebReqSkipCert();
        yield return uwr2.SendWebRequest();
        if (uwr2.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("请求失败 " + uwr2.error);
            yield break;
        }
        else
        {
            try
            {
                Log.Warning(uwr2.downloadHandler.text);
                lyricData = JsonConvert.DeserializeObject<LyricData>(uwr2.downloadHandler.text);
                if (lyricData.code != 200)
                {
                    Log.Error("lyricData.code:" + lyricData.code);
                    yield break;
                }
                string lrcFilePath = musicPath + "/" + singer + "-" + songData.data.name + ".lrc";
                File.WriteAllText(lrcFilePath, lyricData.lrc.lyric);
                mLyric.Load(lrcFilePath);
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
            uwr.certificateHandler = new WebReqSkipCert();
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
            image.transform.localRotation = Quaternion.identity;
        }
    }

    public void ChangeStyle(float time)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (GlobalData.uStyle == UStyle.White)
        {
            textName.DOColor(GlobalData.blackColor, time);
            textSinger.DOColor(GlobalData.blackColor, time);
            camera3d.DOColor(Color.white, time);
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            textName.DOColor(Color.white, time);
            textSinger.DOColor(Color.white, time);
            camera3d.DOColor(GlobalData.blackColor, time);
        }
        buttonRefresh.ChangeStyle(time);
    }

    public class SongData
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
        public long id;
        public string url;
        public string picurl;
        public string artistsname;
    }

    public class SongInfo
    {
        public int code;
        public List<Song> songs = new List<Song>();
    }

    public class Song
    {
        public string name;
        public long id;
        public List<Artist> artists = new List<Artist>();
    }

    public class Artist
    {
        public string name;
        public int id;
    }

    public class LyricData
    {
        public int code;
        public Lrc lrc = new Lrc();
    }

    public class Lrc
    {
        public int version;
        public string lyric;
    }
}