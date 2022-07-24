using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnowTrigger : MonoBehaviour
{

    [SerializeField] 
    private ParticleSystem snowParticle;

    private void Start()
    {
        snowParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            snowParticle.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            snowParticle.Stop();
        }
    }
}
