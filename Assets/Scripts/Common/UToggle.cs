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
        svgImage.color = Data.checkedColor;
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
            background.color = Data.checkedColor;
            checkmark.color = Data.checkedColor;
            text.color = Data.checkedColor;
            svgImage.gameObject.SetActive(true);
        }
        else
        {
            ChangeColor();
            svgImage.gameObject.SetActive(false);
        }
    }

    private void ChangeColor()
    {
        if (Data.uStyle == UStyle.White)
        {
            background.DOColor(Data.blackColor, 0.5f);
            checkmark.DOColor(Data.blackColor, 0.5f);
            text.DOColor(Data.blackColor, 0.5f);
        }
        if (Data.uStyle == UStyle.Black)
        {
            background.DOColor(Color.white, 0.5f);
            checkmark.DOColor(Color.white, 0.5f);
            text.DOColor(Color.white, 0.5f);
        }
    }

    public void ChangeStyle()
    {
        if (isOn)
        {
            return;
        }
        else
        {
            ChangeColor();
        }
    }
}