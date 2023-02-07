using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UPanel : MonoBehaviour
{
    public Image background;

    public virtual void Awake()
    {
        background = transform.Find("Background").GetComponent<Image>(); 
    }

    public virtual void Init()
    {
        if (GlobalData.uStyle == UStyle.White)
        {
            background.DOColor(Color.white, 0f);
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            background.DOColor(GlobalData.blackColor, 0f);
        }
    }
    
    public virtual void ChangeStyle()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (GlobalData.uStyle == UStyle.White)
        {
            background.DOColor(Color.white, 0.5f);
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            background.DOColor(GlobalData.blackColor, 0.5f);
        }
    }
}