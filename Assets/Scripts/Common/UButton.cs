using DG.Tweening;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class UButton : Button, IChangeStyle
{
    public SVGImage svgImage;

    protected override void Awake()
    {
        svgImage = transform.GetComponent<SVGImage>();
        svgImage.color = Color.white;
    }

    public void ChangeStyle(float time)
    {
        if (GlobalData.uStyle == UStyle.White)
        {
            svgImage.DOColor(GlobalData.blackColor, time);
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            svgImage.DOColor(Color.white, time);
        }
    }
}