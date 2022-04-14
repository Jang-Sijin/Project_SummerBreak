using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class MonsterManager : MonoBehaviour
{

    public enum monsterType
    {
        nightMonster,
        slimyee,
        longyee
    }

    public monsterType curMonsterType;
    [SerializeField]
    private float health;

    [SerializeField] 
    private bool sociality = false;
    public VisualEffect explodeEffect;
    public float thrust = 5.0f;
    public float knockTime = 4.0f;
    [SerializeField]
    private int inTime;
    private Rigidbody m_rigidbody;
    private SpawnLoot spawnLoot;
    
    
    public bool checkHit = false;

    public SkinnedMeshRenderer bodyRenderer;
    public SkinnedMeshRenderer eyeRenderer;
    private Material bodyMaterial;
    private Material eyeMaterial;
    void Start()
    {
        if (curMonsterType == monsterType.slimyee)
        {
            health = 15.0f;
        }
        else if (curMonsterType == monsterType.longyee)
        {
            health = 30.0f;
            sociality = true;
        }
        else
        {
            health = 10.0f;
            sociality = true;
        }
        m_rigidbody = GetComponent<Rigidbody>();
        spawnLoot = GetComponent<SpawnLoot>();
        bodyMaterial = bodyRenderer.material;
        eyeMaterial = eyeRenderer.material;
        bodyMaterial.SetFloat("RedLv", 0.0f);
        eyeMaterial.SetFloat("RedLv",0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 6.0f);
    }
    public void TakeHit()
    {

        health -= 10.0f;
        Debug.Log($"[이민호] 몬스터 체력: {health}");
        bool isDead = health <= 0;

        if (isDead)
        {
            VisualEffect newExplodeEffect = Instantiate(explodeEffect, transform.position, transform.rotation);
            newExplodeEffect.Play();
            Destroy(newExplodeEffect.gameObject, 0.5f);
            spawnLoot.spawnLoot = true;
        }

    }

    public bool GetSociality()
    {
        return sociality;
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
            m_rigidbody.AddForce(difference, ForceMode.Impulse);
            StartCoroutine(KnockCo(m_rigidbody));
        }
    }

    public void MonsterHitStart()
    {
        bodyMaterial.SetFloat("RedLv", 0.1f);
        eyeMaterial.SetFloat("RedLv",0.1f);
    }

    public void MonsterHitEnd()
    {
        bodyMaterial.SetFloat("RedLv", 0.0f);
        eyeMaterial.SetFloat("RedLv",0.0f);
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