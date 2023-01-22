using FancyScrollView;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
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
        ImgLoader.Instance.DownLoad(image, storyInfo.ImgUrl);
        string filePath = Application.streamingAssetsPath + "/Story/" + storyInfo.Name + ".txt";
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
                       web.DownloadProgressChanged += (s,e) =>
                       {
                           Debug.LogWarning(e.ProgressPercentage+"%");
                       };
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
}