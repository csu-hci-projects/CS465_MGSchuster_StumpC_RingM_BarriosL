using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Timer : MonoBehaviour
{
    public Text timertex;  // Legacy UI Text component
    public Text lapText;

    private float timeElapsed = 0f;
    private bool isTiming = false;
    private int lapCount = 0;

    private string filePath;

    void Start()
    {
        timertex.text = "0.00";

        filePath = Path.Combine(Application.persistentDataPath, "LapTimes.csv");

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Lap,Time (seconds)\n");
        }
    }

    void Update()
    {
        if (isTiming)
        {
            timeElapsed += Time.deltaTime;
            timertex.text = timeElapsed.ToString("F2");
        }
    }

    // Start button calls this
    public void StartTimer()
    {
        timeElapsed = 0f;
        isTiming = true;
        Debug.Log("Timer started!");
    }

    // Stop button calls this
    public void StopTimer()
    {
        if (!isTiming) return;

        isTiming = false;
        lapCount++;

        float finalLapTime = timeElapsed;
        Debug.Log("Lap " + lapCount + " completed in " + finalLapTime.ToString("F2") + " seconds");

        if (lapText != null)
            lapText.text = "Laps: " + lapCount;

        File.AppendAllText(filePath, lapCount + "," + finalLapTime.ToString("F2") + "\n");
    }
}
