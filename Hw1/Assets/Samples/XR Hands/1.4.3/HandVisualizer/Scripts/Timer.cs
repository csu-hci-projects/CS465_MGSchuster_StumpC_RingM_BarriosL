using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Import Unity's UI namespace
using System.IO;

public class Timer : MonoBehaviour
{
    public Text timertex;  // Reference to the legacy Text component
    private float timeElapsed = 0f;   // Variable to track the elapsed time
    private bool isTiming = false;
    
    private bool lapStarted = false;
    private int lapCount = 0;
    public Text lapText;

    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        // Optionally initialize your text display
        timertex.text = "0.00";

        filePath = Path.Combine(Application.persistentDataPath, "LapTimes.csv");

        // Create file with headers if it doesn't exist
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Lap,Time (seconds)\n");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTiming){
           timeElapsed += Time.deltaTime;  // Increment timeElapsed by the time that has passed since the last frame
           if (timeElapsed != null) {
                timertex.text = timeElapsed.ToString("F2");  // Update the timer text, formatted to 2 decimal places

            }
        }
    }

    private void onTriggerEvent(Collider other)
    {
        if (other.CompareTag("StartZone"))
        {
            timeElapsed = 0f;
            isTiming = true;
            lapStarted = true;
            Debug.Log("Lap started!");
        }
        else if (other.CompareTag("StopZone")) 
        {
            isTiming = false;
            lapCount++;
            lapStarted = false;

            float finalLapTime = timeElapsed;
            Debug.Log("Lap " + lapCount + " completed in " + finalLapTime.ToString("F2") + " seconds");

            if (lapText != null)
                lapText.text = "Laps: " + lapCount;

            // Append to CSV
            File.AppendAllText(filePath, lapCount + "," + finalLapTime.ToString("F2") + "\n");
        }
    }
        
}
