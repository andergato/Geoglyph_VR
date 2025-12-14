using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        // Try main camera first
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            // Fallback if no main camera tag
            Camera[] cams = FindObjectsOfType<Camera>();
            if (cams.Length > 0)
                cam = cams[0].transform;
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // Make the object face the camera but keep it upright (no tilting)
        Vector3 lookPos = cam.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
    }
}
