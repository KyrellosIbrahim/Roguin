using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public float timeElapsed = 0f;
    public TextMeshProUGUI timeText;

    void Update()
    {
        timeElapsed += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        timeText.text = "Time: " +string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}