using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnowTrigger : MonoBehaviour
{

    [SerializeField] 
    private ParticleSystem snowParticle;

    private PlayerStatus _playerStatus;

    private bool startSnow = false;
    private void Start()
    {
        _playerStatus = GameManager.instance.playerGameObject.GetComponent<PlayerStatus>();
        snowParticle.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!startSnow && other.gameObject.CompareTag("Player"))
        {
            startSnow = true;
            _playerStatus.playerInPeak = true;
            snowParticle.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            startSnow = false;
            _playerStatus.playerInPeak = false;
            snowParticle.Stop();
        }
    }
}
