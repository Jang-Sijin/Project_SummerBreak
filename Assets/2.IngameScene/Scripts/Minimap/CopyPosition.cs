using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private bool x, y, z; // 이 값이 true이면 target의 좌표, false이면 현재 좌표를 그대로 사용한다.
    [SerializeField]
    private Transform target; // 쫒아가야할 대상 Transform

    [SerializeField] private int minimapCameraHeight = 15;

    private void Update()
    {
        // 쫒아가야할 대상이 없으면 종료
        if (!target)
            return;

        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y + minimapCameraHeight : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
}
