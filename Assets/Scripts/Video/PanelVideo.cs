using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PanelVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += OnPlayEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.texture == null)
        { 
            return; 
        } 
        rawImage.texture = videoPlayer.texture;
    }

    void OnPlayEnd(VideoPlayer videoPlayer)
    {
        Debug.LogWarning("OnPlayEnd");
        videoPlayer.Stop();
        videoPlayer.Play();
    }

    private void OnEnable()
    {
        rawImage.DOColor(Color.white, 0.5f);
    }

    private void OnDisable()
    {
        rawImage.DOColor(Color.black, 0.1f);
    }
}