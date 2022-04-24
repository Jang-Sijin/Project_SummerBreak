using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTirgger : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Debug.Log("[이민호] 카메라에서 사라짐");
    }

    private void OnBecameVisible()
    {
        Debug.Log("[이민호] 카메라에 있음");
    }
}
