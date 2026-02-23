using UnityEngine;

public class VerticalBillboard : MonoBehaviour
{
    [SerializeField] private Transform targetCamera;

    private void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector3 direction = transform.position - targetCamera.position;



        transform.rotation = Quaternion.LookRotation(direction);
    }
}