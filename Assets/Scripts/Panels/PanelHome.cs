using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelHome : MonoBehaviour
{
    public Transform taichi;
    Vector3 RotationSpeed = Vector3.forward * -135f;
    public TextMeshProUGUI text;
    public Image background;

    private void Awake()
    {
        taichi = transform.Find("taichi");
        text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        background = transform.Find("Background").GetComponent<Image>();
    }

    private void LateUpdate()
    {
        taichi.transform.Rotate(RotationSpeed * Time.deltaTime, Space.Self);
    }

    public void ChangeStyle()
    {
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        if (GlobalData.uStyle == UStyle.White)
        {
            text.DOColor(GlobalData.blackColor, 0.5f);
            background.DOColor(Color.white, 0.5f);
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            text.DOColor(Color.white, 0.5f);
            background.DOColor(GlobalData.blackColor, 0.5f);
        }
    }
}