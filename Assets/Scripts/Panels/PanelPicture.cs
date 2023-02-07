using DG.Tweening;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PanelPicture : MonoBehaviour
{
    public Image background;
    public Image image;
    string beautyUrl = "https://v.api.aa1.cn/api/pc-girl_bz/index.php?wpon=ro38d57y8rhuwur3788y3rd";
    public UButton buttonRefresh;

    private void Awake()
    {
        background = transform.Find("Background").GetComponent<Image>();
        image = transform.Find("Image").GetComponent<Image>();
        buttonRefresh = transform.Find("ButtonRefresh").GetComponent<UButton>();
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonRefresh.onClick.AddListener(() => StartCoroutine(RequestBeauty()));
        StartCoroutine(RequestBeauty());
    }

    private IEnumerator RequestBeauty()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(beautyUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
                uwr.Dispose();
                StartCoroutine(RequestBeauty());
                yield break;
            }
            else
            {
                image.DOColor(Color.black, 0.2f);
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                string filePath = Application.streamingAssetsPath + "/Picture/"
                    + DateTime.Now.ToFileTime() + ".jpg";
                File.WriteAllBytesAsync(filePath, uwr.downloadHandler.data);
                if (image != null)
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                image.DOColor(Color.white, 0.2f);
            }
        }
    }

    public void ChangeStyle()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        buttonRefresh.ChangeStyle();
        if (GlobalData.uStyle == UStyle.White)
        { 
            background.DOColor(Color.white, 0.5f);
        }
        if (GlobalData.uStyle == UStyle.Black)
        { 
            background.DOColor(GlobalData.blackColor, 0.5f);
        }
    }
}