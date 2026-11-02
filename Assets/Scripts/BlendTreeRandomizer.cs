using UnityEngine;

public class BlendTreeRandomizer : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int totalClips = 3;

    [SerializeField] private float changeInterval = 5f; // Change animation every 5 seconds
    private float timer;

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = changeInterval; // Initialize timer
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TriggerRandomClip();
            timer = changeInterval; // Reset the countdown timer
        }
    }

    public void TriggerRandomClip()
    {
        float randomBlend = Random.Range(0, totalClips); // Max is exclusive for ints, ensures 0, 1, or 2
        animator.SetFloat("BlendIndex", randomBlend);
    }
}
