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

    private void ChangeColor()
    {
        if (Data.uStyle == UStyle.White)
        {
            svgImage.DOColor(Data.blackColor, 0.5f); 
        }
        if (Data.uStyle == UStyle.Black)
        {
            svgImage.DOColor(Color.white, 0.5f); 
        }
    }
     
    public void ChangeStyle()
    {
        ChangeColor();
    }
}