using UnityEngine;

public class Menu : MonoBehaviour
{
    public UToggle toggleHome;
    public UToggle togglePicture;
    public UToggle toggleMusic;
    public UToggle toggleVideo;
    public UToggle toggleStory;

    public USlider uSlider;
    public Camera uCamera;

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

        uCamera = Camera.main;
        uSlider = transform.Find("Slider").GetComponent<USlider>();
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
            uCamera.backgroundColor = Color.black;
        }
        if (Data.uStyle == UStyle.White)
        {
            uCamera.backgroundColor = Color.white;
        }
    }
}