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
    public UButton buttonRefresh;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textSinger;
    /// <summary>
    /// https://api.aa1.cn/doc/wyy_music.html
    /// </summary>
    string apiUrl = "https://api.wqwlkj.cn/wqwlapi/wyy_random.php?type=json";
    string lyricUrl = "https://music.163.com/api/song/lyric?id={歌曲ID}&lv=1&kv=1&tv=-1";
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

    private void Awake()
    {
        musicPath = Application.persistentDataPath + "/Music";
        if (!Directory.Exists(musicPath))
        {
            Directory.CreateDirectory(musicPath);
        }
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
    }
     
    private void OnEnable()
    {
        Log.Warning("OnEnable");
        if (audioSource.clip != null)
        {
            audioSource.Play();
            audioSource.time = pauseTime;
        }
        camera3d.gameObject?.SetActive(true);
    }

    private void OnDisable()
    {
        Log.Warning("OnDisable");
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            pauseTime = audioSource.time;
        }
        camera3d.gameObject?.SetActive(false);
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
                SongData apiData = JsonConvert.DeserializeObject<SongData>(uwr.downloadHandler.text);
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
            camera3d.DOColor(Color.white, 0.5f);
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            textName.DOColor(Color.white, 0.5f);
            textSinger.DOColor(Color.white, 0.5f);
            camera3d.DOColor(GlobalData.blackColor, 0.5f);
        }
        buttonRefresh.ChangeStyle();
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
        public int id;
        public string url;
        public string picurl;
        public string artistsname;
    }
}