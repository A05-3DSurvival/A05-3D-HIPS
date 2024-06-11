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
        previousTime = time; // ���� �������� time �ʱ�ȭ
    }

    private void Update()
    {
        time = (time + (timeRate * Time.deltaTime)) % 1f;

        // ���� �������� time���� ���� time�� �� �۾����ٸ�, ���ο� �Ϸ簡 ���۵� ����
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


        previousTime = time; // ���� �������� time�� previousTime�� ����

        UpdateLighting(Sun);
        UpdateLighting(Moon);

        RenderSettings.ambientIntensity = lightingIntensityMultiPlier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiPlier.Evaluate(time);
    }

    void UpdateLighting(LightAnimation lightAnimation)
    {
        // animationCurve ���� time ��ŭ�� �ð��� ������ ���� intensity�� �־��ش�.
        float intensity = lightAnimation.animationCurve.Evaluate(time);

        if (0 < intensity && intensity < 0.001f)
        {
            intensity = 0f;
        }

        // noon�� ������ �Ǵ� �¾��� ����
        // �����ð��� �������� noon �� 90 0 0 �� ������ ������ �ϰ� time �� 0.5�̴�.

        lightAnimation.light.transform.eulerAngles = noon * (time - (lightAnimation == Sun ? 0.25f : 0.75f)) * 4;

        lightAnimation.light.color = lightAnimation.gradient.Evaluate(time);
        lightAnimation.light.intensity = intensity;

        GameObject lightObject = lightAnimation.light.gameObject;

        // activeInHierarchy �� activeSelf �� ������
        // activeSelf�� �ڽ��� Ȱ��ȭ �Ǿ����� �θ� ��Ȱ��ȭ �Ǽ� �Ⱥ��̸� true�� ��ȯ�ϰ�,
        // activeInHierarchy�� ���̾��Ű�� ��Ȱ��ȭ�� ǥ�õǾ� �־� false ��ȯ
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
