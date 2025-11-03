using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TMP_Text TimerText;
    private float timeElapsed;
    private int minutes, seconds;

    // Update is called once per frame
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        minutes = Mathf.FloorToInt(timeElapsed / 60);
        seconds = Mathf.FloorToInt(timeElapsed % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
