using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DollyCamTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dollyCamCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dollyCamCamera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dollyCamCamera.SetActive(false);
        }
    }
}
