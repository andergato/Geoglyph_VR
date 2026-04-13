using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsScript : MonoBehaviour
{
    public Text fpsText; // Assign a UI Text element to this in the Inspector

    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float fps = 0.0f;
    private float updateInterval = 0.5f; // Update every 0.5 seconds

    void Update()
    {
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;

        // Update FPS only at the defined interval
        if (deltaTime > updateInterval)
        {
            fps = frameCount / deltaTime;
            frameCount = 0;
            deltaTime -= updateInterval;

            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
        }
    }
}