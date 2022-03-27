using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

namespace Monster
{
    public class MonsterManager : MonoBehaviour
 {
     private float health;
     public VisualEffect explodeEffect;
     public float thrust = 5.0f;
     public float knockTime = 4.0f;
     private Rigidbody m_rigidbody;
    
     
     public bool checkHit = false;
     
     
     
     void Start()
     {
         health = 100.0f;
         m_rigidbody = GetComponent<Rigidbody>();
         
     }
     
     public void TakeHit()
     {
         
         health -= 10.0f;
         Debug.Log($"몬스터 체력: {health}");
         bool isDead = health <= 0;
         
         if (isDead)
         {
             VisualEffect newExplodeEffect = Instantiate(explodeEffect, transform.position, transform.rotation);
             newExplodeEffect.Play();
             Destroy(newExplodeEffect.gameObject,0.5f);
             
             Destroy(gameObject);
         }
         
     }
     private void OnTriggerEnter(Collider other)
     {
         if (other.gameObject.CompareTag("Equipment_Attack"))
         {
             checkHit = true;
         }
     }

     public void KnockBack()
     {
         Rigidbody player = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
         if (player != null)
         {
             Vector3 difference = transform.position - player.transform.position;
             difference = difference.normalized * thrust;
             m_rigidbody.AddForce(difference,ForceMode.Impulse);
             StartCoroutine(KnockCo(m_rigidbody));
         }
     }
     
 
     private IEnumerator KnockCo(Rigidbody monster)
     {
         if (monster != null)
         {
             yield return new WaitForSeconds(knockTime);
             monster.velocity = Vector3.zero;
         }
     }

 }
}

