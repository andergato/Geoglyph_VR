using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// World-space dialogue panel. Setup:
///  - Canvas set to World Space (scale ~0.001), with a child 'panel' holding the speaker text,
///    body text, and a next-arrow Button.
///  - Your VR ray must be able to click world-space UI (e.g. XR Interaction Toolkit's
///    TrackedDeviceGraphicRaycaster on the canvas), OR use DialogueArrow on a collider instead.
/// The panel moves to the speaking NPC's UIAnchor each conversation and faces the player.
/// </summary>
public class DialogueUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] DialogueManager manager;   // falls back to DialogueManager.Instance
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text speakerText;
    [SerializeField] TMP_Text bodyText;
    [SerializeField] Button nextButton;
    [SerializeField] Image arrowImage;           // optional, for swapping icon on the last line

    [Header("Arrow sprites (optional)")]
    [SerializeField] Sprite nextSprite;
    [SerializeField] Sprite closeSprite;

    [Header("Behaviour")]
    [Tooltip("0 = show each line instantly.")]
    [SerializeField] float charsPerSecond = 40f;
    [SerializeField] bool faceCamera = true;

    Camera cam;
    Coroutine typing;
    bool isTyping;
    int totalChars;

    void Start()
    {
        if (!manager) manager = DialogueManager.Instance;
        if (!manager)
        {
            Debug.LogError("DialogueUI: no DialogueManager found in scene.", this);
            enabled = false;
            return;
        }

        manager.Started += OnStarted;
        manager.LineShown += OnLineShown;
        manager.Ended += OnEnded;

        if (nextButton) nextButton.onClick.AddListener(OnNextPressed);
        panel.SetActive(false);
    }

    void OnDestroy()
    {
        if (!manager) return;
        manager.Started -= OnStarted;
        manager.LineShown -= OnLineShown;
        manager.Ended -= OnEnded;
    }

    void LateUpdate()
    {
        if (!faceCamera || !panel.activeSelf) return;
        if (!cam) cam = Camera.main;
        if (!cam) return;

        // World-space canvases face away from their forward, so look from camera -> panel.
        Vector3 dir = panel.transform.position - cam.transform.position;
        if (dir.sqrMagnitude > 0.0001f)
            panel.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    void OnStarted(NPCDialogue npc)
    {
        panel.transform.position = npc.UIAnchor.position;
        if (speakerText) speakerText.text = npc.SpeakerName;
        panel.SetActive(true);
    }

    void OnLineShown(string text, bool isLast)
    {
        if (arrowImage)
        {
            Sprite s = isLast ? closeSprite : nextSprite;
            if (s) arrowImage.sprite = s;
        }

        if (typing != null) StopCoroutine(typing);
        if (charsPerSecond <= 0f)
        {
            bodyText.text = text;
            bodyText.maxVisibleCharacters = int.MaxValue;
            isTyping = false;
        }
        else
        {
            typing = StartCoroutine(TypeLine(text));
        }
    }

    void OnEnded()
    {
        if (typing != null) StopCoroutine(typing);
        isTyping = false;
        panel.SetActive(false);
    }

    /// <summary>
    /// Hook this to the arrow. First press finishes the typewriter; the next press advances.
    /// Called automatically by the Button, or by DialogueArrow for collider-based arrows.
    /// </summary>
    public void OnNextPressed()
    {
        if (isTyping)
        {
            if (typing != null) StopCoroutine(typing);
            bodyText.maxVisibleCharacters = totalChars;
            isTyping = false;
            return;
        }
        manager.Advance();
    }

    IEnumerator TypeLine(string text)
    {
        isTyping = true;
        bodyText.text = text;
        bodyText.maxVisibleCharacters = 0;
        bodyText.ForceMeshUpdate();
        totalChars = bodyText.textInfo.characterCount;

        float shown = 0f;
        while (shown < totalChars)
        {
            shown += charsPerSecond * Time.unscaledDeltaTime;
            bodyText.maxVisibleCharacters = Mathf.Min(totalChars, Mathf.FloorToInt(shown));
            yield return null;
        }

        bodyText.maxVisibleCharacters = totalChars;
        isTyping = false;
    }
}
