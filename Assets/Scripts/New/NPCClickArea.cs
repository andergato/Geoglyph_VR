using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Invisible world-space UI button that sits over an NPC. The laser pointer clicks it like any
/// other UI button, and it starts that NPC's dialogue.
///
/// Put this on the Button object (Button + Image with alpha 0) inside a child World Space canvas
/// that also has UltimateXR's UxrCanvas component.
/// </summary>
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class NPCClickArea : MonoBehaviour
{
    [SerializeField] NPCDialogue npc;

    [Header("Sizing")]
    [Tooltip("If set, the button is resized and centered to fit all renderers under this object (e.g. the NPC's Model). Leave empty to size it by hand.")]
    [SerializeField] Transform fitToModel;
    [SerializeField] float padding = 0.1f;

    [Header("Behaviour")]
    [SerializeField] bool faceCamera = true;

    Button button;
    Image image;
    RectTransform rect;
    Canvas canvas;
    Camera cam;

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        rect = (RectTransform)transform;
        canvas = GetComponentInParent<Canvas>();

        if (!npc) npc = GetComponentInParent<NPCDialogue>();

        // Invisible, but still hit by the laser.
        image.color = new Color(1f, 1f, 1f, 0f);
        image.raycastTarget = true;

        button.transition = Selectable.Transition.None;
        button.onClick.AddListener(OnClicked);
    }

    void Start()
    {
        if (fitToModel) Fit();

        var manager = DialogueManager.Instance;
        if (manager)
        {
            manager.Started += HandleStarted;
            manager.Ended += HandleEnded;
        }
    }

    void OnDestroy()
    {
        var manager = DialogueManager.Instance;
        if (manager)
        {
            manager.Started -= HandleStarted;
            manager.Ended -= HandleEnded;
        }
    }

    void OnClicked()
    {
        if (npc) npc.Interact();
    }

    // While a conversation is open, stop blocking the laser so it can reach the dialogue arrow.
    void HandleStarted(NPCDialogue _) => image.raycastTarget = false;
    void HandleEnded() => image.raycastTarget = true;

    void LateUpdate()
    {
        if (!faceCamera || !canvas) return;
        if (!cam) cam = Camera.main;
        if (!cam) return;

        Vector3 dir = canvas.transform.position - cam.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
            canvas.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    /// <summary>Fits the button to the model's bounds. Call again if the model changes.</summary>
    [ContextMenu("Fit To Model")]
    public void Fit()
    {
        if (!fitToModel || !canvas) return;

        var renderers = fitToModel.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);

        // Canvas sits at the model's center; width uses the larger horizontal extent so it works from any side.
        canvas.transform.position = b.center;

        float worldWidth = Mathf.Max(b.size.x, b.size.z) + padding * 2f;
        float worldHeight = b.size.y + padding * 2f;

        Vector3 s = canvas.transform.lossyScale;
        if (s.x <= 0f || s.y <= 0f) return;

        var canvasRect = (RectTransform)canvas.transform;
        canvasRect.sizeDelta = new Vector2(worldWidth / s.x, worldHeight / s.y);

        // Button fills the canvas.
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
