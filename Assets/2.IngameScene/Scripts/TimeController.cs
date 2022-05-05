using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    [Header("↓ 게임에서의 시간 속도 (실제 흐르는 시간 * TimeMultiplier)")]
    [SerializeField]
    private float timeMultiplier;

    [Header("↓게임이 시작되었을 때 시간")]
    [SerializeField]
    private float startHour;

    [Header("↓현재 게임의 시간 TEXT")]
    [SerializeField]
    private TextMeshProUGUI timeText;

    [Header("↓해가 떠있는 시간의 조명(Light)")]
    [SerializeField]
    private Light sunLight;

    [Header("↓일출 시간")]
    [SerializeField]
    private float sunriseHour;

    [Header("↓일몰 시간")]
    [SerializeField]
    private float sunsetHour;

    [Header("↓해가 떠있는 시간의 빛 색상")]
    [SerializeField]
    private Color dayAmbientLight;

    [Header("↓달이 떠있는 시간의 빛 색상")]
    [SerializeField]
    private Color nightAmbientLight;

    [Header("↓Light이 변환될 때 ")]
    [SerializeField]
    private AnimationCurve lightChangeCurve;

    [Header("↓최대 태양 조명 강도 [0~1]")]
    [SerializeField]
    private float maxSunLightIntensity;

    [Header("↓밤 조명(Light)")]
    [SerializeField]
    private Light moonLight;

    [Header("↓최대 달 조명 강도 [0~1]")]
    [SerializeField]
    private float maxMoonLightIntensity;

    private DateTime currentTime;

    private TimeSpan sunriseTime;

    private TimeSpan sunsetTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
    
    public void SetInGameTime(int hour)
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(hour);
    }
    
    public DateTime InGameTime()
    {
        return currentTime;
    }

    public void SetTimeMultiplier(int timeSpeed)
    {
        timeMultiplier = timeSpeed;
    }
}