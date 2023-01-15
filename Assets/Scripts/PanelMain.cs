using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMain : MonoBehaviour
{
    public Camera uCamera;
    public USlider uSlider;
    public UToggle[] uToggles;

    // Start is called before the first frame update
    void Start()
    {
        uSlider.onClick += OnClickSlider;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClickSlider()
    {
        foreach (UToggle uToggle in uToggles)
        {
            uToggle.ChangeStyle();
        }
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
