using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public UToggle toggleHome;
    public UToggle togglePicture;
    public UToggle toggleMusic;
    public UToggle toggleVideo;
    public UToggle toggleStory;

    public USlider uSlider;
    public Image background;

    // Start is called before the first frame update
    void Start()
    {
        Transform toggles = transform.Find("Toggles");
        toggleHome = toggles.Find("ToggleHome").GetComponent<UToggle>();
        togglePicture = toggles.Find("TogglePicture").GetComponent<UToggle>();
        toggleMusic = toggles.Find("ToggleMusic").GetComponent<UToggle>();
        toggleVideo = toggles.Find("ToggleVideo").GetComponent<UToggle>();
        toggleStory = toggles.Find("ToggleStory").GetComponent<UToggle>();

        toggleHome.isOn = true;
        background = transform.Find("Background").GetComponent<Image>();
        uSlider = transform.Find("Slider").GetComponent<USlider>();
        uSlider.value = 1f;
        uSlider.onClick += OnClickSlider;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClickSlider()
    {
        toggleHome.ChangeStyle();
        togglePicture.ChangeStyle();
        toggleMusic.ChangeStyle();
        toggleVideo.ChangeStyle();
        toggleStory.ChangeStyle();

        if (Data.uStyle == UStyle.Black)
        {
            background.DOColor(Color.black, 0.5f);
        }
        if (Data.uStyle == UStyle.White)
        {
            background.DOColor(Color.white, 0.5f);
        }
    }
}