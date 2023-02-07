using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Image background;
    public UToggle toggleHome;
    public UToggle togglePicture;
    public UToggle toggleMusic;
    public UToggle toggleVideo;
    public UToggle toggleStory;

    public USlider uSlider;

    public PanelHome panelHome;
    public PanelPicture panelPicture;
    public PanelMusic panelMusic;
    public PanelVideo panelVideo;
    public PanelStory panelStory;

    // Start is called before the first frame update
    void Start()
    {
        background = transform.parent.Find("Background").GetComponent<Image>();
        Transform toggles = transform.Find("Toggles");
        toggleHome = toggles.Find("ToggleHome").GetComponent<UToggle>();
        togglePicture = toggles.Find("TogglePicture").GetComponent<UToggle>();
        toggleMusic = toggles.Find("ToggleMusic").GetComponent<UToggle>();
        toggleVideo = toggles.Find("ToggleVideo").GetComponent<UToggle>();
        toggleStory = toggles.Find("ToggleStory").GetComponent<UToggle>();

        toggleHome.isOn = true;
        toggleHome.onValueChanged.AddListener((value) => OnToggle(value, toggleHome));
        togglePicture.onValueChanged.AddListener((value) => OnToggle(value, togglePicture));
        toggleMusic.onValueChanged.AddListener((value) => OnToggle(value, toggleMusic));
        toggleVideo.onValueChanged.AddListener((value) => OnToggle(value, toggleVideo));
        toggleStory.onValueChanged.AddListener((value) => OnToggle(value, toggleStory));

        uSlider = transform.Find("Slider").GetComponent<USlider>();
        uSlider.value = 1f;
        uSlider.onClick += OnClickSlider;

        panelHome = transform.parent.Find("PanelHome").GetComponent<PanelHome>();
        panelPicture = transform.parent.Find("PanelPicture").GetComponent<PanelPicture>();
        panelMusic = transform.parent.Find("PanelMusic").GetComponent<PanelMusic>();
        panelVideo = transform.parent.Find("PanelVideo").GetComponent<PanelVideo>();
        panelStory = transform.parent.Find("PanelStory").GetComponent<PanelStory>();
    }

    void OnClickSlider()
    {
        toggleHome.ChangeStyle(GlobalData.time);
        togglePicture.ChangeStyle(GlobalData.time);
        toggleMusic.ChangeStyle(GlobalData.time);
        toggleVideo.ChangeStyle(GlobalData.time);
        toggleStory.ChangeStyle(GlobalData.time);

        panelHome.ChangeStyle(GlobalData.time);
        panelMusic.ChangeStyle(GlobalData.time);
        panelPicture.ChangeStyle(GlobalData.time);
        panelVideo.ChangeStyle(GlobalData.time);
        panelStory.ChangeStyle(GlobalData.time);

        if (GlobalData.uStyle == UStyle.Black)
        {
            background.DOColor(GlobalData.blackColor, GlobalData.time);
        }
        if (GlobalData.uStyle == UStyle.White)
        {
            background.DOColor(Color.white, GlobalData.time);
        }
    }

    void OnToggle(bool value, UToggle uToggle)
    {
        if (value)
        {
            panelHome.gameObject.SetActive(false);
            panelPicture.gameObject.SetActive(false);
            panelMusic.gameObject.SetActive(false);
            panelVideo.gameObject.SetActive(false);
            panelStory.gameObject.SetActive(false);
            if (uToggle.name == toggleHome.name)
            {
                panelHome.gameObject.SetActive(true);
            }
            if (uToggle.name == togglePicture.name)
            {
                panelPicture.gameObject.SetActive(true);
            }
            if (uToggle.name == toggleMusic.name)
            {
                panelMusic.gameObject.SetActive(true);
            }
            if (uToggle.name == toggleVideo.name)
            {
                panelVideo.gameObject.SetActive(true);
            }
            if (uToggle.name == toggleStory.name)
            {
                panelStory.gameObject.SetActive(true);
            }
        }
    }
}