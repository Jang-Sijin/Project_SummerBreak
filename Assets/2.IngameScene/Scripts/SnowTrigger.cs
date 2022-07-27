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
    private void Start()
    {
        _playerStatus = GameManager.instance.playerGameObject.GetComponent<PlayerStatus>();
        snowParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerStatus.playerInPeak = true;
            snowParticle.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerStatus.playerInPeak = false;
            snowParticle.Stop();
        }
    }
}
