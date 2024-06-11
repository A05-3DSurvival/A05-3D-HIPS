using TMPro;
using UnityEngine;

public class DateAndTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    private DayNightCycle dayNightCycle;

    // Start is called before the first frame update
    void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeAndDate();
    }

    // 시간과 날짜를 업데이트하는 함수
    void UpdateTimeAndDate()
    {
        if (dayNightCycle != null)
        {
            float currentTime = dayNightCycle.time;

            float realTime = currentTime * 24f;
            int hours = Mathf.FloorToInt(realTime);
            int minutes = Mathf.FloorToInt((realTime - hours) * 60f);

            timeText.text = hours.ToString("00") + ":" + minutes.ToString("00");

            dateText.text = "Day: " + dayNightCycle.GetCurrentDay() + "day";

        }
        else
        {
            Debug.LogWarning("DayNightCycle instance not found!");
        }
    }
}
