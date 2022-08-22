using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyBox : MonoBehaviour
{
    [SerializeField] 
    private SkyboxBlender dayNightSkyboxBlender;

    [SerializeField] 
    private SkyboxBlender peakSkyboxBlender;

    private int curInTime = 0;
    private int lastInTime = 0;

    private bool isChangedSkybox = false;
    private bool isChangedPeak = false;
    private PlayerStatus _playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        _playerStatus = GameManager.instance.playerGameObject.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (!_playerStatus.playerInPeak)
        {
            
            curInTime = GameManager.instance.GetInGameTime().Hour;

            if (curInTime != lastInTime)
            {
                isChangedSkybox = false;
            }
            
            if (isChangedPeak)
            {
                Debug.Log("[이민호] 정상에 나와서 스카이박스 변경");
                isChangedPeak = false;
                dayNightSkyboxBlender.SkyboxBlend(true);
            }
            
            if (!isChangedSkybox && (curInTime == 5 || curInTime == 7 || curInTime == 18 || curInTime == 20))
            {
                Debug.Log("[이민호] 스카이박스 변경");
                isChangedSkybox = true;
                dayNightSkyboxBlender.SkyboxBlend(true);
                lastInTime = curInTime;
            }
            
        }
        else if (!isChangedPeak)
        {
            Debug.Log("[이민호] 정상에 들어가 스카이박스 변경");
            isChangedPeak = true;
            peakSkyboxBlender.SkyboxBlend(true);
        }
    }
}
