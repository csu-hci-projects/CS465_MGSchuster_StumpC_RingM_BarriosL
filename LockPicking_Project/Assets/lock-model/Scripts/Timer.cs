using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Timer : MonoBehaviour
{
    public Text lapText;

    private float timeElapsed = 0f;
    private bool isTiming = false;
    private int lapCount = 0;

    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "LapTimes.csv");

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Lap,Time (seconds)\n");
        }

        timeElapsed = 0f;
        isTiming = true;
        Debug.Log("Timer started automatically when scene loaded!");
    }

    void Update()
    {
        if (isTiming)
        {
            timeElapsed += Time.deltaTime;
        }
    }

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
