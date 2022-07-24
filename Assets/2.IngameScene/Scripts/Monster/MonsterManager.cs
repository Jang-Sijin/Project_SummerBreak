using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class MonsterManager : MonoBehaviour
{

    public enum monsterType
    {
        nightMonster,
        green_slimyee,
        red_slimyee,
        green_longyee,
        red_longyee,
        acorn,
        clam
    }

    public monsterType curMonsterType;
    [SerializeField]
    private float health;

    public VisualEffect explodeEffect;
    public float thrust = 5.0f;
    public float knockTime = 4.0f;
    [SerializeField]
    private int inTime;
    private Rigidbody m_rigidbody;
    private SpawnLoot spawnLoot;

    [SerializeField]
    private Vector3 spawnPoint;
    public bool checkHit = false;

    public SkinnedMeshRenderer bodyRenderer;
    public SkinnedMeshRenderer eyeRenderer;
    private Material bodyMaterial;
    private Material eyeMaterial;

    private bool inCameravisible = false;

    [SerializeField] 
    private Animator animator;

    public bool attacking = false;
    //status
    //private float damgeValue;
    //private float speed;

    private bool dead = false;

    
    public GameObject bullet;
    public GameObject bulletTransform;

    private PlayerMovement _playerMovement;

    [SerializeField] 
    private ParticleSystem _Hitparticle;

    [SerializeField] 
    private GameObject damageText;
    
    void Start()
    {
        // HP 01 
        if (curMonsterType == monsterType.green_slimyee || curMonsterType == monsterType.nightMonster 
                                                        || curMonsterType == monsterType.clam)
        {
            health = 10.0f;
        }
        // HP 02
        else if (curMonsterType == monsterType.green_longyee || curMonsterType == monsterType.red_slimyee 
                                                             || curMonsterType == monsterType.acorn)
        {
            health = 20.0f;
        }
        // HP 03
        else if(curMonsterType == monsterType.red_longyee)
        {
            health = 30.0f;
        }
        // slimyee
        if (curMonsterType == monsterType.green_slimyee || curMonsterType == monsterType.red_slimyee 
                                                        || curMonsterType == monsterType.nightMonster)
        {
            //speed = 2.0f;
        }
        // longyee
        else if (curMonsterType == monsterType.green_longyee || curMonsterType == monsterType.red_longyee)
        {
            //speed = 3.0f;
        }
        
        animator = GetComponent<Animator>();
        spawnPoint = this.transform.position;
        m_rigidbody = GetComponent<Rigidbody>();
        spawnLoot = GetComponent<SpawnLoot>();
        bodyMaterial = bodyRenderer.material;
        eyeMaterial = eyeRenderer.material;
        _playerMovement = 
            GameManager.instance.playerGameObject.GetComponent<PlayerMovement>();
        bodyMaterial.SetFloat("RedLv", 0.0f);
        eyeMaterial.SetFloat("RedLv",0.0f);
    }

    private void Update()
    {
        
        if (!dead)
        {
            MonsterVisible();
        }

        
    }

    private void MonsterVisible()
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
                    dead = true;
                    //Debug.Log("[이민호] 밖으로 나감");
                    Destroy(gameObject);
                }

                return;
            }
        }

        inCameravisible = true;
    }
    public void TakeHit()
    {
        float damage = Random.Range(8, 12);

        health -= damage;
        
        //Debug.Log($"[이민호] 몬스터 체력: {health}");

        bool isDead = health <= 0;
        
        if (isDead)
        {
            VisualEffect newExplodeEffect = Instantiate(explodeEffect, transform.position, transform.rotation);
            newExplodeEffect.Play();
            Destroy(newExplodeEffect.gameObject, 0.5f);
            spawnLoot.spawnLoot = true;
        }
        else
        {
            if (damageText)
            {
                ShowDamgeText(damage);
            }
        }
    }

    private void ShowDamgeText(float damgeValue)
    {
        GameObject gameObj = Instantiate(damageText, transform.position, Quaternion.identity, transform);
        
        TextMeshPro textMesh = gameObj.GetComponent<TextMeshPro>();
        
        textMesh.text = damgeValue.ToString();
    }
    
    

    public Vector3 RandomPoint(float range)
    {
        
        Vector3 randomPoint = spawnPoint + Random.insideUnitSphere * range;

        //Debug.DrawRay(randomPoint, Vector3.up, Color.red, 1);
        StartCoroutine(moveCoolTime());
        
        return randomPoint;
    }
    
    private IEnumerator moveCoolTime()
    {
        yield return new WaitForSeconds(3.0f);
    }
    
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(this.transform.position, 6.0f);
        
        //Gizmos.DrawWireSphere(this.transform.position,1.0f);
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                _playerMovement.HitStart(AcornBT.damageValue, m_rigidbody);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Equipment_Attack"))
        {
            checkHit = true;
            ParticleSystem newParticle = Instantiate(_Hitparticle, transform.position,
                transform.rotation);
            newParticle.Play();
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
    private IEnumerator KnockCo(Rigidbody monster)
    {
        if (monster != null)
        {
            yield return new WaitForSeconds(knockTime);
            monster.velocity = Vector3.zero;
        }
    }

    public void DashAttack()
    {
        attacking = true;
        animator.SetBool("Attack", true);
        animator.SetBool("Idle", false);

        StartCoroutine(AttackCo(m_rigidbody));
    }

    public void ShootBullet()
    {
        GameObject newBullet = Instantiate(bullet, bulletTransform.transform.position, transform.rotation);
        Rigidbody newBulletRigid = newBullet.GetComponent<Rigidbody>();
        newBulletRigid.AddForce(transform.forward * 5.0f, ForceMode.Impulse);
    }
    
    private IEnumerator AttackCo(Rigidbody monster)
    {
        if (monster != null)
        {
            yield return new WaitForSeconds(0.5f);
            monster.AddForce(transform.forward * 10.0f, ForceMode.Impulse);
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
    
    
    
}