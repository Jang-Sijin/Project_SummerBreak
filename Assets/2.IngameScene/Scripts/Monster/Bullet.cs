using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    public VisualEffect explodeEffect;

    private PlayerMovement _playerMovement;
    private Rigidbody m_rigidbody;
    
    private bool inCameravisible = false;
    
    private void Start()
    {
        m_rigidbody = transform.GetComponent<Rigidbody>();
        _playerMovement = GameManager.instance.playerGameObject.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        BulletVisible();
    }

    private void BulletVisible()
    {
        //Camera mainCamera = Camera.main;
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        var point = transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                if (inCameravisible)
                {
                    //Debug.Log("[이민호] 밖으로 나감");
                    Destroy(gameObject);
                }

                return;
            }
        }

        inCameravisible = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        VisualEffect newExplodeEffect = Instantiate(explodeEffect, transform.position, transform.rotation);
        newExplodeEffect.Play();
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playerMovement.HitStart(ClamBT.damageValue, m_rigidbody);
        }
        
        Destroy(gameObject);
    }
}
