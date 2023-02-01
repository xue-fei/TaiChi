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

    public PanelHome panelHome;
    public PanelPicture panelPicture;
    public PanelMusic panelMusic;
    public GameObject panelVideo;
    public GameObject panelStory;

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
        toggleHome.onValueChanged.AddListener((value) => OnToggle(value, toggleHome));
        togglePicture.onValueChanged.AddListener((value) => OnToggle(value, togglePicture));
        toggleMusic.onValueChanged.AddListener((value) => OnToggle(value, toggleMusic));
        toggleVideo.onValueChanged.AddListener((value) => OnToggle(value, toggleVideo));
        toggleStory.onValueChanged.AddListener((value) => OnToggle(value, toggleStory));

        background = transform.parent.Find("Background").GetComponent<Image>();
        uSlider = transform.Find("Slider").GetComponent<USlider>();
        uSlider.value = 1f;
        uSlider.onClick += OnClickSlider;

        panelHome = transform.parent.Find("PanelHome").GetComponent<PanelHome>();
        panelPicture = transform.parent.Find("PanelPicture").GetComponent<PanelPicture>();
        panelMusic = transform.parent.Find("PanelMusic").GetComponent<PanelMusic>();
        panelVideo = transform.parent.Find("PanelVideo").gameObject;
        panelStory = transform.parent.Find("PanelStory").gameObject;
    }

    void OnClickSlider()
    {
        toggleHome.ChangeStyle();
        togglePicture.ChangeStyle();
        toggleMusic.ChangeStyle();
        toggleVideo.ChangeStyle();
        toggleStory.ChangeStyle();

        panelHome.ChangeStyle();
        panelMusic.ChangeStyle();
        panelPicture.ChangeStyle();

        if (GlobalData.uStyle == UStyle.Black)
        {
            background.DOColor(GlobalData.blackColor, 0.5f);
        }
        if (GlobalData.uStyle == UStyle.White)
        {
            background.DOColor(Color.white, 0.5f);
        }
    }

    void OnToggle(bool value, UToggle uToggle)
    {
        if (value)
        {
            panelHome.gameObject.SetActive(false);
            panelPicture.gameObject.SetActive(false);
            panelMusic.gameObject.SetActive(false);
            panelVideo.SetActive(false);
            panelStory.SetActive(false);
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
                panelVideo.SetActive(true);
            }
            if (uToggle.name == toggleStory.name)
            {
                panelStory.SetActive(true);
            }
        }
    }
}