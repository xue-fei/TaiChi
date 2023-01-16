using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class UToggle : Toggle
{
    public SVGImage background;
    public SVGImage checkmark;
    public TextMeshProUGUI text;


    protected override void Awake()
    {
        background = transform.Find("Background").GetComponent<SVGImage>();
        checkmark = background.transform.Find("Checkmark").GetComponent<SVGImage>();
        text = background.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

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
        }
        else
        {
            if (Data.uStyle == UStyle.White)
            {
                background.color = Color.black;
                checkmark.color = Color.black;
                text.color = Color.black;
            }
            if (Data.uStyle == UStyle.Black)
            {
                background.color = Color.white;
                checkmark.color = Color.white;
                text.color = Color.white;
            }
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
            if (Data.uStyle == UStyle.White)
            {
                background.color = Color.black;
                checkmark.color = Color.black;
                text.color = Color.black;
            }
            if (Data.uStyle == UStyle.Black)
            {
                background.color = Color.white;
                checkmark.color = Color.white;
                text.color = Color.white;
            }
        }
    }
}