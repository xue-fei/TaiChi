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
    public GameObject panelPicture;
    public UButton buttonRefresh;
    public GameObject panelMusic;
    public GameObject panelVideo;
    public GameObject panelStory;

    // Start is called before the first frame update
    void Start()
    {
        Loom.Initialize();
        ImgLoader imgLoader = gameObject.AddComponent<ImgLoader>();
        imgLoader.Init();
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
        panelPicture = transform.parent.Find("PanelPicture").gameObject;
        buttonRefresh = panelPicture.transform.Find("ButtonRefresh").GetComponent<UButton>();
        panelMusic = transform.parent.Find("PanelMusic").gameObject;
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
        buttonRefresh.ChangeStyle();

        panelHome.ChangeStyle();

        if (Data.uStyle == UStyle.Black)
        {
            background.DOColor(Data.blackColor, 0.5f);
        }
        if (Data.uStyle == UStyle.White)
        {
            background.DOColor(Color.white, 0.5f);
        }
    }

    void OnToggle(bool value, UToggle uToggle)
    {
        if (value)
        {
            panelHome.gameObject.SetActive(false);
            panelPicture.SetActive(false);
            panelMusic.SetActive(false);
            panelVideo.SetActive(false);
            panelStory.SetActive(false);
            if (uToggle.name == toggleHome.name)
            {
                panelHome.gameObject.SetActive(true);
            }
            if (uToggle.name == togglePicture.name)
            {
                panelPicture.SetActive(true);
            }
            if (uToggle.name == toggleMusic.name)
            {
                panelMusic.SetActive(true);
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