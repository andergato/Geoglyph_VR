using UnityEngine;

/// <summary>
/// The "!" icon above an NPC's head. Put this on an empty parent object above the NPC
/// and put the actual icon (sprite/mesh/TMP text) on a child, assigned to 'visual'.
/// It bobs and turns toward the player's camera. NPCDialogue shows/hides it.
/// </summary>
public class NPCMarker : MonoBehaviour
{
    [SerializeField] GameObject visual;
    [SerializeField] float bobHeight = 0.05f;
    [SerializeField] float bobSpeed = 2f;
    [SerializeField] bool faceCamera = true;

    Vector3 baseLocalPos;
    Camera cam;

    void Awake()
    {
        baseLocalPos = transform.localPosition;
    }

    public void SetVisible(bool visible)
    {
        if (visual && visual.activeSelf != visible) visual.SetActive(visible);
    }

    void LateUpdate()
    {
        if (!visual || !visual.activeSelf) return;

        transform.localPosition = baseLocalPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);

        if (!faceCamera) return;
        if (!cam) cam = Camera.main;
        if (!cam) return;

        // Yaw-only billboard so the icon stays upright.
        Vector3 toCam = cam.transform.position - transform.position;
        toCam.y = 0f;
        if (toCam.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(-toCam);
    }
}
