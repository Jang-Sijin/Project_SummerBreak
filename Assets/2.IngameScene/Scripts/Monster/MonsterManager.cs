using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class MonsterManager : MonoBehaviour
 {
     private float health;
     public float thrust = 5.0f;
     public float knockTime = 4.0f;
     private Rigidbody m_rigidbody;
 
     public bool checkHit = false;
     
     void Start()
     {
         health = 30.0f;
         m_rigidbody = GetComponent<Rigidbody>();
     }
     
     public void TakeHit()
     {
         
         health -= 10.0f;
         Debug.Log($"몬스터 체력: {health}");
         bool isDead = health <= 0;
         if (isDead)
         {
             Die();
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
     private void Die()
     {
         Destroy(gameObject);
     }
     
 }
}

