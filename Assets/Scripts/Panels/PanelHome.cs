using UnityEngine;

public class PanelHome : MonoBehaviour
{
    public Transform taichi;
    Vector3 RotationSpeed = Vector3.forward * -135f;

    private void Start()
    {
        taichi = transform.Find("taichi");
    }

    private void LateUpdate()
    {
        taichi.transform.Rotate(RotationSpeed * Time.deltaTime, Space.Self);
    } 
}