using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class USlider : Slider
{
    public SVGImage background;
    public SVGImage fill;
    public SVGImage handle;
    public TextMeshProUGUI text;
    public UnityAction onClick;
    [SerializeField]


    protected override void Awake()
    {
        text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        text.text = "黑暗";
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogWarning("嘿嘿嘿");
        if (Data.uStyle == UStyle.White)
        {
            value = 1f;
            text.color = Color.white;
            Data.uStyle = UStyle.Black;
            text.text = "黑暗";
            onClick?.Invoke();
            Debug.LogWarning("黑暗");
            return;
        }
        else
        {
            value = 0f;
            text.color = Color.black;
            Data.uStyle = UStyle.White;
            text.text = "光明";
            Debug.LogWarning("光明");
            onClick?.Invoke();
        }
    }
}