using DG.Tweening;
using TMPro;
using UnityEngine;

public class PanelHome : MonoBehaviour
{
    public Transform taichi;
    Vector3 RotationSpeed = Vector3.forward * -135f;
    public TextMeshProUGUI text;

    private void Awake()
    {
        taichi = transform.Find("taichi");
        text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
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
        }
        if (GlobalData.uStyle == UStyle.Black)
        {
            text.DOColor(Color.white, 0.5f);
        }
    }
}