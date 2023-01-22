using FancyScrollView; 
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
    private StoryInfo storyInfo;

    public override void UpdateContent(StoryInfo storyInfo)
    {
        this.storyInfo = storyInfo;
        textName.text = storyInfo.Name;
        ImgLoader.Instance.DownLoad(image, storyInfo.ImgUrl);
    }

    protected override void UpdatePosition(float normalizedPosition, float localPosition)
    {
        base.UpdatePosition(normalizedPosition, localPosition);

        //var wave = Mathf.Sin(normalizedPosition * Mathf.PI * 2) * 65;
        //transform.localPosition += Vector3.right * wave;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(async () =>
        {
            string filePath = Application.streamingAssetsPath + "/Story/" + storyInfo.Name + ".txt";
            using (var web = new WebClient())
            {
                await web.DownloadFileTaskAsync(storyInfo.DownUrl, filePath);
                Debug.LogWarning(storyInfo.Name+" 下载完成");
            }
        });
    }
}