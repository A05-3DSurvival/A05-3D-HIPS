using UnityEngine;

[System.Serializable]
public class LightAnimation
{
    public Light light;
    public Gradient gradient;
    public AnimationCurve animationCurve;
}

public class DayNightCycle : MonoBehaviour
{
    [Range(0f, 1f)] public float time;
    public float fullDayLength;
    public float startTime = 0f;
    private float timeRate;
    private Vector3 noon = new Vector3(90f, 0f, 0f);
    private int day = 1;
    private float previousTime;
    private bool IsSpawn;

    public LightAnimation Sun;
    public LightAnimation Moon;

    [Header("OtherSetting")]
    public AnimationCurve lightingIntensityMultiPlier;
    public AnimationCurve reflectionIntensityMultiPlier;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
        previousTime = time; // 이전 프레임의 time 초기화
    }

    private void Update()
    {
        time = (time + (timeRate * Time.deltaTime)) % 1f;

        // 이전 프레임의 time보다 현재 time이 더 작아졌다면, 새로운 하루가 시작된 것임
        if (time < previousTime)
        {
            day++;
            Debug.Log("New day: " + day);

            IsSpawn = false;
        }
        if (time >= 0.25f && time <= 0.75f && IsSpawn == false)
        {
            IsSpawn = true;
            Enemy.instance.dayStart();
        }


        previousTime = time; // 현재 프레임의 time을 previousTime에 저장

        UpdateLighting(Sun);
        UpdateLighting(Moon);

        RenderSettings.ambientIntensity = lightingIntensityMultiPlier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiPlier.Evaluate(time);
    }

    void UpdateLighting(LightAnimation lightAnimation)
    {
        // animationCurve 에서 time 만큼의 시간에 설정된 값을 intensity에 넣어준다.
        float intensity = lightAnimation.animationCurve.Evaluate(time);

        if (0 < intensity && intensity < 0.001f)
        {
            intensity = 0f;
        }

        // noon은 기준이 되는 태양의 각도
        // 정오시간을 기준으로 noon 은 90 0 0 의 각도를 가져야 하고 time 은 0.5이다.

        lightAnimation.light.transform.eulerAngles = noon * (time - (lightAnimation == Sun ? 0.25f : 0.75f)) * 4;

        lightAnimation.light.color = lightAnimation.gradient.Evaluate(time);
        lightAnimation.light.intensity = intensity;

        GameObject lightObject = lightAnimation.light.gameObject;

        // activeInHierarchy 와 activeSelf 의 차이점
        // activeSelf는 자신은 활성화 되었지만 부모가 비활성화 되서 안보이면 true를 반환하고,
        // activeInHierarchy는 하이어라키엔 비활성화로 표시되어 있어 false 반환
        if (lightAnimation.light.intensity == 0 && lightObject.activeInHierarchy == true)
        {
            lightObject.SetActive(false);
        }
        else if (lightAnimation.light.intensity > 0 && lightObject.activeInHierarchy == false)
        {
            lightObject.SetActive(true);
        }
    }

    public int GetCurrentDay()
    {
        return day;
    }
}
