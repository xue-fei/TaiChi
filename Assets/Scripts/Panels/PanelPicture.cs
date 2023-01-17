using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PanelPicture : MonoBehaviour
{
    public RawImage rawImage;
    string beautyUrl = "https://v.api.aa1.cn/api/pc-girl_bz/index.php?wpon=ro38d57y8rhuwur3788y3rd";
    public Button buttonRefresh;

    // Start is called before the first frame update
    void Start()
    {
        rawImage = transform.Find("RawImage").GetComponent<RawImage>();
        buttonRefresh = transform.Find("ButtonRefresh").GetComponent<Button>();
        buttonRefresh.onClick.AddListener(() => StartCoroutine(RequestBeauty()));
        StartCoroutine(RequestBeauty()); 
    }
     
    private IEnumerator RequestBeauty()
    {
        rawImage.DOColor(Color.black, 0.2f);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(beautyUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            { 
                uwr.Dispose();
                StartCoroutine(RequestBeauty());
                yield break;
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                rawImage.texture = texture;
                rawImage.DOColor(Color.white, 0.2f);
            }
        }
    }
}