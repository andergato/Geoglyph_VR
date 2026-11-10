using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls round flow for a VR bow game: countdown timer, targets hit, win/lose.
/// Attach to an empty GameObject in the scene (one instance only).
/// Targets call BowGameManager.Instance.RegisterHit() when they get hit.
/// </summary>
public class BowGameManager : MonoBehaviour
{
    public static BowGameManager Instance { get; private set; }

    public enum GameState { Ready, Playing, Won, Lost }

    [Header("Round Settings")]
    [SerializeField] private float roundTime = 60f;       // seconds
    [SerializeField] private int targetsToWin = 10;
    [SerializeField] private string startFlag = "";

    [Header("UI (World-space TextMeshPro for VR)")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text messageText;        // "Win!" / "Time's up!"

    [Header("Events (hook up sounds, spawners, etc.)")]
    public UnityEvent onRoundStart;
    public UnityEvent onWin;
    public UnityEvent onLose;

    public GameState State { get; private set; } = GameState.Ready;
    public int TargetsHit { get; private set; }
    public float TimeRemaining { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ResetRound();
    }

    private void Update()
    {
        if (State != GameState.Playing)
        {
            if(QuestFlags.Has(startFlag)) StartRound();
            return;
        }

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndRound(won: false);
        }

        UpdateUI();
    }

    // ---- Public API ----

    /// <summary>Hook this to a "start" button/target, or call from another script.</summary>
    public void StartRound()
    {
        if (State == GameState.Playing) return;

        ResetRound();
        State = GameState.Playing;
        SetMessage(string.Empty);
        onRoundStart?.Invoke();
    }

    /// <summary>Call from each target's hit trigger.</summary>
    public void RegisterHit()
    {
        if (State != GameState.Playing) return; // ignore hits outside a round

        TargetsHit++;
        UpdateUI();

        if (TargetsHit >= targetsToWin)
            EndRound(won: true);
    }

    /// <summary>Resets to the Ready state without starting.</summary>
    public void ResetRound()
    {
        State = GameState.Ready;
        TargetsHit = 0;
        TimeRemaining = roundTime;
        SetMessage(string.Empty);
        UpdateUI();
    }

    // ---- Internals ----

    private void EndRound(bool won)
    {
        State = won ? GameState.Won : GameState.Lost;
        SetMessage(won ? "You win!" : "Time's up!");
        UpdateUI();

        if (won) onWin?.Invoke();
        else onLose?.Invoke();
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            TimeSpan t = TimeSpan.FromSeconds(Mathf.Max(0f, TimeRemaining));
            timerText.text = $"{t.Minutes:00}:{t.Seconds:00}";
        }

        if (scoreText != null)
            scoreText.text = $"{TargetsHit} / {targetsToWin}";
    }

    private void SetMessage(string msg)
    {
        if (messageText != null) messageText.text = msg;
    }
}
