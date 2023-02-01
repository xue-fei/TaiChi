using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class USlider : Slider, IChangeStyle
{
    public TextMeshProUGUI text;
    public UnityAction onClick;

    protected override void Awake()
    {
        text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        text.text = "黑暗";
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ChangeStyle();
        onClick?.Invoke();
    }

    public void ChangeStyle()
    {
        if (GlobalData.uStyle == UStyle.White)
        {
            this.DOValue(1f, 0.5f);
            text.text = "黑暗";
            text.DOColor(Color.white, 0.5f);
            GlobalData.uStyle = UStyle.Black;
            return;
        }
        else
        {
            this.DOValue(0f, 0.5f);
            text.text = "光明";
            text.DOColor(Color.black, 0.5f);
            GlobalData.uStyle = UStyle.White;
        }
    }
}