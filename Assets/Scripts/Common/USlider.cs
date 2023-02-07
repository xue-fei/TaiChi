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
        ChangeStyle(GlobalData.time);
        onClick?.Invoke();
    }

    public void ChangeStyle(float time)
    {
        if (GlobalData.uStyle == UStyle.White)
        {
            this.DOValue(1f, time);
            text.text = "黑暗";
            text.DOColor(Color.white, time);
            GlobalData.uStyle = UStyle.Black;
            return;
        }
        else
        {
            this.DOValue(0f, time);
            text.text = "光明";
            text.DOColor(Color.black, time);
            GlobalData.uStyle = UStyle.White;
        }
    }
}