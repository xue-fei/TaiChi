using FancyScrollView;
using TMPro; 
using UnityEngine.UI;

public class StoryItem : FancyScrollRectCell<StoryInfo, StoryContext>
{
    public Image image; 
    public TextMeshProUGUI textTitle;
    public Button button;
     
    public override void UpdateContent(StoryInfo storyInfo)
    {
        textTitle.text = storyInfo.Name;
        ImgLoader.Instance.DownLoad(image, storyInfo.ImgUrl);
    }

    protected override void UpdatePosition(float normalizedPosition, float localPosition)
    {
        base.UpdatePosition(normalizedPosition, localPosition);

        //var wave = Mathf.Sin(normalizedPosition * Mathf.PI * 2) * 65;
        //transform.localPosition += Vector3.right * wave;
    }
}