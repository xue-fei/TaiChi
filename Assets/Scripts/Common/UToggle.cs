using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UToggle : Toggle, IChangeStyle
{
    public SVGImage background;
    public SVGImage checkmark;
    public SVGImage svgImage;
    public TextMeshProUGUI text;


    protected override void Awake()
    {
        background = transform.Find("Background").GetComponent<SVGImage>();
        checkmark = background.transform.Find("Checkmark").GetComponent<SVGImage>();
        text = background.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        svgImage = transform.Find("SVG Image").GetComponent<SVGImage>();
        svgImage.color = GlobalData.checkedColor;
        svgImage.gameObject.SetActive(false);

        background.color = Color.white;
        checkmark.color = Color.white;
        text.color = Color.white;

        onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(bool value)
    {
        if (value)
        {
            background.color = GlobalData.checkedColor;
            checkmark.color = GlobalData.checkedColor;
            text.color = GlobalData.checkedColor;
            svgImage.gameObject.SetActive(true);
        }
        else
        {
            ChangeColor(GlobalData.time);
            svgImage.gameObject.SetActive(false);
        }
    }

    private void ChangeColor(float time)
    {
        if (GlobalData.uStyle == UStyle.White)
        {
            background.DOColor(GlobalData.blackColor, time);
            checkmark.DOColor(GlobalData.blackColor, time);
            text.DOColor(GlobalData.blackColor, time);
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            background.DOColor(Color.white, time);
            checkmark.DOColor(Color.white, time);
            text.DOColor(Color.white, time);
        }
    }

    public void ChangeStyle(float time)
    {
        if (isOn)
        {
            return;
        }
        else
        {
            ChangeColor(time);
        }
    }
}