using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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
            this.DOValue(1f, 0.5f); 
            text.text = "黑暗";
            text.DOColor(Color.white, 0.5f); 
            Data.uStyle = UStyle.Black;
            onClick?.Invoke();
            Debug.LogWarning("黑暗");
            return;
        }
        else
        {
            this.DOValue(0f, 0.5f);
            text.text = "光明";
            text.DOColor(Color.black, 0.5f);
            Data.uStyle = UStyle.White; 
            Debug.LogWarning("光明");
            onClick?.Invoke();
        }
    }
}