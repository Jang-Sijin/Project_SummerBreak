using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePeakCamera : MonoBehaviour
{
    [SerializeField] 
    private GameObject camera;

    public bool triggerd;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !triggerd)
        {
            triggerd = true;
            camera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && triggerd)
        {
            triggerd = false;
            camera.SetActive(false);
        }
    }
}
