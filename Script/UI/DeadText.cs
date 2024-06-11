using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadText : MonoBehaviour
{
    public TextMeshProUGUI AliveText;

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

            AliveText.text = "살아남은 시간 : Day: " + dayNightCycle.GetCurrentDay()+ "  " +  hours.ToString("00") + " : " + minutes.ToString("00");
        }
        else
        {
            Debug.LogWarning("DayNightCycle instance not found!");
        }
    }
}
