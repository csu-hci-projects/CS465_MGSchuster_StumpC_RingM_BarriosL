using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Import Unity's UI namespace

public class Timer : MonoBehaviour
{
    public Text timertex;  // Reference to the legacy Text component
    private float timeElapsed = 0f;   // Variable to track the elapsed time

    // Start is called before the first frame update
    void Start()
    {
        // Optionally initialize your text display
        timertex.text = "0.00";
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;  // Increment timeElapsed by the time that has passed since the last frame
        timertex.text = timeElapsed.ToString("F2");  // Update the timer text, formatted to 2 decimal places
    }
}
