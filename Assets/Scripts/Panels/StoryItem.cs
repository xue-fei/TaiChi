using FancyScrollView;
using System.Collections;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StoryItem : FancyScrollRectCell<StoryInfo, StoryContext>
{
    public Image image;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textBrief;
    public Button button;
    public TextMeshProUGUI textButton;

    public override void UpdateContent(StoryInfo storyInfo)
    {
        textName.text = storyInfo.Name;
        Driver.Instance.StartCoroutine(LoadTexture(image, storyInfo));
        string filePath = Application.persistentDataPath + "/Story/" + storyInfo.Name + ".txt";
        button.onClick.RemoveAllListeners();
        if (!File.Exists(filePath))
        {
            textButton.text = "下载阅读";
            button.onClick.AddListener(() =>
            {
                Loom.RunAsync(async () =>
                {
                    using (var web = new WebClient())
                    {
                        //web.DownloadProgressChanged += (s, e) =>
                        //{
                        //    Debug.LogWarning(e.ProgressPercentage + "%");
                        //};
                        await web.DownloadFileTaskAsync(storyInfo.DownUrl, filePath);
                        Loom.QueueOnMainThread(() =>
                        {
                            textButton.text = "阅读";
                        });
                        Debug.LogWarning(storyInfo.Name + " 下载完成");
                    }
                });
            });
        }
        else
        {
            textButton.text = "阅读";
            //继续
        }
    }

    protected override void UpdatePosition(float normalizedPosition, float localPosition)
    {
        base.UpdatePosition(normalizedPosition, localPosition);
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private IEnumerator LoadTexture(Image image, StoryInfo storyInfo)
    {
        string filePath = Application.persistentDataPath + "/Story/" + storyInfo.Name + ".jpg";
        bool ready = false;
        if (File.Exists(filePath))
        {
            storyInfo.ImgUrl = "file://" + filePath;
            ready = true;
        }

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(storyInfo.ImgUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("网络错误");
                uwr.Dispose();
                yield break;
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                if (!ready)
                {
                    File.WriteAllBytesAsync(filePath, uwr.downloadHandler.data);
                }
                if (image != null)
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
    }
}