using System;
using TMPro;
using UnityEngine;

public class UIMinimap : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private float zoomMin = 30; // 카메라의 orthographicSize 최소 크기
    [SerializeField] private float zoomMax = 90; // 카메라의 orthographicSize 최대 크기
    [SerializeField] private float zoomOneStep = 5; // 1회 줌을 할 때 증가/감소되는 수치
    [SerializeField] private TextMeshProUGUI textMapName;

    private void Awake()
    {
        // 맵 이름을 현재 씬 이름으로 설정 (원하는 이름으로 설정)
        textMapName.text = "MiniMap";
    }

    public void ZoomIn()
    {
        // 카메라의 orthographicSize 값을 감소시켜 카메라에 보이는 사물 크기 확대
        minimapCamera.fieldOfView = Mathf.Max(minimapCamera.fieldOfView - zoomOneStep, zoomMin);
    }

    public void ZoomOut()
    {
        // 카메라의 orthographicSize 값을 증가시켜 카메라에 보이는 사물 크기 축소
        minimapCamera.fieldOfView = Mathf.Min(minimapCamera.fieldOfView + zoomOneStep, zoomMax);
    }
}
