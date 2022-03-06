using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float thrust = 0.0f;
    public float knockTime = 0.0f;
    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Rigidbody monster = other.GetComponent<Rigidbody>();
            if (monster != null)
            {
                Debug.Log("진짜 됨");
                Vector3 difference = monster.transform.position - transform.position;
                difference = difference.normalized * thrust;
                monster.AddForce(difference,ForceMode.Impulse);
                StartCoroutine(KnockCo(monster));
            }
            
        }
    }

    private IEnumerator KnockCo(Rigidbody monster)
    {
        if (monster != null)
        {
            Debug.Log("됨");
            yield return new WaitForSeconds(knockTime);
            monster.velocity = Vector3.zero;
        }
    }
}