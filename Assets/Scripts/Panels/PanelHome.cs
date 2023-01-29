using DG.Tweening;
using TMPro;
using UnityEngine;

public class PanelHome : MonoBehaviour
{
    public Transform taichi;
    Vector3 RotationSpeed = Vector3.forward * -135f;
    public TextMeshProUGUI text;

    private void Start()
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
        if (Data.uStyle == UStyle.White)
        {
            text.DOColor(Data.blackColor, 0.5f);
        }
        if (Data.uStyle == UStyle.Black)
        {
            text.DOColor(Color.white, 0.5f);
        }
    }
}