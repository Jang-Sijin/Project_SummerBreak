using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI realTimeText;
    [SerializeField] private TextMeshProUGUI gameWorldTimeText;
    [SerializeField] private TextMeshProUGUI playTimeText;
    private float playTime;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(TimeRoutine));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TimeRoutine()
    {
        while (true)
        {
            // 현실 시간 출력
            realTimeText.text = DateTime.Now.ToString("yyyy.MM.dd ") +
                                DateTime.Now.DayOfWeek.ToString().ToUpper().Substring(0, 3) + 
                                DateTime.Now.ToString("\nHH:mm:ss tt"); 
            
            // 플레이 시간, 게임세계 시간 계산 및 출력
            playTime += Time.deltaTime;
            playTimeText.text = playTime.ToString("F1");
            gameWorldTimeText.text = playTime.ToString("F1");

            yield return null;
        }
    }
}
