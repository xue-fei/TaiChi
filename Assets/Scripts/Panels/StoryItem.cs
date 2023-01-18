using EnhancedUI.EnhancedScroller; 
using TMPro; 
using UnityEngine.UI;

public class StoryItem : EnhancedScrollerCellView
{
    public Image image; 
    public TextMeshProUGUI textTitle;
    public Button button;

    public void SetData(StoryInfo storyInfo)
    {
        textTitle.text = storyInfo.Name;
        ImgLoader.Instance.DownLoad(image, storyInfo.ImgUrl);
    }
}