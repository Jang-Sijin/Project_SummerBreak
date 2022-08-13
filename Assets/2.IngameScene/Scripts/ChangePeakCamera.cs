using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePeakCamera : MonoBehaviour
{
    [SerializeField] 
    private GameObject _camera;

    public bool triggerd;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !triggerd)
        {
            triggerd = true;
            _camera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && triggerd)
        {
            triggerd = false;
            _camera.SetActive(false);
        }
    }
}
